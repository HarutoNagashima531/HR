using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class RecCrackHR : MonoBehaviour
{
    /// <summary>
    /// 変化点を保持するリスト型変数
    /// </summary>
    private List<CrackPoint> cPoints = null;

    [SerializeField]
    private GameObject linePrefab;

    /// <summary>
    /// スタートボタンが押されているか
    /// </summary>
    private bool ChalkingStatus = false;

    private CrackInfoObject crackInfoObject;

    //jsonに変換する前の文字列
    public string jsonstr = "";
    // 線の全体の長さ、このスクリプトでの計算とTextMeshManager.csに渡すための変数
    public double lineLength = 0d;





    // Start is called before the first frame update
    void Start()
    {
        crackInfoObject = new CrackInfoObject();
        crackInfoObject.crackInfos = new List<CrackInfo>();
        jsonstr = "";

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// エアタップしたらこの関数を呼ぶ、オブジェクト「Point」の座標と角度を記録
    /// </summary>
    public void recCoordinate()
    {
        ///オブジェクト「Point」の座標と角度を代入
        Vector3 Pos = GameObject.Find("Point").transform.position;
        Quaternion Rot = GameObject.Find("Point").transform.rotation;

        ///変化点を保持するリストに追加
        cPoints.Add(new CrackPoint()
        {
            x = Pos.x,
            y = Pos.y,
            z = Pos.z,
        });


    }

    public void startChalking()
    {
        ChalkingStatus = true;
        cPoints = new List<CrackPoint>();
        Debug.Log("記録を開始。");

    }

    public void finishChalking()
    {
        if (cPoints == null || cPoints.Count == 1)
        {
            Debug.Log("スタートボタンを押してください。また、1つ以上の点を選択する必要があります。");
        }
        else
        {
            // 設定されたひび幅の値(単位：mm)
            //ここにつまみで設定したヒビの幅を入れる
            double Width = 0.1;

            lineLength = 0d;

            for (int i = 0; i < cPoints.Count - 1; i++)
            {
                lineLength += Vector3.Distance(new Vector3(cPoints[i].x, cPoints[i].y, cPoints[i].z), new Vector3(cPoints[i + 1].x, cPoints[i + 1].y, cPoints[i + 1].z)) * 1000d;//* 1000dで単位をmmにしている
            }

            CrackInfo crackInfo = new CrackInfo()
            {
                // 単位：mm
                CrackWidth = Width,
                CrackLength = lineLength,
                crackPoint = cPoints,
            };

            // 最終的に保存する配列にチョーキング情報を追加
            crackInfoObject.crackInfos.Add(crackInfo);

            string json = JsonUtility.ToJson(crackInfo);
            //Debug.Log(json);

            // 変化点同士をつないで線を描く処理
            // ここでの線の幅はピクセル
            // mmではないので注意
            Draw(cPoints, Width);

            ChalkingStatus = false;
            cPoints = null;

            Debug.Log("チョーキング完了");
        }
    }

    /// <summary>
    /// オブジェクトの配列をJson形式で保存する処理
    /// </summary>
    public void SaveCrackInformations()
    {
        if (crackInfoObject.crackInfos.Count == 0)
        {
            Debug.Log("チョーキングデータが存在しません");
        }
        else
        {
            StreamWriter writer;
            //string adress = Application.dataPath + "/crackdata.json";
            string adress = "U:/Users/ikeda/AppData/Local/Packages/Template3D_pzq3xp76mxafg/LocalState" + "/crackdata.json";


            // Json形式にシリアライズ
            jsonstr = JsonUtility.ToJson(crackInfoObject, true);

            if (!File.Exists(adress))
            {
                writer = File.CreateText(adress);
                writer.Flush();
                writer.Dispose();
            }

            // Jsonデータを保存する処理
            writer = new StreamWriter(adress, false);
            writer.Write(jsonstr);
            writer.Flush();
            writer.Close();

            Debug.Log("出力完了");
        }
    }

    public void resetscene()
    {
        var clones = GameObject.FindGameObjectsWithTag("BlueBall");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }

        crackInfoObject = new CrackInfoObject();
        crackInfoObject.crackInfos = new List<CrackInfo>();


    }


    /// <summary>
    /// // 変化点同士をつないで線を描く処理
    /// </summary>
    /// <param name="points"></param>
    private void Draw(List<CrackPoint> cPoints, double Width)
    {
        GameObject line = Instantiate(linePrefab);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

        // 線の太さを設定
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;

        // 選択された変化点を設定
        lineRenderer.positionCount = cPoints.Count;
        // リスト型を配列にする
        Vector3[] positions = new Vector3[cPoints.Count];
        for (int i = 0; i < cPoints.Count; i++)
        {
            
            positions[i] = new Vector3(cPoints[i].x, cPoints[i].y, cPoints[i].z);
        }

        lineRenderer.SetPositions(positions);
    }



}


/// <summary>
/// 変化点を保存するクラス
/// </summary>
[System.Serializable]
public class CrackPoint
{
    public float x;
    public float y;
    public float z;
}


/// <summary>
/// ヒビ1つ分のヒビの情報
/// </summary>
[System.Serializable]
public class CrackInfo
{
    public double CrackWidth = 1;
    public double CrackLength;
    public List<CrackPoint> crackPoint;
}


/// <summary>
/// 全てのヒビのデータをまとめるためのクラス
/// </summary>
[System.Serializable]
public class CrackInfoObject
{
    public List<CrackInfo> crackInfos;
}
