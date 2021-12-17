using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshManagerHR : MonoBehaviour
{
    GameObject tm;
    //tmの意味はテキストメッシュ

    /// <summary>
    /// CrackLength.csからjosnstrをもらうための準備
    /// </summary>
    private GameObject recCrackHRObj;
    private RecCrackHR RCHRScript;
    public string DisplayText = "null";



    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.Find("TextMeshObj");

        ///emptyオブジェクトのRecCrackHRObjを取得
        recCrackHRObj = GameObject.Find("RecCrackHRObj");
        RCHRScript = recCrackHRObj.GetComponent<RecCrackHR>();
    }


    // Update is called once per frame
    void Update()
    {
        //ToStringのカッコ内のF以降の数が小数第何位まで表示するかの数
        tm.GetComponent<TextMesh>().text = RCHRScript.lineLength.ToString("F5");
    }
}
