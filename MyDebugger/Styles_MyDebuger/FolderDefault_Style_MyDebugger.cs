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
            string folderName = " ";
            for (int i = 0; i < MyDebugWindow.ContentOffset.x / (folderStyle.fontSize / 3); i++)
                folderName += '-';
            folderName += " " + folder.CleanName;
            folder.IsOpen = EditorGUILayout.Foldout(folder.IsOpen, folderName, folderStyle);
        }

    }
}
#endif
