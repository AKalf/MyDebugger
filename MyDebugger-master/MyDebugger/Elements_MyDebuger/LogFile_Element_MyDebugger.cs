#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.Events;

public interface LogElement { 
    string Payload { get; }
    string Path { get; }
    int Line { get; }
}
namespace MyDebugger.Elements {
    [Serializable]
    public struct LogFile_Element_MyDebugger : LogElement {

        [SerializeField]
        private int logId;
        public int LogID => logId;

        [SerializeField]
        private string payload;
        public string Payload => payload;

        [SerializeField]
        private int styleID;
        [NonSerialized]
        private Styles_MyDebugger style;
        public Styles_MyDebugger Style {
            get { if (style == null) style = Styles_MyDebugger.GetStyleByID(styleID); return style; }
        }
        [SerializeField]
         string pathToLog;
        public string Path => pathToLog;
     
        [SerializeField]
        int line;
        public int Line => line;
       
        [SerializeField]
        private MonoBehaviour[]
        pings;
        public MonoBehaviour[] Pings => pings;

        public LogFile_Element_MyDebugger(int ID, string message, Styles_MyDebugger style, MonoBehaviour[] objToPing, LogFolder_Element_MyDebugger parentFolder, string path, int line) {
            logId = ID;
            pathToLog = "";
            if (string.IsNullOrEmpty(path) == false) {
                string[] splitPath = path.Split('\\');
                bool relativePathFound = false;
                for (int i = 0; i < splitPath.Length - 1; i++) {
                    if (splitPath[i] == "Assets" || relativePathFound) {
                        pathToLog += splitPath[i] + '/';
                        relativePathFound = true;
                    }
                }
                pathToLog += splitPath[splitPath.Length - 1];
            }
            this.line = line;
            payload = message;
            this.style = style;
            styleID = Styles_MyDebugger.GetStyleID(style);
            pings = objToPing;
            parentFolder.Files.Add(this);
        }
    }
}
#endif
