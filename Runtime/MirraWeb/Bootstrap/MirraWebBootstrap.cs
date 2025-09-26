using AOT;
using MirraGames.SDK.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.MirraWeb {

    [Provider(typeof(IBootstrap))]
    public class MirraWebBootstrap : CommonBootstrap {

        [DllImport(Naming.InternalDll)] private static extern void createMirraSDK(string configurationJson, DelegateVoid onInstance);

        [MonoPInvokeCallback(typeof(DelegateVoid))]
        private static void OnCreateInstance(int senderId) {
            try {
                OnInstanceCallbacks.PopInvokeAll();
            }
            catch (Exception exception) {
                Logger.CreateError(nameof(MirraWebBootstrap), nameof(OnCreateInstance), exception);
            }
        }

        private static readonly Stack<Action> OnInstanceCallbacks = new();

        public MirraWebBootstrap(PreferencesReader preferencesReader) {
            OnInstanceCallbacks.Push(SetInitialized);
            PlatformSettings platformSettings = new(preferencesReader);
            string platformSettingsJson = JsonUtility.ToJson(platformSettings);
            if (string.IsNullOrEmpty(platformSettingsJson)) {
                platformSettingsJson = Naming.EmptyJson;
            }
            Logger.CreateText(this, platformSettingsJson);
            createMirraSDK(platformSettingsJson, OnCreateInstance);
        }

    }

}