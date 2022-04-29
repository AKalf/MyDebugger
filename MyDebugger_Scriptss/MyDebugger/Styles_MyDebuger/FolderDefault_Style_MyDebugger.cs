#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using MyDebugger.Elements;
namespace MyDebugger.Styles {
    public class FolderDefault_Style : Styles_MyDebugger {
        private static FolderDefault_Style instance = null;
        public static FolderDefault_Style Instance { get { if (instance == null) instance = new FolderDefault_Style(); return instance; } }
        private static GUIStyle folderDefaultGUIStyle = null;
        public override void Print<T>(T folderPayload) {
            folderDefaultGUIStyle = new GUIStyle(EditorStyles.foldout);
            LogFolder_Element_MyDebugger folder = folderPayload as LogFolder_Element_MyDebugger;
            GUIStyle folderStyle = GetGUIStyle(folder.GetCleanName(), ref folderDefaultGUIStyle, MyDebugWindow.MyDebugNormalFont, GUIColors.White, GUIColors.Black, GUIColors.GreyLight, false, TextAnchor.MiddleLeft);
            folderStyle.fontSize = 16;
            folderStyle.margin = new RectOffset((int)MyDebugWindow.ContentOffset.x, 0, 5, 5);
            folder.IsOpen = EditorGUILayout.Foldout(folder.IsOpen, folder.CleanName, folderStyle);
        }
    }
}
#endif
