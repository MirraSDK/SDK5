/**
 * Allocates a UTF-8 encoded copy of a JS string in the WASM linear memory.
 *
 * The caller is responsible for freeing the returned pointer with _free()
 * unless it is returned directly from a jslib function (Unity marshals those
 * automatically).
 *
 * @param  {string|null|undefined} text
 * @return {number} Pointer to the allocated buffer, or 0 for null/undefined.
 */
Module.allocateString = function (text) {
    if (text === null || text === undefined) {
        return 0; // null pointer — safe to pass to C# as IntPtr.Zero
    }
    text = String(text); // coerce non-string values defensively
    const bufferSize = lengthBytesUTF8(text) + 1;
    const buffer = _malloc(bufferSize);
    stringToUTF8(text, buffer, bufferSize);
    return buffer;
}

/**
 * Invokes a Unity C# callback from JavaScript.
 *
 * Supports every Unity WebGL version from 2022 through 6.3+ by probing the
 * available Emscripten runtime API in priority order:
 *
 *   1. getWasmTableEntry  — Unity 6 / Emscripten ≥ 3.1.44 (WebAssembly Table)
 *   2. wasmTable.get      — direct WebAssembly.Table access (global or Module)
 *   3. makeDynCall        — mid-range Emscripten (~3.1.x)
 *   4. dynCall            — classic Emscripten (Unity 2022–2023)
 *   5. Module.dynCall_*   — oldest Emscripten legacy path
 *
 * When the callback is a function pointer (number), any string arguments are
 * temporarily allocated in WASM memory and freed after the call, since dynCall
 * and WebAssembly Table calls only accept numeric (int/pointer) arguments.
 *
 * When the callback is a string (exported C function name), Module.ccall is
 * used instead — it handles string marshaling automatically.
 *
 * @param {number}          senderId  Prepended as the first argument.
 * @param {number|string}   callback  Function pointer or exported name.
 * @param {...*}             args      Additional arguments (number|string|boolean).
 */
Module.invokeMonoPCallback = function (senderId, callback, ...args) {
    try {
        // ── Guard: missing or null callback ──────────────────────────
        if (callback === null || callback === undefined) {
            console.warn('invokeMonoPCallback: callback is null/undefined, skipping');
            return;
        }
        // ── Coerce all arguments ─────────────────────────────────────
        // Prepend senderId, then normalise types:
        //   boolean  → 0 | 1
        //   null/undefined → 0
        //   everything else kept as-is (number or string)
        const argsWithId = [senderId, ...args];
        const coercedArgs = argsWithId.map((value) => {
            if (value === null || value === undefined) return 0;
            if (typeof value === 'boolean') return value ? 1 : 0;
            return value;
        });
        // ── Branch: pointer-based callback vs. named export ──────────
        const callbackIsPointer = (typeof callback === 'number');
        if (callbackIsPointer) {
            // Null function pointer guard (index 0 in WebAssembly Table is null).
            if (callback === 0) {
                console.warn('invokeMonoPCallback: callback pointer is 0 (null), skipping');
                return;
            }
            // dynCall / wasmTable only accept numeric args.  Any string
            // value must first be allocated as a WASM pointer.  We track
            // those allocations so we can free them after the call.
            const allocatedPtrs = [];
            const ptrArgs = coercedArgs.map((value) => {
                if (typeof value === 'string') {
                    const ptr = Module.allocateString(value);
                    allocatedPtrs.push(ptr);
                    return ptr;
                }
                return value;
            });
            // Signature for dynCall / makeDynCall: void + N × int
            const signature = 'v' + ptrArgs.map(() => 'i').join('');
            try {
                // 1) Unity 6+ / Emscripten ≥ 3.1.44: getWasmTableEntry
                //    Returns the raw WASM function; call with args only.
                if (typeof getWasmTableEntry === 'function') {
                    const fn = getWasmTableEntry(callback);
                    if (typeof fn !== 'function') {
                        throw new Error('getWasmTableEntry(' + callback + ') did not return a function');
                    }
                    fn.apply(null, ptrArgs);
                    return;
                }
                // 2) Direct WebAssembly.Table (global wasmTable or Module.wasmTable)
                const table = (typeof wasmTable !== 'undefined' && wasmTable !== null)
                    ? wasmTable
                    : (Module.wasmTable || null);
                if (table) {
                    const fn = table.get(callback);
                    if (typeof fn !== 'function') {
                        throw new Error('wasmTable.get(' + callback + ') did not return a function');
                    }
                    fn.apply(null, ptrArgs);
                    return;
                }
                // 3) makeDynCall — returns a trampoline: makeDynCall(sig)(ptr, ...args)
                const makeDynCallFn = (typeof makeDynCall === 'function')
                    ? makeDynCall
                    : (typeof Module.makeDynCall === 'function' ? Module.makeDynCall : null);
                if (makeDynCallFn) {
                    makeDynCallFn(signature).apply(null, [callback].concat(ptrArgs));
                    return;
                }
                // 4) dynCall (classic): dynCall(sig, ptr, argsArray)
                const dynCallFn = (typeof dynCall === 'function')
                    ? dynCall
                    : (typeof Module.dynCall === 'function' ? Module.dynCall : null);
                if (dynCallFn) {
                    dynCallFn(signature, callback, ptrArgs);
                    return;
                }
                // 5) Module['dynCall_<sig>'] (oldest legacy path)
                const dynCallMethodName = 'dynCall_' + signature;
                const dynCallMethod = Module[dynCallMethodName];
                if (typeof dynCallMethod === 'function') {
                    dynCallMethod.apply(null, [callback].concat(ptrArgs));
                    return;
                }
                throw new Error(
                    'No method available to invoke function pointer. ' +
                    'Tried: getWasmTableEntry, wasmTable, makeDynCall, dynCall, ' +
                    dynCallMethodName
                );
            } finally {
                // Free every temporarily-allocated string pointer.
                for (let i = 0; i < allocatedPtrs.length; i++) {
                    if (allocatedPtrs[i] !== 0) {
                        try { _free(allocatedPtrs[i]); } catch (_) { /* _free may not be exported */ }
                    }
                }
            }
        } else if (typeof callback === 'string') {
            // Named exported function — ccall handles string marshaling automatically.
            if (typeof Module.ccall !== 'function') {
                throw new Error('Module.ccall is not available to invoke named callback "' + callback + '"');
            }
            const argTypes = coercedArgs.map((value) => {
                return (typeof value === 'string') ? 'string' : 'number';
            });
            Module.ccall(
                callback,    // The exported C function name
                null,        // Return type (void)
                argTypes,    // Argument types
                coercedArgs  // Argument values
            );
        } else {
            throw new Error(
                'Callback must be a number (function pointer) or string (exported name), got: ' + typeof callback
            );
        }
    }
    catch (exception) {
        console.error("faulty or missing callback", exception);
    }
}