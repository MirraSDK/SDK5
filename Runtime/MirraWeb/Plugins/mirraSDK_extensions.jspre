Module.allocateString = function (text) {
    const bufferSize = lengthBytesUTF8(text) + 1;
    const buffer = _malloc(bufferSize);
    stringToUTF8(text, buffer, bufferSize);
    return buffer;
}

Module.invokeMonoPCallback = function (senderId, callback, ...args) {
    try {
        // Append the id to the beginning of arguments.
        const argsWithId = [senderId, ...args];
        // Convert booleans to 0/1. Keep numbers & strings as is.
        const coercedArgs = argsWithId.map((value) => {
            if (typeof value === 'boolean') return value ? 1 : 0;
            return value;
        });
        // Map arguments to their types for the signature.
        const signature = 'v' + coercedArgs.map(value => {
            if (typeof value === 'number') return 'i';
            if (typeof value === 'string') return 's';
            throw new Error('Unsupported argument type: ' + typeof value);
        }).join('');
        // Check if 'callback' is a pointer (number) or a string name of an exported function.
        const callbackIsPointer = (typeof callback === 'number');
        if (callbackIsPointer) {
            if (typeof Module.makeDynCall === 'function') {
                makeDynCall(signature, callback, coercedArgs);
            }
            else if (typeof Module.dynCall === 'function') {
                dynCall(signature, callback, coercedArgs);
            }
            else {
                // Construct the dynCall_* method name based on the signature
                const dynCallMethodName = `dynCall_${signature}`;
                const dynCallMethod = Module[dynCallMethodName];
                if (typeof dynCallMethod === 'function') {
                    // Invoke the callback using the specific dynCall_* method
                    dynCallMethod(callback, ...coercedArgs);
                } else {
                    throw new Error(`Dynamic call method ${dynCallMethodName} not found in Module. Signature ${signature} may not be supported.`);
                }
            }
        } else if (typeof Module !== 'undefined' && typeof Module.ccall === 'function') {
            // If callback is a string, we can use ccall
            const argTypes = coercedArgs.map(value => {
                return (typeof value === 'string') ? 'string' : 'number';
            });
            Module.ccall(
                callback,   // The exported C function name (string)
                null,       // Return type (null = void)
                argTypes,   // Array of argument types
                coercedArgs // Array of argument values
            );
        } else {
            throw new Error("No valid method to invoke Unity callback found");
        }
    }
    catch (exception) {
        console.error("faulty or missing callback", exception);
    }
}