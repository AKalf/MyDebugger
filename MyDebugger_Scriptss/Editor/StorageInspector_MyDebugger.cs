using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyDebugger.Elements;
namespace MyDebugger.Storage.EditorScripts {
    [CustomEditor(typeof(Storage_MyDebugger))]
    public class StorageInspector_MyDebugger : Editor {

        public override void OnInspectorGUI() {
            serializedObject.Update();
            if (Storage_MyDebugger.Instance.Root == null)
                return;
            EditorGUILayout.LabelField(Storage_MyDebugger.Instance.Root.CleanName + ", files: " + Storage_MyDebugger.Instance.Root.Files.Count);
            foreach (LogFolder_Element_MyDebugger folder in Storage_MyDebugger.Instance.Root.GetChildFolders()) {
                EditorGUILayout.LabelField(folder.CleanName + ", files: " + folder.Files.Count);
                DrawFolders(folder.GetChildFolders());
            }
        }
        private void DrawFolders(LogFolder_Element_MyDebugger[] folders) {
            foreach (LogFolder_Element_MyDebugger folder in folders) {
                EditorGUILayout.LabelField(folder.CleanName + ", files: " + folder.Files.Count);
                DrawFolders(folder.GetChildFolders());
            }

        }
    }


}
