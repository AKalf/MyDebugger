using MyDebugger.Elements;
using MyDebugger;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyDebugger.Storage;

#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using System.Diagnostics;
public class MyDebugWindow : EditorWindow {

    public static MyDebugWindow Window;

    static Vector2 mainScroll = Vector2.zero;
    static Font myDebugNormalFont = null;
    static Font myDebugBoldFont = null;
    public static Font MyDebugNormalFont { get { if (myDebugNormalFont == null) myDebugNormalFont = Resources.Load<Font>("RobotoFont/Roboto-Regular"); return myDebugNormalFont; } }
    public static Font MyDebugBoldFont { get { if (myDebugBoldFont == null) myDebugBoldFont = Resources.Load<Font>("RobotoFont/Roboto-Medium"); return myDebugBoldFont; } }

    public static Vector2 ContentOffset = Vector2.zero;

    public static Queue<LogFolder_Element_MyDebugger> foldersToDraw = new Queue<LogFolder_Element_MyDebugger>();

    private static int maxFiles = 10;
    private static Rect fileRect = new Rect();
    private static int lastIndex = 0;
    [MenuItem("Window/MyDebugWindow")]
    static void Init() {
        Window = EditorWindow.GetWindow<MyDebugWindow>();
    }
    void OnInspectorUpdate() {
        if (Window == null) {
            Window = EditorWindow.GetWindow<MyDebugWindow>();
            maxFiles = (int)position.height;
        }
    }
    void OnGUI() {
        if (MyDebug.Instance.Root == null)
            return;

        if (GUILayout.Button("Clear all logs"))
            Storage_MyDebugger.ClearLogs();
        foldersToDraw.Clear();
        ContentOffset = Vector2.zero;
        ContentOffset.y = 3;

        mainScroll = EditorGUILayout.BeginScrollView(mainScroll);
        MyDebug.Instance.Root.Style.Print(MyDebug.Instance.Root);
        if (MyDebug.Instance.Root.IsOpen) {
            ContentOffset.x += Consts.TabOffset;
            foreach (LogFile_Element_MyDebugger file in MyDebug.Instance.Root.Files) {
                file.Style.Print(file);
            }
            DrawFolders(MyDebug.Instance.Root.GetChildFolders());
        }
        EditorGUILayout.EndScrollView();
    }
    private void DrawFolders(LogFolder_Element_MyDebugger[] folders) {
        float folderContentOffset = ContentOffset.x;
        if (folders.Length > 0) {
            foreach (LogFolder_Element_MyDebugger folder in folders) {
                if (folder.files.Count > 0) {
                    folder.Style.Print(folder);
                    if (folder.IsOpen) {
                        ContentOffset.x += Consts.TabOffset;
                        foreach (LogFile_Element_MyDebugger file in folder.Files) {
                            fileRect = file.Style.Print(file);
                        }

                        DrawFolders(folder.GetChildFolders());
                        ContentOffset.x = folderContentOffset;
                    }
                }
            }
        }
    }

    public static class Consts {
        public static float TabOffset = 15.0f;
        public static float FolderLineHeight = 1.5f;
        public static float ElementsVerticalSpacing = 2.5f;
    }
}
#endif

namespace MyDebugger {
    public class MyDebug {
        private static MyDebug instance = null;
        public static MyDebug Instance { get { if (instance == null) instance = new MyDebug(); return instance; } }
#if UNITY_EDITOR
        public LogFolder_Element_MyDebugger Root => Storage.Storage_MyDebugger.Instance.Root;
#else
        public LogFolder_Element_MyDebugger Root => null;
#endif
        private int _currentLogID = 0;
        public int GetNewID => _currentLogID += 1;
        public int PeekID => _currentLogID;

        // They are repeated because of stack trace
        public static void Log(string message, LogFolder_Element_MyDebugger folder, Styles_MyDebugger style, params MonoBehaviour[] objectsToPing) {
#if UNITY_EDITOR
            StackFrame callStack = new StackFrame(1, true);
            string messageProper = "[" + System.DateTime.Now + "]\n\n" + message + "\nPath: " + callStack.GetFileName() + ", at line: " + callStack.GetFileLineNumber();
            LogFile_Element_MyDebugger newLogFile = new LogFile_Element_MyDebugger(Instance.GetNewID, messageProper, style, objectsToPing, folder, callStack.GetFileName(), callStack.GetFileLineNumber());
#endif
        }
        public static void Log(string message, Styles_MyDebugger style, params MonoBehaviour[] objectsToPing) {
#if UNITY_EDITOR
            StackFrame callStack = new StackFrame(1, true);
            string messageProper = "[" + System.DateTime.Now + "]\n\n" + message + "\nPath: " + callStack.GetFileName() + ", at line: " + callStack.GetFileLineNumber();
            LogFile_Element_MyDebugger newLogFile = new LogFile_Element_MyDebugger(Instance.GetNewID, messageProper, style, objectsToPing, Instance.Root, callStack.GetFileName(), callStack.GetFileLineNumber());
#endif
        }
        public static void Log(string message, LogFolder_Element_MyDebugger folder, params MonoBehaviour[] objectsToPing) {
#if UNITY_EDITOR
            StackFrame callStack = new StackFrame(1, true);
            string messageProper = "[" + System.DateTime.Now + "]\n\n" + message + "\nPath: " + callStack.GetFileName() + ", at line: " + callStack.GetFileLineNumber();
            LogFile_Element_MyDebugger newLogFile = new LogFile_Element_MyDebugger(Instance.GetNewID, messageProper, Styles_MyDebugger.Normal, objectsToPing, folder, callStack.GetFileName(), callStack.GetFileLineNumber());
#endif
        }
        public static void Log(string message, params MonoBehaviour[] objectsToPing) {
#if UNITY_EDITOR
            StackFrame callStack = new StackFrame(1, true);
            string messageProper = "[" + System.DateTime.Now + "]\n\n" + message + "\nPath: " + callStack.GetFileName() + ", at line: " + callStack.GetFileLineNumber();
            LogFile_Element_MyDebugger newLogFile = new LogFile_Element_MyDebugger(Instance.GetNewID, messageProper, Styles_MyDebugger.Normal, objectsToPing, Instance.Root, callStack.GetFileName(), callStack.GetFileLineNumber());
#endif
        }
        public static LogFolder_Element_MyDebugger GetFolder(string name, LogFolder_Element_MyDebugger parent, Styles_MyDebugger style = null) {
#if UNITY_EDITOR
            LogFolder_Element_MyDebugger result = FolderStructure<LogFolder_Element_MyDebugger>.GetElementByPath(parent.Path + "/" + name, Instance.Root);
            if (result == null)
                result = new LogFolder_Element_MyDebugger(name, parent, style);
            return result;
#else
            return null;
#endif
        }


    }
}
