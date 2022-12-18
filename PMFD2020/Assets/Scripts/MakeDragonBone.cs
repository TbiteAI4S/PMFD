using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class MakeDragonBone : MonoBehaviour
{
    Curve curvescript;

    /* パラメータ */
    HeadParameter headParameter;
    BodyParameter bodyParameter;
    ArmParameter armParameter;
    FootParameter footParameter;
    WingParameter wingParameter;


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


        //上腕 upper_arm
        new Vector3(-1.5f, -1.8f, 0.0f),
        new Vector3(-1.1f, -4.7f, -0.7f),
        //前腕 forearm
        new Vector3(-1.1f, -4.7f, -0.7f),
        new Vector3(-2.8f, -5.2f, -0.7f),
        //大腿 thigh
        new Vector3(4.3f, -1.6f, 0.0f),
        new Vector3(4.5f, -4.2f, -0.7f),
        //下腿 lower_leg
        new Vector3(4.5f, -3.2f, -0.7f),
        new Vector3(4.3f, -4.7f, -0.7f),
        //足 foot
        new Vector3(4.3f, -4.7f, -0.7f),
        new Vector3(2.9f, -5.4f, -0.7f)
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
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //前腕 forearm
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //大腿 thigh
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //下腿 lower_leg
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //足 foot
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),

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

        //上腕 upper_arm
        3.2f,-0.2f,
        //前腕 forearm
        1.0f,1.0f,
        //大腿 thigh
        3.7f,0.6f,
        //下腿 lower_leg
        1.0f,-0.6f,
        //足 foot
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

        //上腕 upper_arm
        0.0f,0.0f,
        //前腕 forearm
        0.4f,0.6f,
        //大腿 thigh
        0.1f,1.0f,
        //下腿 lower_leg
        0.0f,-1.0f,
        //足 foot
        0.0f,0.0f,
    };

    Vector3[] wing_position =
    {
        //翼の下腕
        new Vector3(-1.2f,-0.2f,0.0f),
        new Vector3(0.0f,3.0f,-0.7f),
        //翼の上腕
        new Vector3(0.0f,3.0f,-0.7f),
        new Vector3(1.2f,6.0f,-1.2f),
        //翼の指内1
        new Vector3(1.2f,6.0f,-1.2f),
        new Vector3(2.2f,3.7f,-1.6f),
        //翼の指内2
        new Vector3(2.2f,3.7f,-1.6f),
        new Vector3(4.6f,1.3f,-2.1f),
        //翼の指中央1
        new Vector3(1.2f,6.0f,-1.2f),
        new Vector3(3.9f,5.7f,-1.6f),
        //翼の指中央2
        new Vector3(3.9f,5.7f,-1.6f),
        new Vector3(7.7f,4.4f,-2.1f),
        //翼の指外1
        new Vector3(1.2f,6.0f,-1.2f),
        new Vector3(5.2f,8.0f,-1.6f),
        //翼の指外2
        new Vector3(5.2f,8.0f,-1.6f),
        new Vector3(9.4f,7.5f,-2.1f),

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
        //翼の指内1
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指内2
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指中央1
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指中央2
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指外1
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        //翼の指外2
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
    };

    //張り
    float[] wing_tension =
    {   /* 始点, 終点 */
        //翼の上腕
        1.0f,1.0f,
        1.0f,1.0f,
        //翼の指内
        1.0f,1.0f,
        1.0f,1.0f,
        //翼の指中央
        1.0f,1.0f,
        1.0f,1.0f,
        //翼の指外
        1.0f,1.0f,
        1.0f,1.0f,
    };

    //方向
    float[] wing_direction =
    {   /* 始点, 終点 */
        //翼の上腕
        0.0f,0.0f,
        0.0f,0.0f,
        //翼の指内
        0.0f,0.0f,
        0.0f,0.0f,
        //翼の指中央
        0.0f,0.0f,
        0.0f,0.0f,
        //翼の指外
        0.0f,0.0f,
        0.0f,0.0f,
    };

    //ドラゴンボーンの分割数
    const int step = 10;

    //ドラゴンボーン
    public Vector3[][] dragonbone;
    public Vector3[][] right_arm_leg_dragonbone;
    public Vector3[][] wing_dragonbone;
    public Vector3[][] right_wign_dragonborne;

    //シェーダーに渡す用の配列
    public Vector3[] _Dragonborne = new Vector3[step];
    public Vector3[] _Jawborne = new Vector3[step];
    public Vector3[] _Neckborne = new Vector3[step];
    public Vector3[] _Bodyborne = new Vector3[step];
    public Vector3[] _Tailborne = new Vector3[step];

    public Vector3[] _ArmLeftborne = new Vector3[2 * step];
    public Vector3[] _ArmRightborne = new Vector3[2* step];
    public Vector3[] _FootLeftborne = new Vector3[2 * step];
    public Vector3[] _FootRightborne = new Vector3[2 * step];

    //マテリアル
    private Material mat;

    //レンダラー
    [SerializeField] private Renderer _renderer;

    ////////////////////////////////////////////////////////////////////////////////////////////

    /*-----関数-----*/

    //ドラゴンボーンの作成
    /*
     * velocityを計算して曲線を作りboneArrayに渡す
     */
    public void makebone(Vector3[][] boneArray)
    {
        for (int j = 0; j < 20; j = j + 2)//始点と終点のvelocityの計算をパーツの個数分行う
        {
            //ドラゴンボーン作成
            velocity[j].x = tension[j] * Mathf.Cos(direction[j]);
            velocity[j].y = tension[j] * Mathf.Sin(direction[j]);
            velocity[j + 1].x = tension[j + 1] * Mathf.Cos(direction[j + 1]);
            velocity[j + 1].y = tension[j + 1] * Mathf.Sin(direction[j + 1]);

            Vector3[] p = new Vector3[step];
            for (int i = 0; i < step; i++)//stepの回数分曲線の点を作る
            {
                float t = i / (float)(step - 1.0f);
                p[i] = curvescript.curve(position[j], velocity[j], position[j + 1], velocity[j + 1], t);
            }
            //曲線を渡す
            boneArray[j / 2] = p;
        }
    }
    //翼のドラゴンボーン作成
    public void makewingbone(Vector3[][] wingboneArray)
    {
        for (int j = 0; j < 16; j = j + 2)//始点と終点のvelocityの計算をパーツの個数分行う
        {
            //ドラゴンボーン作成
            wing_velocity[j].x = wing_tension[j] * Mathf.Cos(wing_direction[j]);
            wing_velocity[j].y = wing_tension[j] * Mathf.Sin(wing_direction[j]);
            wing_velocity[j + 1].x = wing_tension[j + 1] * Mathf.Cos(wing_direction[j + 1]);
            wing_velocity[j + 1].y = wing_tension[j + 1] * Mathf.Sin(wing_direction[j + 1]);

            Vector3[] p = new Vector3[step];
            for (int i = 0; i < step; i++)//stepの回数分曲線の点を作る
            {
                float t = i / (float)(step - 1.0f);
                p[i] = curvescript.curve(wing_position[j], wing_velocity[j], wing_position[j + 1], wing_velocity[j + 1], t);
            }
            //曲線を渡す
            wingboneArray[j / 2] = p;
        }
    }

    //左右対称のパーツを作成(右側の腕，足，翼)
    void make_rightparts(Vector3[][] boneArray, Vector3[][] right_boneArray, Vector3[][] wingboneArray, Vector3[][] wingright_boneArray)
    {
        //腕と足
        for (int i = 0; i < 5; i++)
        {
            //腕の足はindex5から始まる
            int k = i + 5;
            Vector3[] inversion = new Vector3[boneArray[k].Length];
            //z座標を反転する
            for (int j = 0; j < boneArray[k].Length; j++)
            {
                inversion[j] = new Vector3(boneArray[k][j].x, boneArray[k][j].y, -1.0f * boneArray[k][j].z);
            }
            right_boneArray[i] = inversion;
        }
        //翼
        for (int i = 0; i < 8; i++)
        {
            Vector3[] wing_inversion = new Vector3[wingboneArray[i].Length];
            for (int j = 0; j < wingboneArray[i].Length; j++)
            {
                wing_inversion[j] = new Vector3(boneArray[i][j].x, boneArray[i][j].y, -1.0f * boneArray[i][j].z);
            }
            wingright_boneArray[i] = wing_inversion;
        }
    }

    //シェーダーに座標を渡す
    void BoneDataToShader()
    {
        for (int i = 0; i < 10; i++)
        {
            _Dragonborne[i] = (dragonbone[0][i] + new Vector3(10f, 10f, 0)) * 0.05f + new Vector3(0, 0, 0.5f);
            _Jawborne[i] = (dragonbone[1][i] + new Vector3(10f, 10f, 0)) * 0.05f + new Vector3(0, 0, 0.5f);
            _Neckborne[i] = (dragonbone[2][i] + new Vector3(10f, 10f, 0)) * 0.05f + new Vector3(0, 0, 0.5f);
            _Bodyborne[i] = (dragonbone[3][i] + new Vector3(10f, 10f, 0)) * 0.05f + new Vector3(0, 0, 0.5f);
            _Tailborne[i] = (dragonbone[4][i] + new Vector3(10f, 10f, 0)) * 0.05f + new Vector3(0, 0, 0.5f);
        }
        for (int i = 0; i < 10; i++)
        {
            _ArmLeftborne[i] = (dragonbone[5][i] + new Vector3(10f, 10f, 10f)) * 0.05f + new Vector3(0, 0, 0.5f);
            _ArmLeftborne[i+10] = (dragonbone[6][i] + new Vector3(10f, 10f, 10f)) * 0.05f + new Vector3(0, 0, 0.5f);

            _ArmRightborne[i] = (right_arm_leg_dragonbone[0][i] + new Vector3(10f, 10f, 10f)) * 0.05f + new Vector3(0, 0, 0.5f);
            _ArmRightborne[i+10] = (right_arm_leg_dragonbone[1][i] + new Vector3(10f, 10f, 10f)) * 0.05f + new Vector3(0, 0, 0.5f);

            _FootLeftborne[i] = (dragonbone[7][i] + new Vector3(10f, 10f, 10f)) * 0.05f + new Vector3(0, 0, 0.5f);
            _FootLeftborne[i+10] = (dragonbone[8][i] + new Vector3(10f, 10f, 10f)) * 0.05f + new Vector3(0, 0, 0.5f);

            _FootRightborne[i] = (right_arm_leg_dragonbone[2][i] + new Vector3(10f, 10f, 10f)) * 0.05f + new Vector3(0, 0, 0.5f);
            _FootRightborne[i+10] = (right_arm_leg_dragonbone[3][i] + new Vector3(10f, 10f, 10f)) * 0.05f + new Vector3(0, 0, 0.5f);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        //curveのスクリプト取得
        curvescript = GameObject.Find("Curve").GetComponent<Curve>();
        //パラメータスクリプトの取得
        headParameter = GameObject.Find("headPanel").GetComponent<HeadParameter>();
        bodyParameter = GameObject.Find("bodyPanel").GetComponent<BodyParameter>();
        armParameter = GameObject.Find("armPanel").GetComponent<ArmParameter>();
        footParameter = GameObject.Find("footPanel").GetComponent<FootParameter>();
        wingParameter = GameObject.Find("wingPanel").GetComponent<WingParameter>();

        //配列の初期化
        dragonbone = new Vector3[10][];
        right_arm_leg_dragonbone = new Vector3[5][];
        wing_dragonbone = new Vector3[8][];
        right_wign_dragonborne = new Vector3[8][];

        //各パーツでドラゴンボーン作成
        makebone(dragonbone);
        makewingbone(wing_dragonbone);
        //左右対称のパーツを作成
        make_rightparts(dragonbone, right_arm_leg_dragonbone, wing_dragonbone, right_wign_dragonborne);
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("_ArmLeftborne" + _ArmLeftborne[i] + ",_ArmRightborne" + _ArmRightborne[i]);
        }

        //テスト
        BoneDataToShader();    


    }

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
        //ArmParameterの更新
        for (int i = 0; i < 4; i++)
        {
            int k = i + 10;
            position[k] = armParameter.left_arm_position[i];
            tension[k] = armParameter.arm_tension[i];
            direction[k] = armParameter.arm_direction[i];
        }
        //FootParameterの更新
        for (int i = 0; i < 6; i++)
        {
            int k = i + 14;
            position[k] = footParameter.left_foot_position[i];
            tension[k] = footParameter.foot_tension[i];
            direction[k] = footParameter.foot_direction[i];
            //Debug.Log("position[" + k + "]" + position[k]+ ",tension[" + k + "]" + tension[k]+ ",direction[" + k + "]" + direction[k]);
        }
        //WingParameterの更新
        for (int i = 0; i < 16; i++)
        {
            wing_position[i] = wingParameter.left_wing_position[i];
            wing_tension[i] = wingParameter.wing_tension[i];
            wing_direction[i] = wingParameter.wing_direction[i];
        }

        //ドラゴンボーンを作る
        makebone(dragonbone);
        makewingbone(wing_dragonbone);
        //左右対称のパーツを作成
        make_rightparts(dragonbone, right_arm_leg_dragonbone, wing_dragonbone, right_wign_dragonborne);
        
    }
}
