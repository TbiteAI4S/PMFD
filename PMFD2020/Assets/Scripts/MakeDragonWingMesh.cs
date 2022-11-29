using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDragonWingMesh : MonoBehaviour
{
    Curve curvescript;
    //ドラゴンボーンのデータ
    DragonBorneData dbd;
    WingBorneData wbd;

    /* パラメータ */
    WingParameter wingParameter;

    /* ドラゴンのボーンデータ */
    Vector3[] wing_position =
    {
        //翼の下腕
        new Vector3(-1.2f,-0.2f,0.0f),
        new Vector3(0.0f,3.0f,0.0f),
        //翼の上腕
        new Vector3(0.0f,3.0f,0.0f),
        new Vector3(1.2f,6.0f,0.0f),
        //翼の指中央1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(3.9f,5.7f,0.0f),
        //翼の指中央2
        new Vector3(3.9f,5.7f,0.0f),
        new Vector3(7.7f,4.4f,0.0f),
        //翼の指内1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(2.2f,3.7f,0.0f),
        //翼の指内2
        new Vector3(2.2f,3.7f,0.0f),
        new Vector3(4.6f,1.3f,0.0f),
        //翼の指外1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(5.2f,8.0f,0.0f),
        //翼の指外2
        new Vector3(5.2f,8.0f,0.0f),
        new Vector3(9.4f,7.5f,0.0f),
        //////////////////////////////////
        // ↑16個の点
        // ↓翼膜用に追加の4点（胴体側から順に）
        //翼膜1
        new Vector3(-1.2f,-0.2f,0.0f),  //翼の下腕の根本
        new Vector3(4.6f,1.3f,0.0f),    //翼の指内2の先
        //翼膜2
        new Vector3(4.6f,1.3f,0.0f),    //翼の指内2の先
        new Vector3(7.7f,4.4f,0.0f),    //翼の指中央2の先
        //翼膜3
        new Vector3(7.7f,4.4f,0.0f),    //翼の指中央2の先
        new Vector3(9.4f,7.5f,0.0f),    //翼の指外2の先
    };

    //端点での速度
    Vector3[] wing_velocity =
    {
        //翼の下腕
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の上腕
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指中央1
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指中央2
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指内1
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指内2
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指外1
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指外2
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //////////////////////////////////
        // ↑16個の点
        // ↓翼膜用に追加の4点（胴体側から順に）
        //翼膜1
        new Vector3(1.0f, 0.0f, 0.0f),  //翼の下腕の根本
        new Vector3(1.0f, 0.0f, 0.0f),   //翼の指内2の先
        //翼膜2
        new Vector3(1.0f, 0.0f, 0.0f),    //翼の指内2の先
        new Vector3(1.0f, 0.0f, 0.0f),    //翼の指中央2の先
        //翼膜3
        new Vector3(1.0f, 0.0f, 0.0f),    //翼の指中央2の先
        new Vector3(1.0f, 0.0f, 0.0f),    //翼の指外2の先
    };

    //張り
    float[] wing_tension =
    {   /* 始点, 終点 */
        //翼の上腕
        1.0f,1.0f,
        1.0f,1.0f,
        //翼の指中央
        1.0f,1.0f,
        1.0f,1.0f,
        //翼の指内
        1.0f,1.0f,
        1.0f,1.0f,
        //翼の指外
        1.0f,1.0f,
        1.0f,1.0f,
        //////////////////////////////////
        // ↑16個の点
        // ↓翼膜用に追加の4点（胴体側から順に）
        //翼膜1
        1.0f,   //翼の下腕の根本
        1.0f,   //翼の指内2の先
        //翼膜2
        1.0f,   //翼の指内2の先
        1.0f,   //翼の指中央2の先
        //翼膜3
        1.0f,   //翼の指中央2の先
        1.0f,   //翼の指外2の先
    };

    //方向
    float[] wing_direction =
    {   /* 始点, 終点 */
        //翼の上腕
        0.0f,0.0f,
        0.0f,0.0f,
        //翼の指中央
        0.0f,0.0f,
        0.0f,0.0f,
        //翼の指内
        0.0f,0.0f,
        0.0f,0.0f,
        //翼の指外
        0.0f,0.0f,
        0.0f,0.0f,
        //////////////////////////////////
        // ↑16個の点
        // ↓翼膜用に追加の4点（胴体側から順に）
        //翼膜1
        0.0f,   //翼の下腕の根本
        0.0f,   //翼の指内2の先
        //翼膜2
        0.0f,   //翼の指内2の先
        0.0f,   //翼の指中央2の先
        //翼膜3
        0.0f,   //翼の指中央2の先
        0.0f,   //翼の指外2の先
    };

    //翼膜の頂点の分割数
    const int step1_2_3_4 = 5;
    const int stepwing5_6_7 = 9;

    //翼膜の頂点
    Vector3[][] wingVertex;      //最初に生成    [11][5]
    Vector3[][] wing_membrane;   //Mesh用の頂点  [7 ][9]

    //翼膜のメッシュ
    Mesh wing_membraneMesh;

    //翼膜の頂点を作成
    /*
     * velocityを計算して曲線を作りboneArrayに渡す
     */
    void makewingVertex(Vector3[][] wingVertexArray)
    {
        for (int j = 0; j < 22; j = j + 2)//始点と終点のvelocityの計算をパーツの個数分行う
        {
            //ドラゴンボーン作成
            wing_velocity[j].x = wing_tension[j] * Mathf.Cos(wing_direction[j]);
            wing_velocity[j].y = wing_tension[j] * Mathf.Sin(wing_direction[j]);
            wing_velocity[j + 1].x = wing_tension[j + 1] * Mathf.Cos(wing_direction[j + 1]);
            wing_velocity[j + 1].y = wing_tension[j + 1] * Mathf.Sin(wing_direction[j + 1]);

            Vector3[] p = new Vector3[step1_2_3_4];
            for (int i = 0; i < step1_2_3_4; i++)//stepの回数分曲線の点を作る
            {
                float t = i / (float)(step1_2_3_4 - 1.0f);
                p[i] = curvescript.curve(wing_position[j], wing_velocity[j], wing_position[j + 1], wing_velocity[j + 1], t);
            }
            //曲線を渡す
            wingVertexArray[j / 2] = p;
        }
    }

    //翼膜の頂点からMeshの頂点に変換
    /*
     * 最初の配列からMesh用の配列に変換
     */
    void makeMeshVertex(Vector3[][] wingVertexArray, Vector3[][] wing_membraneArray)
    {
        //wingVertexの j と j+1 の配列を1つにする
        //配列 j+1 の初めは j の最後と同じなため統合する
        //対象は翼膜1～3以外
        for(int i = 0; i < 5; i++)
        {
            int j = 2 * i;
            Vector3[] wing = new Vector3[wingVertexArray[j].Length + wingVertexArray[j + 1].Length - 1];
            Array.Copy(wingVertexArray[j], wing, wingVertexArray[j].Length);
            Array.Copy(wingVertexArray[j + 1], 1, wing, wingVertexArray[j].Length, wingVertexArray[j + 1].Length - 1);
            //Mesh用配列に渡す
            wing_membraneArray[i] = wing;
        }
        //翼膜1～3をMesh用に渡す
        for(int i = 0; i < 3; i++)
        {
            int j = i + 8;
            wing_membraneArray[i] = wingVertexArray[j];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //curveのスクリプト取得
        curvescript = GameObject.Find("Curve").GetComponent<Curve>();

        //ドラゴンボーンのデータのスクリプトを取得
        dbd = GameObject.Find("Doragon").GetComponent<DragonBorneData>();
        wbd = GameObject.Find("Doragon").GetComponent<WingBorneData>();

        //パラメータスクリプトの取得
        wingParameter = GameObject.Find("wingPanel").GetComponent<WingParameter>();

        //配列の初期化
        wingVertex = new Vector3[11][];
        wing_membrane = new Vector3[7][];

        //ドラゴンボーンの作成
        makewingVertex(wingVertex);
        for(int i = 0; i < wingVertex[0].Length; i++)
        {
            Debug.Log("wingVertex" + wingVertex[0][i]);
        }
        

        //メッシュの作成
        //wing_membraneMesh = new Mesh();
    }

    // Update is called once per frame
    void Update()
    {
        //WingParameterの更新
        for (int i = 0; i < 16; i++)
        {
            wing_position[i] = wingParameter.left_wing_position[i];
            wing_tension[i] = wingParameter.wing_tension[i];
            wing_direction[i] = wingParameter.wing_direction[i];
        }
    }
}
