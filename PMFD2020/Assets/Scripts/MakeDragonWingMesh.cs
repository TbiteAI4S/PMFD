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
        //翼の指内1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(2.2f,3.7f,0.0f),
        //翼の指内2
        new Vector3(2.2f,3.7f,0.0f),
        new Vector3(4.6f,1.3f,0.0f),
        //翼の指中央1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(3.9f,5.7f,0.0f),
        //翼の指中央2
        new Vector3(3.9f,5.7f,0.0f),
        new Vector3(7.7f,4.4f,0.0f),
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
        new Vector3(1.0f, 0.0f, 0.0f),  //翼の指内2の先
        //翼膜2
        new Vector3(1.0f, 0.0f, 0.0f),  //翼の指内2の先
        new Vector3(1.0f, 0.0f, 0.0f),  //翼の指中央2の先
        //翼膜3
        new Vector3(1.0f, 0.0f, 0.0f),  //翼の指中央2の先
        new Vector3(1.0f, 0.0f, 0.0f),  //翼の指外2の先
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
    Vector3[][] wingVertex = new Vector3[11][];     //最初に生成[11][5]
    Vector3[] wing_membraneVertex = new Vector3[63];//Meshに使用する配列

    //翼膜のメッシュ
    Mesh wing_membraneMesh;
    //メッシュフィルター
    MeshFilter meshFilter;

    //翼膜メッシュの頂点リスト
    int[] wing_triangles = new int[198];
    int[] wing_triangles1 = { 0,10, 1,   1,10, 2,   2,10,11,   2,11, 3,   3,11,12,   3,12, 4,   4,12,13,
                             36, 7,37,  37, 7, 6,  37, 6,38,  38, 6, 5,  38, 5,39,  39, 5, 4,  39, 4,40,
                             17,43,16,  16,43,15,  15,43,42,  15,42,14,  14,42,41,  14,41,13,  13,41,40,
                              4,13,40};
    int[] wing_triangles2 = { 0,19,10,  10,19,20,  10,20,11,  11,20,21,  11,20,12,  12,20,22,  12,22,13,
                             17,16,46,  46,16,15,  46,15,47,  47,15,14,  47,14,48,  48,14,13,  48,13,49,
                             26,52,25,  25,52,51,  25,51,24,  24,51,50,  24,50,23,  23,50,49,  23,49,22,
                             13,22,49};
    int[] wing_triangles3 = {18,28,19,  19,28,20,  20,28,29,  20,29,21,  21,29,30,  21,30,31,  21,31,22,
                             26,25,55,  55,25,24,  55,24,56,  56,24,23,  56,23,57,  57,23,22,  57,22,58,
                             35,61,34,  34,61,60,  33,61,60,  33,60,32,  32,60,59,  32,59,31,  31,59,58,
                             22,31,58};

    //翼膜の頂点を作成
    /* velocityを計算して曲線を作りboneArrayに渡す */
    void makewingVertex(Vector3[][] wingVertexArray)
    {
        for (int j = 0; j < 22; j = j + 2)//始点と終点のvelocityの計算をパーツの個数分行う
        {
            //ドラゴンボーン作成
            wing_velocity[j].x = wing_tension[j] * Mathf.Cos(wing_direction[j]);
            wing_velocity[j].y = wing_tension[j] * Mathf.Sin(wing_direction[j]);
            wing_velocity[j + 1].x = wing_tension[j + 1] * Mathf.Cos(wing_direction[j + 1]);
            wing_velocity[j + 1].y = wing_tension[j + 1] * Mathf.Sin(wing_direction[j + 1]);
            //翼の腕・指の場合
            if (j < 16)
            {
                Vector3[] p = new Vector3[step1_2_3_4];
                for (int i = 0; i < step1_2_3_4; i++)//stepの回数分曲線の点を作る
                {
                    float t = i / (float)(step1_2_3_4 - 1.0f);
                    p[i] = curvescript.curve(wing_position[j], wing_velocity[j], wing_position[j + 1], wing_velocity[j + 1], t);
                }
                //曲線を渡す
                wingVertexArray[j / 2] = p;
            }
            else//翼膜の場合
            {
                Vector3[] p = new Vector3[stepwing5_6_7];
                for (int i = 0; i < stepwing5_6_7; i++)//stepの回数分曲線の点を作る
                {
                    float t = i / (float)(stepwing5_6_7 - 1.0f);
                    p[i] = curvescript.curve(wing_position[j], wing_velocity[j], wing_position[j + 1], wing_velocity[j + 1], t);
                }
                //曲線を渡す
                wingVertexArray[j / 2] = p;
            }            
        }
    }    

    //翼膜mesh頂点を作成
    void makeWingMeshVertex(Vector3[][] wingVertexArray, Vector3[] wing_membraneVertexArray)
    {
        //wing_membraneVertexArrayの配列番号
        int num = 0;
        //対象は翼膜1～3以外をmesh用配列に渡す
        for (int i = 0; i < 4; i++)
        {
            //wingVertexArrayの j と j+1 の配列を1つにする
            //配列 j+1 の初めは j の最後と同じなため統合する
            int j = 2 * i;
            Vector3[] wing = new Vector3[wingVertexArray[j].Length + wingVertexArray[j + 1].Length - 1];
            Array.Copy(wingVertexArray[j], wing, wingVertexArray[j].Length);
            Array.Copy(wingVertexArray[j + 1], 1, wing, wingVertexArray[j].Length, wingVertexArray[j + 1].Length - 1);
            //Mesh用配列に渡す
            if (i == 0)//最初は逆順に渡す
            {
               for(int k = 0; k < 9; k++)
               {
                    int reverse = 8 - k;
                    wing_membraneVertexArray[num] = wing[reverse];
                    //Debug.Log("num=" + reverse);
                    num += 1;
               }
            }
            else//残りはそのままの順で配列に渡す
            {
                for (int k = 0; k < 9; k++)
                {
                    wing_membraneVertexArray[num] = wing[k];
                    //Debug.Log("num=" + num);
                    num += 1;
                }
            }
        }
        //翼膜1～3をMesh用に渡す
        for (int i = 0; i < 3; i++)
        {
            //wingVertexArray[8]～[10]が対象
            int j = i + 8;
            //wing_membraneVertexArray[36]から翼膜1～3を入れていく
            for (int k = 0; k < 9; k++)
            {
                wing_membraneVertexArray[num] = wingVertexArray[j][k];
                //Debug.Log("num=" + num);
                num += 1;
            }
        }
    }

    //翼膜wing_trianglesを作成
    void makeWing_triangles()
    {
        int index = 0;
        for(int i = 0; i < 66; i++)
        {
            wing_triangles[index] = wing_triangles1[i];
            index += 1;
        }
        for (int i = 0; i < 66; i++)
        {
            wing_triangles[index] = wing_triangles2[i];
            index += 1;
        }
        for (int i = 0; i < 66; i++)
        {
            wing_triangles[index] = wing_triangles3[i];
            index += 1;
        }
    }

    //翼のメッシュを作成
    void createMesh(Vector3[] vertexList, int[] meshTriangles)
    {
        //メッシュを作る
        wing_membraneMesh = new Mesh();

        //メッシュに頂点リストを登録
        wing_membraneMesh.SetVertices(vertexList);

        //メッシュに面を構成するインデックスリストを登録
        wing_membraneMesh.SetTriangles(meshTriangles, 0);

        //作成したメッシュをメッシュフィルターに設定
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = wing_membraneMesh;
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

        //ドラゴンボーンの作成
        makewingVertex(wingVertex);
        //mesh用の配列に変換
        makeWingMeshVertex(wingVertex, wing_membraneVertex);
        //meshのtrianglesを作成
        makeWing_triangles();

        //メッシュの作成
        createMesh(wing_membraneVertex, wing_triangles);
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
