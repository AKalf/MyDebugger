#if UNITY_EDITOR
using UnityEditor;
using MyDebugger.Styles;
#endif
using System.Collections.Generic;
using UnityEngine;
using MyDebugger.Elements;

namespace MyDebugger {
    public abstract class Styles_MyDebugger {

#if UNITY_EDITOR
        //public static Styles_MyDebugger Header => Header_Style_MyDebugger.Instance;
        public static Styles_MyDebugger Normal => Normal_Style_MyDebugger.Instance;
        public static Styles_MyDebugger FolderDefault => FolderDefault_Style.Instance;

        protected const float TextLineHeightOffset = 10.0f;

        protected float lastBackgounrdHeight = -1;
        protected float lastBackgroundWidth = -1;
        protected float lastContentOffset = 0;
        static Texture2D background = null;

        private static readonly Dictionary<Styles_MyDebugger, int> registeredStylesByInstance = new Dictionary<Styles_MyDebugger, int>() {
            {Normal, 0 },/* {Header, 1 },*/ {FolderDefault, 2 }

        };
        private static readonly Dictionary<int, Styles_MyDebugger> registeredStyleByID = new Dictionary<int, Styles_MyDebugger>() {
            {0,Normal }, /* {Header, 1 },*/ {2, FolderDefault }

        };

        protected GUIStyle GetGUIStyle(string message, ref GUIStyle style, Font font, Color textColor, Color backgroundColor, Color borderColor, bool drawBackground = true, TextAnchor textAnchor = TextAnchor.MiddleLeft) {
            if (style == null) {
                style = new GUIStyle();
            }
            style.font = font;
            style.normal.textColor = textColor;
            style.alignment = textAnchor;
            style.stretchHeight = false;
            style.richText = true;
            if (MyDebugWindow.Window == null)
                return style;
            if (drawBackground)
                CalculateAndSetBackground(message, style, backgroundColor, borderColor);
            return style;
        }
        protected GUIStyle GetGUIStyle(string message, ref GUIStyle style, Font font, Color textColor, Color backgroundColor, bool drawBackground = true, TextAnchor textAnchor = TextAnchor.MiddleLeft) {
            return GetGUIStyle(message, ref style, font, textColor, backgroundColor, GUIColors.GreyLightDark, drawBackground, textAnchor);
        }
        protected GUIStyle GetGUIStyle(string message, ref GUIStyle style, Font font, Color textColor, bool drawBackground = true, TextAnchor textAnchor = TextAnchor.MiddleLeft) {
            return GetGUIStyle(message, ref style, font, textColor, GUIColors.GreyDark, GUIColors.GreyLightDark, drawBackground, textAnchor);
        }
        protected GUIStyle GetGUIStyle(string message, ref GUIStyle style, Font font, bool drawBackground = true, TextAnchor textAnchor = TextAnchor.MiddleLeft) {
            return GetGUIStyle(message, ref style, font, GUIColors.White, GUIColors.GreyDark, GUIColors.GreyLightDark, drawBackground, textAnchor);
        }



        private void CalculateAndSetBackground(string message, GUIStyle style, Color backgroundColor, Color borderColor) {
            if (message == null)
                Debug.LogError("NULL MESSAGE");
            if (style == null)
                Debug.LogError("NULL Style");
            if (style.font == null)
                style.font = MyDebugWindow.MyDebugNormalFont;
            int lineHeight;
            if (message.Split('\n') != null)
                lineHeight = style.font.lineHeight * (message.Split('\n').Length + 3);
            else
                lineHeight = style.font.lineHeight;
            int lineWidth = (int)MyDebugWindow.Window.position.width;
            if (lastBackgounrdHeight != lineHeight || lastBackgroundWidth != lineWidth || lastContentOffset != MyDebugWindow.ContentOffset.x || background == null) {
                lastBackgroundWidth = lineWidth;
                lastBackgounrdHeight = lineHeight;
                lastContentOffset = MyDebugWindow.ContentOffset.x;
                style.fixedHeight = lineHeight + 20;
                background = GetBackgroundForStyle(lineWidth, lineHeight, backgroundColor, borderColor);
            }
            style.normal.background = background;
        }


        public abstract Rect Print(LogElement payload);
        public static int GetStyleID(Styles_MyDebugger style) {
            if (registeredStylesByInstance.ContainsKey(style))
                return registeredStylesByInstance[style];
            else {
                Debug.LogError("Unregisted MyDebuggerStyle: " + style.GetType().Name);
                return 0;
            }
        }
        public static Styles_MyDebugger GetStyleByID(int styleID) {
            if (registeredStyleByID.ContainsKey(styleID))
                return registeredStyleByID[styleID];
            else {
                Debug.LogError("There is no MyDebugger.Style with ID: " + styleID);
                return null;
            }
        }

        private static Texture2D GetBackgroundForStyle(int lineWidth, int lineHeight, Color backgroundColor, Color borderColor) {


            if (background == null) {
                background = new Texture2D(lineWidth, lineHeight);
                Color[] pix = background.GetPixels();
                int line = 0;
                float offset = MyDebugWindow.ContentOffset.x;
                for (int i = 0; i < pix.Length; i++) {
                    if (i < line * lineWidth + offset) {
                        pix[i] = GUIColors.DefaultGrey;
                    }
                    else if (i < lineWidth + 1 + offset || i > pix.Length - (lineWidth + 1) || i == line * lineWidth + offset)
                        pix[i] = borderColor;
                    else
                        pix[i] = backgroundColor;
                    if (i >= line * lineWidth + lineWidth - 1)
                        line++;
                }
                background.SetPixels(pix);
                background.Apply();
            }
            return background;
        }



        protected static class GUIColors {
            public readonly static Color White = new Color(1, 1, 1);
            public readonly static Color Black = new Color(0, 0, 0);

            public readonly static Color Red = new Color(1, 0, 0);
            public readonly static Color Blue = new Color(0, 0, 0.8f);
            public readonly static Color GreyLight = new Color(0.85f, 0.85f, 0.85f);
            public readonly static Color GreyLightDark = new Color(0.4f, 0.4f, 0.4f);
            public readonly static Color Grey = new Color(0.7f, 0.7f, 0.7f);
            public readonly static Color GreyDark = new Color(0.1f, 0.1f, 0.1f);
            public readonly static Color DefaultGrey = EditorGUIUtility.isProSkin ? new Color(0.22F, 0.22F, 0.22F) : new Color(0.76f, 0.76f, 0.76f);

        }
#else
        public static Styles_MyDebugger Header => null;
        public static Styles_MyDebugger Normal => null;
        public static Styles_MyDebugger FolderDefault => null;
#endif
    }
}
