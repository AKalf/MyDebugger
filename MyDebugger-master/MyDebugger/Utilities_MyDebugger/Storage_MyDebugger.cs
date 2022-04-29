#if UNITY_EDITOR

using MyDebugger.Elements;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyDebugger.Storage {
    [CreateAssetMenu]
    [InitializeOnLoad]
    public class Storage_MyDebugger : ScriptableObject {


        private static Storage_MyDebugger instance = null;
        public static Storage_MyDebugger Instance { get { return instance; } }

        private static bool hasBuilded = false;

        [SerializeField]
        private List<LogFolder_Element_MyDebugger> serializedFolders;
        public const string DEFAULT_PATH_TO_DATABASE = "Assets/MyDebugger-master/Resources";
        [SerializeField]
        private LogFolder_Element_MyDebugger root;
        public LogFolder_Element_MyDebugger Root {
            get {
                if (instance == null) {
                    instance = AssetDatabase.LoadAssetAtPath<Storage_MyDebugger>("Assets/MyDebugger_Scripts/Storage_MyDebugger");
                }
                if (instance.root == null)
                    instance.root = new LogFolder_Element_MyDebugger("Root", null);
                return instance.root;
            }
        }

        public void Awake() {
            if (instance == null)
                instance = AssetDatabase.LoadAssetAtPath<Storage_MyDebugger>("Assets/MyDebugger_Scripts/Storage_MyDebugger");
            if (instance.serializedFolders == null)
                instance.serializedFolders = new List<LogFolder_Element_MyDebugger>();
            if (instance.root == null)
                instance.root = new LogFolder_Element_MyDebugger("Root", null);
            else if (!hasBuilded) {
                instance.root = FolderStructure<LogFolder_Element_MyDebugger>.BuildOrederedFolderStructure(serializedFolders, root);
                hasBuilded = true;
            }
        }
        static Storage_MyDebugger() {
            EditorApplication.update += OnUnityLaunch;
        }
        private static void OnUnityLaunch() {
            if (instance == null) {
                string[] guids = AssetDatabase.FindAssets("t:" + nameof(Storage_MyDebugger));
                if (guids.Length == 0) {
                    Debug.LogWarning("No Debugger Databases found, creating one...");
                    AssetDatabase.CreateAsset(new Storage_MyDebugger(), DEFAULT_PATH_TO_DATABASE);
                    guids = AssetDatabase.FindAssets("t:" + nameof(Storage_MyDebugger));
                }
                if (guids.Length > 1)
                    Debug.LogError("Multiple Debugger Databases found!");
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                instance = AssetDatabase.LoadAssetAtPath<Storage_MyDebugger>(path);
            }
            if (instance.serializedFolders == null)
                instance.serializedFolders = new List<LogFolder_Element_MyDebugger>();
            if (instance.root == null)
                instance.root = new LogFolder_Element_MyDebugger("Root", null);
            else if (!hasBuilded) {
                instance.root = FolderStructure<LogFolder_Element_MyDebugger>.BuildOrederedFolderStructure(instance.serializedFolders, instance.root);
                hasBuilded = true;
            }
        }
        public Storage_MyDebugger() {
            if (instance == null)
                instance = this;
            if (instance.serializedFolders == null)
                instance.serializedFolders = new List<LogFolder_Element_MyDebugger>();
            if (instance.root == null) {
                instance.root = new LogFolder_Element_MyDebugger("Root", null);
                instance.root = FolderStructure<LogFolder_Element_MyDebugger>.BuildOrederedFolderStructure(serializedFolders, root);
            }
            if (!hasBuilded) {
                instance.root = FolderStructure<LogFolder_Element_MyDebugger>.BuildOrederedFolderStructure(serializedFolders, root);
                hasBuilded = true;
            }
        }
        public static void AddFolder(LogFolder_Element_MyDebugger folder) {
            instance.serializedFolders.Add(folder);
        }
        public static void AddFile(LogFile_Element_MyDebugger file) {
            AddFile(file, Instance.Root);
        }
        public static void AddFile(LogFile_Element_MyDebugger file, LogFolder_Element_MyDebugger folder) {
            if (instance.serializedFolders.Contains(folder) == false)
                instance.serializedFolders.Add(folder);
            folder.files.Add(file);
        }
        public static void ClearLogs() {
            if (Application.isPlaying == false) {
                instance.serializedFolders = new List<LogFolder_Element_MyDebugger>();
                instance.root = new LogFolder_Element_MyDebugger("Root", null);
            }
            else {
                instance.serializedFolders.ForEach(folder => folder.Files.Clear());
            }

            EditorUtility.SetDirty(Instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }
}
#endif