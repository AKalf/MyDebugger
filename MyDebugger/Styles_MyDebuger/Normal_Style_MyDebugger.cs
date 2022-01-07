#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MyDebugger.Styles {
    [System.Serializable]
    public class Normal_Style_MyDebugger : Styles_MyDebugger {
        private static Normal_Style_MyDebugger instance = null;
        public static Normal_Style_MyDebugger Instance { get { if (instance == null) instance = new Normal_Style_MyDebugger(); return instance; } }

        private static GUIStyle normalStyle = null;


        public override void Print<MessageString>(MessageString messageStringPayload) {
            try {
                string message = messageStringPayload as string;
                GUIStyle normal = GetGUIStyle(message, ref normalStyle, MyDebugWindow.MyDebugNormalFont);
                normal.contentOffset = new Vector2(MyDebugWindow.ContentOffset.x + 10, 0);
                EditorGUILayout.LabelField(message, normal, GUILayout.Height(normal.fixedHeight), GUILayout.ExpandHeight(false));
            }
            catch { }
        }
    }
}
#endif
