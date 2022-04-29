#if UNITY_EDITOR
using MyDebugger.Elements;
using UnityEditor;
using UnityEngine;

namespace MyDebugger.Styles {
    [System.Serializable]
    public class Normal_Style_MyDebugger : Styles_MyDebugger {
        private static Normal_Style_MyDebugger instance = null;
        public static Normal_Style_MyDebugger Instance { get { if (instance == null) instance = new Normal_Style_MyDebugger(); return instance; } }

        private static GUIStyle normalStyle = null;


        public override Rect Print(LogElement messageStringPayload) {
            try {

                string message = messageStringPayload.Payload;
                GUIStyle normal = GetGUIStyle(message, ref normalStyle, MyDebugWindow.MyDebugNormalFont);
                normal.contentOffset = new Vector2(MyDebugWindow.ContentOffset.x + 10, 0);
                if (GUILayout.Button(message, normal, GUILayout.Height(normal.fixedHeight), GUILayout.ExpandHeight(false))) {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<MonoScript>(messageStringPayload.Path), messageStringPayload.Line);
                }
                Rect rect = GUILayoutUtility.GetLastRect();
                return rect;
            }
            catch {

            }
            return new Rect();
        }
    }
    //public class Header_Style_MyDebugger : Styles_MyDebugger {
    //    private static Header_Style_MyDebugger instance = null;
    //    public static Header_Style_MyDebugger Instance { get { if (instance == null) instance = new Header_Style_MyDebugger(); return instance; } }
    //    private static GUIStyle headerGUIStyle = null;

    //    public override Rect Print(LogElement messageStringPayload) {
    //        try {
    //            string message = messageStringPayload.Payload;
    //            GUIStyle header = GetGUIStyle(message, ref headerGUIStyle, MyDebugWindow.MyDebugBoldFont, GUIColors.White, GUIColors.Black, GUIColors.GreyLight, true, TextAnchor.MiddleCenter);
    //            Rect rect = EditorGUILayout.GetControlRect(true, header.fixedHeight, header, GUILayout.Height(header.fixedHeight), GUILayout.ExpandHeight(false));
    //            EditorGUILayout.LabelField(message, header, GUILayout.Height(header.fixedHeight), GUILayout.ExpandHeight(false));

    //        }
    //        catch {
    //        }
    //        return new Rect();
    //    }
    //}
}
#endif
