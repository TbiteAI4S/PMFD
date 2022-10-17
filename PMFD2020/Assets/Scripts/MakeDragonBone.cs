using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class MakeDragonBone : MonoBehaviour
{
    CurveManage cm;
    Curve curvescript;
    /*----ドラゴンボーンのデータ----*/
    //座標
    Vector3[] position =
    {
        //頭(上あご) head
        new Vector3(-6.31f, 2.29f, 0.0f),
        new Vector3(-4.58f, 4.16f, 0.0f),
        //下顎 jaw
        new Vector3(-5.47f, 1.69f, 0.0f),
        new Vector3(-4.66f, 3.23f, 0.0f),
        //首 neck
        new Vector3(-4.35f, 3.55f, 0.0f),
        new Vector3(-2.25f, -1.09f, 0.0f),
        //胴体 body
        new Vector3(-2.25f, -1.09f, 0.0f),
        new Vector3(5.06f, -0.48f, 0.0f),
        //尾 tail
        new Vector3(5.06f, -0.48f, 0.0f),
        new Vector3(12.84f, -0.87f, 0.0f),

        /* 左側 */
        //左上腕 left_upper_arm
        new Vector3(-1.45f, -1.83f, 0.0f),
        new Vector3(-1.14f, -3.73f, 0.0f),
        //左前腕 left_forearm
        new Vector3(-1.14f, -3.73f, 0.0f),
        new Vector3(-2.79f, -5.23f, 0.0f),
        //左大腿 left_thigh
        new Vector3(4.28f, -1.58f, 0.0f),
        new Vector3(4.51f, -3.22f, 0.0f),
        //左下腿 left_lower_leg
        new Vector3(4.51f, -3.22f, 0.0f),
        new Vector3(4.28f, -4.74f, 0.0f),
        //左足 left_foot
        new Vector3(4.28f, -4.74f, 0.0f),
        new Vector3(2.85f, -5.42f, 0.0f)
    };

    //端点での速度
    Vector3[] velocity ={
        //頭(上あご) head
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //下顎 jaw
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //首 neck
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //胴体 body
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //尾 tail
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),

        /* 左側 */
        //上腕 left_upper_arm
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //前腕 left_forearm
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //大腿 left_thigh
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //下腿 left_lower_leg
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //足 left_foot
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),

        /* 右側 */
        //上腕 right_upper_arm
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //前腕 right_forearm
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //大腿 right_thigh
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //下腿 right__lower_leg
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //足 right_foot
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f)
    };

    //張り
    float[] tension =
    {
        //頭(上あご) head
        1.0f,1.0f,
        //下顎 jaw
        1.0f,1.0f,
        //首 neck
        1.0f,1.0f,
        //胴体 body
        1.0f,1.0f,
        //尾 tail
        1.0f,1.0f,

        /* 左側 */
        //上腕 left_upper_arm
        1.0f,1.0f,
        //前腕 left_forearm
        1.0f,1.0f,
        //大腿 left_thigh
        1.0f,1.0f,
        //下腿 left_lower_leg
        1.0f,1.0f,
        //足 left_foot
        1.0f,1.0f,

        /* 右側 */
        //上腕 right_upper_arm
        1.0f,1.0f,
        //前腕 right_forearm
        1.0f,1.0f,
        //大腿 right_thigh
        1.0f,1.0f,
        //下腿 right_lower_leg
        1.0f,1.0f,
        //足 right_foot
        1.0f,1.0f
    };

    //方向
    float[] direction =
    {
        //頭(上あご) head
        0.0f,0.0f,
        //下顎 jaw
        0.0f,0.0f,
        //首 neck
        0.0f,0.0f,
        //胴体 body
        0.0f,0.0f,
        //尾 tail
        0.0f,0.0f,

        /* 左側 */
        //上腕 left_upper_arm
        0.0f,0.0f,
        //前腕 left_forearm
        0.0f,0.0f,
        //大腿 left_thigh
        0.0f,0.0f,
        //下腿 left_lower_leg
        0.0f,0.0f,
        //足 left_foot
        0.0f,0.0f,

        /* 右側 */
        //上腕 right_upper_arm
        0.0f,0.0f,
        //前腕 right_forearm
        0.0f,0.0f,
        //大腿 right_thigh
        0.0f,0.0f,
        //下腿 right_lower_leg
        0.0f,0.0f,
        //足 right_foot
        0.0f,0.0f
    };

    //ドラゴンボーンの分割数
    const int step = 10;

    //ドラゴンボーン
    public Vector3[][] dragonbone;

    //マテリアル
    private Material mat;

    //レンダラー
    [SerializeField] private Renderer _renderer;

    ////////////////////////////////////////////////////////////////////////////////////////////


    LineRenderer lineRenderer;

    /*-----関数-----*/

    //ドラゴンボーンの作成
    /*
     * velocityを計算して曲線を作りboneArrayに渡す
     */
    public void makebone(Vector3[][] boneArray)
    {
        for (int j = 0; j < 20; j = j + 2)
        {
            //ドラゴンボーン作成
            velocity[j].x = tension[j] * Mathf.Cos(direction[j]);
            velocity[j].y = tension[j] * Mathf.Sin(direction[j]);
            velocity[j + 1].x = tension[j + 1] * Mathf.Cos(direction[j + 1]);
            velocity[j + 1].y = tension[j + 1] * Mathf.Sin(direction[j + 1]);

            Vector3[] p = new Vector3[step];
            for (int i = 0; i < step; i++)
            {
                float t = i / (float)(step - 1.0f);
                p[i] = curvescript.curve(position[j], velocity[j], position[j + 1], velocity[j + 1], t);
            }
            //曲線を渡す
            boneArray[j / 2] = p;
        }
    }

    //ドラゴンボーンの更新
    /*
     * 変化した端点と再計算したvelocityで曲線を作りboneArrayに渡す
     */
    public void updatemakebone(Vector3 update_start, Vector3 update_end, Vector3[][] boneArray)
    {
        for (int j = 0; j < 20; j = j + 2)
        {
            //ドラゴンボーン作成
            velocity[j].x = tension[j] * Mathf.Cos(direction[j]);
            velocity[j].y = tension[j] * Mathf.Sin(direction[j]);
            velocity[j + 1].x = tension[j + 1] * Mathf.Cos(direction[j + 1]);
            velocity[j + 1].y = tension[j + 1] * Mathf.Sin(direction[j + 1]);

            Vector3[] p = new Vector3[step];
            for (int i = 0; i < step; i++)
            {
                float t = i / (float)(step - 1.0f);
                p[i] = curvescript.curve(position[j]+update_start, velocity[j], position[j + 1]+update_end, velocity[j + 1], t);
            }
            //曲線を渡す
            boneArray[j / 2] = p;
        }
    }

    //シェーダーに座標を渡す
    void BoneDataToShader()
    {
        //マテリアルを取得
        mat = _renderer.material;

        //ボーンの座標をシェーダーに渡す形に変換
        Vector3[] throwpoints = new Vector3[50];
        int throwNum = 0;

        for (int i = 0; i < dragonbone.Length; i++)//パーツの数だけ
        {
            int b = dragonbone[i].Length / 2;
            for (int j = 0; j < b; j++)//各パーツの半分(偶数番号)のみ渡す
            {
                int k = j * 2;
                throwpoints[throwNum] = dragonbone[i][k];
                throwNum += 1;
            }
        }

        //マテリアルに座標を渡す
        for (int i = 0; i < throwpoints.Length; i++)
        {
            mat.SetVector("_MPositions[" + i + "]", throwpoints[i]);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////

    int aa = 8;
    int bb = 16;

    // Start is called before the first frame update
    void Start()
    {
        //CueveManagerを取得
        cm = GameObject.Find("SliderManager").GetComponent<CurveManage>();

        //curveのスクリプト取得
        curvescript = GameObject.Find("Curve").GetComponent<Curve>();

        //配列の初期化
        dragonbone = new Vector3[10][];

        //各パーツでドラゴンボーン作成
        makebone(dragonbone);

        //シェーダーに渡す
        //BoneDataToShader();

        ///////////////////////////////////////////////////////////////////
        
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        // 点の数を指定する
        lineRenderer.positionCount = dragonbone[aa].Length;
        // 線を引く場所を指定する
        lineRenderer.SetPositions(dragonbone[aa]);
        
        /////////////////////////////////////////////////////////////////
    }

    // Update is called once per frame
    void Update()
    {
        
        //スライダーからのデータの更新
        Vector3 start = cm.nowPostion[0];
        Vector3 end = cm.nowPostion[1];
        tension[bb] = cm.nowtension[0];
        direction[bb] = cm.nowdirection[0];
        tension[bb+1] = cm.nowtension[1];
        direction[bb+1] = cm.nowdirection[1];

        //各パーツでドラゴンボーン作成
        updatemakebone(start, end, dragonbone);


        ///////////////////////////////////////////////////////////////////
        
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        
        // 点の数を指定する
        lineRenderer.positionCount = dragonbone[aa].Length;
        // 線を引く場所を指定する
        lineRenderer.SetPositions(dragonbone[aa]);
        
        ///////////////////////////////////////////////////////////////////
        
    }
}
