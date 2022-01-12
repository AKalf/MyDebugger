
using System.Collections.Generic;
using UnityEngine;

namespace MyDebugger.Storage {
    public static class FolderStructure<FolderType> where FolderType : FolderStructure<FolderType>.IFolder {

        public interface IFolder {
            string[] GetChildrenCleanNames();
            string GetCleanName();

            FolderType[] GetChildFolders();
            void AddSubFolder(FolderType folder);
        }
#if UNITY_EDITOR
        public static FolderType BuildOrederedFolderStructure(List<FolderType> inputList, FolderType inputRoot) {
            List<FolderType> copyInput = new List<FolderType>();
            inputList.ForEach(folder => copyInput.Add(folder));

            FolderType root = inputRoot;
            FolderType currentFolder = root;

            Queue<FolderType> foldersToSearch = new Queue<FolderType>();
            foldersToSearch.Enqueue(currentFolder);

            if (copyInput.Contains(root))
                copyInput.Remove(root);
            while (foldersToSearch.Count > 0) {
                currentFolder = foldersToSearch.Dequeue();
                foreach (string childFolderName in currentFolder.GetChildrenCleanNames()) {
                    FolderType childFolder = CheckFolderNames(copyInput, childFolderName);
                    if (childFolder != null) {
                        currentFolder.AddSubFolder(childFolder);
                        copyInput.Remove(childFolder);
                        foldersToSearch.Enqueue(childFolder);
                    }
                }
            }
            return root;
        }
        public static FolderType GetElementByPath(string path, FolderType root) {
            string[] folderNames = path.Split('/');
            FolderType currentNode = root;
            for (int i = 1; i < folderNames.Length; i++) {
                foreach (FolderType child in currentNode.GetChildFolders()) {
                    if (child.GetCleanName() == folderNames[i])
                        currentNode = child;
                }
                if (i == folderNames.Length - 1 && folderNames[i] == currentNode.GetCleanName())
                    return currentNode;
                else {
                    //Debug.LogError ("Folder at path: " + path + " could not be found. Make sure that the folder exists in the DB and that the foldering structured has been build");
                }
            }
            return default;
        }

        private static FolderType CheckFolderNames(List<FolderType> folders, string name) {
            foreach (FolderType folder in folders) {
                if (folder.GetCleanName() == name) {
                    return folder;
                }
                else
                    return default;
            }
            return default;
        }
#endif
    }
}
