using MyDebugger;
using MyDebugger.Elements;
using UnityEngine;

public class Test : MonoBehaviour {
    [SerializeField]
    UnityEngine.UI.Button b = null;

    LogFolder_Element_MyDebugger folderA;
    LogFolder_Element_MyDebugger folderB;
    LogFolder_Element_MyDebugger folderC;
    LogFolder_Element_MyDebugger folderD;

    void Start() {
        b.onClick.AddListener(print);
        folderA = MyDebug.GetFolder("A Folder", MyDebug.Instance.Root);


        folderB = MyDebug.GetFolder("B Folder", folderA);


        folderC = MyDebug.GetFolder("C Folder", folderA);


        folderD = MyDebug.GetFolder("D Folder", folderC);

    }

    // Update is called once per frame
    void Update() {

    }
    private void print() {

        for (int i = 0; i < 1000; i++) {
            int lines = Random.Range(1, 30);
            string debugMessage = "";
            for (int j = 0; j < lines; j++) {
                debugMessage += j + " line -------------------\n";
            }
            MyDebug.Log(debugMessage, style: Styles_MyDebugger.Normal, this);
        }

    }
}
