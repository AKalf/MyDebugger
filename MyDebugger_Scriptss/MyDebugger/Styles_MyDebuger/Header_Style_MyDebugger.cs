using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MyDebugger.Styles {
    public class Header_Style_MyDebugger : Styles_MyDebugger {
        private static Header_Style_MyDebugger instance = null;
        public static Header_Style_MyDebugger Instance { get { if (instance == null) instance = new Header_Style_MyDebugger(); return instance; } }
        private static GUIStyle headerGUIStyle = null;

        public override void Print<T>(T messageStringPayload) {
            try {
                string message = messageStringPayload as string;
                GUIStyle header = GetGUIStyle(message, ref headerGUIStyle, MyDebugWindow.MyDebugBoldFont, GUIColors.White, GUIColors.Black, GUIColors.GreyLight, true, TextAnchor.MiddleCenter);
                EditorGUILayout.LabelField(message, header, GUILayout.Height(header.fixedHeight), GUILayout.ExpandHeight(false));
            }
            catch { }
        }
    }
}
#endif
