
using System;
using System.Collections.Generic;
using UnityEngine;
using MyDebugger.Storage;

namespace MyDebugger.Elements {
    [Serializable]
    public class LogFolder_Element_MyDebugger : FolderStructure<LogFolder_Element_MyDebugger>.IFolder {
        [SerializeField]
        private string path;
        public string Path => path;

        [SerializeField]
        private string cleanName = null;

        public string CleanName {
            get {
                if (cleanName == null) {
                    string[] path = Path.Split('/');
                    cleanName = path[path.Length - 1];
                }
                return cleanName;
            }
        }

        [SerializeField]
        private int styleID;
#if UNITY_EDITOR
        [NonSerialized]
        private Styles_MyDebugger style;
        public Styles_MyDebugger Style {
            get { if (style == null) style = Styles_MyDebugger.GetStyleByID(styleID); return style; }
        }
#endif

        [SerializeField]
        private List<string> childrenNames = new List<string>();
        public List<string> ChildrenNames => childrenNames;


#if UNITY_EDITOR
        [SerializeField]
        public List<LogFile_Element_MyDebugger> files;
        public List<LogFile_Element_MyDebugger> Files => files;
#endif
        [NonSerialized]
        public List<LogFolder_Element_MyDebugger> ChildFolders = new List<LogFolder_Element_MyDebugger>();

        [NonSerialized]
        public LogFolder_Element_MyDebugger Parent = null;

        public bool IsOpen { get; set; }

        public LogFolder_Element_MyDebugger(string folderName, LogFolder_Element_MyDebugger parentFolder, Styles_MyDebugger style = null) {
#if UNITY_EDITOR
            if (style == null)
                style = Styles_MyDebugger.FolderDefault;
            this.style = style;
            styleID = Styles_MyDebugger.GetStyleID(style);

            cleanName = folderName;
            if (cleanName == "Root") {
                path = "Root";
                parentFolder = null;
            }
            else {
                path = (parentFolder == null ? "Root/" : parentFolder.path + "/") + cleanName;
                if (parentFolder == null)
                    parentFolder = Storage_MyDebugger.Instance.Root;
                parentFolder.AddSubFolder(this);
            }

            files = new List<LogFile_Element_MyDebugger>();
            IsOpen = false;
            Storage_MyDebugger.AddFolder(this);
#endif
        }

        public string[] GetChildrenCleanNames() {
            return ChildrenNames.ToArray();
        }

        public string GetCleanName() {
            return CleanName;
        }

        public LogFolder_Element_MyDebugger[] GetChildFolders() {
            if (ChildFolders == null)
                ChildFolders = new List<LogFolder_Element_MyDebugger>();
            return ChildFolders.ToArray();
        }

        public void AddSubFolder(LogFolder_Element_MyDebugger folder) {
            if (ChildFolders.Contains(folder) == false) {
                folder.Parent = this;
                ChildrenNames.Add(folder.CleanName);
                ChildFolders.Add(folder);
            }
        }

    }
}

