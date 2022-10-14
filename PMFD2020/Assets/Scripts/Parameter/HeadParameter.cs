using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * ドラゴンボーンの基本となる数値を保存する
 * リセットする際はこの数値を使う
 */
public class HeadParameter : MonoBehaviour
{
    /* ベースのドラゴンボーン */
    //座標
    Vector3[] baseposition =
    {
        //頭(上あご) head
        new Vector3(-6.31f, 2.29f, 0.0f),
        new Vector3(-4.58f, 4.16f, 0.0f),
        //下顎 jaw
        new Vector3(-1.77f, 1.54f, 0.0f),
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
    //張り
    float[] basetension =
    {   /* 始点, 終点 */
        
        //頭(上あご) head
        0.8f,4.0f,
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
    float[] basedirection =
    {   /* 始点, 終点 */

        //頭(上あご) head
        0.0f,-1.0f,
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

    /* ドラゴンボーンの最小値 */
}
