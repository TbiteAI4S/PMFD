using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class MakeDragonBone : MonoBehaviour
{
    Curve curvescript;
    //ドラゴンボーンのデータ
    DragonBorneData dbd;

    /* パラメータ */
    HeadParameter headParameter;
    BodyParameter bodyParameter;


    /*----ドラゴンボーンのデータ----*/
    //座標
    Vector3[] position =
    {
        //頭(上あご) head
        new Vector3(-6.3f, -2.5f, 0.0f),
        new Vector3(-4.6f, -0.7f, 0.0f),
        //下顎 jaw
        new Vector3(-5.5f, -3.1f, 0.0f),
        new Vector3(-4.7f, -1.5f, 0.0f),
        //首 neck
        new Vector3(-4.4f, -1.2f, 0.0f),
        new Vector3(-2.3f, -1.1f, 0.0f),
        //胴体 body
        new Vector3(-2.3f, -1.1f, 0.0f),
        new Vector3(5.1f, -0.5f, 0.0f),
        //尾 tail
        new Vector3(5.1f, -0.5f, 0.0f),
        new Vector3(12.8f, -0.9f, 0.0f),

        /* 左側 */
        //左上腕 left_upper_arm
        new Vector3(-1.5f, -1.8f, 0.0f),
        new Vector3(-1.1f, -4.7f, 0.0f),
        //左前腕 left_forearm
        new Vector3(-1.1f, -4.7f, 0.0f),
        new Vector3(-2.8f, -5.2f, 0.0f),
        //左大腿 left_thigh
        new Vector3(4.3f, -1.6f, 0.0f),
        new Vector3(4.5f, -4.2f, 0.0f),
        //左下腿 left_lower_leg
        new Vector3(4.5f, -3.2f, 0.0f),
        new Vector3(4.3f, -4.7f, 0.0f),
        //左足 left_foot
        new Vector3(4.3f, -4.7f, 0.0f),
        new Vector3(2.9f, -5.4f, 0.0f)
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
    {   /* 始点, 終点 */
        
        //頭(上あご) head
        -0.4f,1.0f,
        //下顎 jaw
        1.6f,1.0f,
        //首 neck
        1.0f,1.0f,
        //胴体 body
        3.6f,-2.6f,
        //尾 tail
        1.0f,1.0f,

        /* 左側 */
        //上腕 left_upper_arm
        3.2f,-0.2f,
        //前腕 left_forearm
        1.0f,1.0f,
        //大腿 left_thigh
        3.7f,0.6f,
        //下腿 left_lower_leg
        1.0f,-0.6f,
        //足 left_foot
        1.0f,1.0f,
    };

    //方向
    float[] direction =
    {
        /* 始点, 終点 */

        //頭(上あご) head
        0.0f,-1.0f,
        //下顎 jaw
        0.0f,0.0f,
        //首 neck
        0.0f,0.0f,
        //胴体 body
        0.8f,0.4f,
        //尾 tail
        0.0f,0.0f,

        /* 左側 */
        //上腕 left_upper_arm
        0.0f,0.0f,
        //前腕 left_forearm
        0.4f,0.6f,
        //大腿 left_thigh
        0.1f,1.0f,
        //下腿 left_lower_leg
        0.0f,-1.0f,
        //足 left_foot
        0.0f,0.0f,
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

    // Start is called before the first frame update
    void Start()
    {
        //curveのスクリプト取得
        curvescript = GameObject.Find("Curve").GetComponent<Curve>();

        //ドラゴンボーンのデータのスクリプトを取得
        dbd = GameObject.Find("Doragon").GetComponent<DragonBorneData>();

        //パラメータスクリプトの取得
        headParameter = GameObject.Find("headPanel").GetComponent<HeadParameter>();
        bodyParameter = GameObject.Find("bodyPanel").GetComponent<BodyParameter>();

        //配列の初期化
        dragonbone = new Vector3[10][];

        //各パーツでドラゴンボーン作成
        makebone(dragonbone);

        //シェーダーに渡す
        //BoneDataToShader();
    }

    // Update is called once per frame
    void Update()
    {
        //HeadParameterの更新
        for (int i = 0; i < 4; i++)
        {
            position[i] = headParameter.head_position[i];
            tension[i] = headParameter.head_tension[i];
            direction[i] = headParameter.head_direction[i];
        }
        //BodyParameterの更新
        
        for (int i = 0; i < 6; i++)
        {
            int k = i + 4;
            position[k] = bodyParameter.body_position[i];
            tension[k] = bodyParameter.body_tension[i];
            direction[k] = bodyParameter.body_direction[i];
        }

        //ドラゴンボーンを作る
        makebone(dragonbone);
        
    }
}
