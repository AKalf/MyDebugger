#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.Events;

namespace MyDebugger.Elements {
    [Serializable]
    public struct LogFile_Element_MyDebugger {

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
        private MonoBehaviour[]
        pings;
        public MonoBehaviour[] Pings => pings;

        public LogFile_Element_MyDebugger(int ID, string message, Styles_MyDebugger style, MonoBehaviour[] objToPing, LogFolder_Element_MyDebugger parentFolder) {
            logId = ID;
            payload = message;
            this.style = style;
            styleID = Styles_MyDebugger.GetStyleID(style);
            pings = objToPing;
            parentFolder.Files.Add(this);
        }
    }
}
#endif
