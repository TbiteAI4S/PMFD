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
        new Vector3(-2.34f, 1.95f, 0.0f),
        new Vector3(-1.14f, 2.62f, 0.0f),
        //下顎 jaw
        new Vector3(-1.77f, 1.54f, 0.0f),
        new Vector3(-1.14f, 2.62f, 0.0f),
        //首 neck
        new Vector3(-1.14f, 2.62f, 0.0f),
        new Vector3(-0.13f, 0.09f, 0.0f),
        //胴体 body
        new Vector3(-0.13f, 0.09f, 0.0f),
        new Vector3(3.81f, 0.5f, 0.0f),
        //尾 tail
        new Vector3(3.81f, 0.5f, 0.0f),
        new Vector3(5.0f, -2.5f, 0.0f),
        //上腕 upper_arm
        /*-left-*/
        /*-right-*/
        //前腕 forearm
        /*-left-*/
        /*-right-*/
        //大腿 thigh
        /*-left-*/
        /*-right-*/
        //下腿 lower_leg
        /*-left-*/
        /*-right-*/
        //足 foot
        /*-left-*/
        /*-right-*/
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
        //上腕 upper_arm
        /*-left-*/
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        /*-right-*/
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //前腕 forearm
        /*-left-*/
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        /*-right-*/
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //大腿 thigh
        /*-left-*/
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        /*-right-*/
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //下腿 lower_leg
        /*-left-*/
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        /*-right-*/
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //足 foot
        /*-left-*/
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        /*-right-*/
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
        //上腕 upper_arm
        /*-left-*/
        1.0f,1.0f,
        /*-right-*/
        1.0f,1.0f,
        //前腕 forearm
        /*-left-*/
        1.0f,1.0f,
        /*-right-*/
        1.0f,1.0f,
        //大腿 thigh
        /*-left-*/
        1.0f,1.0f,
        /*-right-*/
        1.0f,1.0f,
        //下腿 lower_leg
        /*-left-*/
        1.0f,1.0f,
        /*-right-*/
        1.0f,1.0f,
        //足 foot
        /*-left-*/
        1.0f,1.0f,
        /*-right-*/
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
        //上腕 upper_arm
        /*-left-*/
        0.0f,0.0f,
        /*-right-*/
        0.0f,0.0f,
        //前腕 forearm
        /*-left-*/
        0.0f,0.0f,
        /*-right-*/
        0.0f,0.0f,
        //大腿 thigh
        /*-left-*/
        0.0f,0.0f,
        /*-right-*/
        0.0f,0.0f,
        //下腿 lower_leg
        /*-left-*/
        0.0f,0.0f,
        /*-right-*/
        0.0f,0.0f,
        //足 foot
        /*-left-*/
        0.0f,0.0f,
        /*-right-*/
        0.0f,0.0f
    };

    //ドラゴンボーンの分割数
    int step = 10;

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
    void makebone(Vector3[][] boneArray)
    {
        for (int j = 0; j < 10; j = j + 2)
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

    //シェーダーに座標を渡す
    void BoneDataToShader()
    {
        //マテリアルを取得
        mat = _renderer.material;

        //ボーンの座標をシェーダーに渡す形に変換
        Vector3[] throwpoints = new Vector3[25];
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


    // Start is called before the first frame update
    void Start()
    {
        //CueveManagerを取得
        cm = GameObject.Find("SliderManager").GetComponent<CurveManage>();

        //curveのスクリプト取得
        curvescript = GameObject.Find("Curve").GetComponent<Curve>();

        //配列の初期化
        dragonbone = new Vector3[5][];

        //各パーツでドラゴンボーン作成
        makebone(dragonbone);

        //シェーダーに渡す
        BoneDataToShader();

        ///////////////////////////////////////////////////////////////////
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        int a = 4;
        // 点の数を指定する
        lineRenderer.positionCount = dragonbone[a].Length;
        // 線を引く場所を指定する
        lineRenderer.SetPositions(dragonbone[a]);
        /////////////////////////////////////////////////////////////////
    }

    // Update is called once per frame
    void Update()
    {
        //スライダーからのデータの更新
        tension[8] = cm.nowtension[0];
        direction[8] = cm.nowdirection[0];
        tension[9] = cm.nowtension[1];
        direction[9] = cm.nowdirection[1];

        //各パーツでドラゴンボーン作成
        makebone(dragonbone);


        ///////////////////////////////////////////////////////////////////
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        int a = 4;
        // 点の数を指定する
        lineRenderer.positionCount = dragonbone[a].Length;
        // 線を引く場所を指定する
        lineRenderer.SetPositions(dragonbone[a]);
        ///////////////////////////////////////////////////////////////////
        
    }
}
