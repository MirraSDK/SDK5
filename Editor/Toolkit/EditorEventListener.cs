using System;
using UnityEditor;
using UnityEditor.Compilation;

namespace MirraGames.SDK.Editor {

    [InitializeOnLoad]
    internal static class EditorEventListener {

        public static event Action OnCompilationStarted;
        public static event Action OnCompilationFinished;
        public static event Action OnImportPackageStarted;
        public static event Action OnImportPackageFinished;

        static EditorEventListener() {
            CompilationPipeline.compilationStarted += CompilationStarted;
            CompilationPipeline.compilationFinished += CompilationFinished;
            AssetDatabase.importPackageStarted += ImportPackageStarted;
            AssetDatabase.importPackageCompleted += ImportPackageFinished;
            AssetDatabase.importPackageCancelled += ImportPackageFinished;
            AssetDatabase.importPackageFailed += ImportPackageFailed;
        }

        private static void CompilationStarted(object obj) {
            OnCompilationStarted?.Invoke();
        }

        private static void CompilationFinished(object obj) {
            OnCompilationFinished?.Invoke();
        }

        private static void ImportPackageStarted(string packageName) {
            OnImportPackageStarted?.Invoke();
        }

        private static void ImportPackageFinished(string packageName) {
            OnImportPackageFinished?.Invoke();
        }

        private static void ImportPackageFailed(string packageName, string errorMessage) {
            ImportPackageFinished(packageName);
        }

    }

}