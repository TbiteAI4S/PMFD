﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * ドラゴンボーンの基本となる数値を保存する
 * リセットする際はこの数値を使う
 */
public class DragonBorneData : MonoBehaviour
{
    /* ベースのドラゴンボーン */
    //座標
    public Vector3[] baseposition =
    {
        //頭(上あご) head
        new Vector3(-6.31f, 2.29f, 0.0f),
        new Vector3(-4.58f, 4.16f, 0.0f),
        //下顎 jaw
        new Vector3(-5.47f, 1.69f, 0.0f),
        new Vector3(-4.66f, 3.23f, 0.0f),
        //首 neck
        new Vector3(-4.35f, -1.2f, 0.0f),
        new Vector3(-2.25f, -1.09f, 0.0f),
        //胴体 body
        new Vector3(-2.25f, -1.09f, 0.0f),
        new Vector3(5.06f, -0.48f, 0.0f),
        //尾 tail
        new Vector3(5.06f, -0.48f, 0.0f),
        new Vector3(12.84f, -0.87f, 0.0f),

        /* 左側 */
        //左上腕 left_upper_arm
        new Vector3(-1.15f, -1.83f, 0.0f),
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
    public float[] basetension =
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
    public float[] basedirection =
    {   /* 始点, 終点 */

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

    /* ドラゴンボーンの最小値 */
    public Vector3[] minposition =
    {
        //頭(上あご) head
        new Vector3(-6.31f, 2.29f, 0.0f),
        new Vector3(-4.58f, 4.16f, 0.0f),
        //下顎 jaw
        new Vector3(-5.47f, 1.69f, 0.0f),
        new Vector3(-4.66f, 3.23f, 0.0f),
        //首 neck
        new Vector3(-4.35f, -1.0f, 0.0f),
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
        new Vector3(-1.14f, -2.73f, 0.0f),
        //左前腕 left_forearm
        new Vector3(-1.14f, -2.73f, 0.0f),
        new Vector3(-2.79f, -5.23f, 0.0f),
        //左大腿 left_thigh
        new Vector3(4.28f, -1.58f, 0.0f),
        new Vector3(4.51f, -2.22f, 0.0f),
        //左下腿 left_lower_leg
        new Vector3(4.51f, -3.22f, 0.0f),
        new Vector3(4.28f, -4.24f, 0.0f),
        //左足 left_foot
        new Vector3(4.28f, -4.74f, 0.0f),
        new Vector3(2.85f, -5.42f, 0.0f)
    };
    //張り
    public float[] mintension =
    {   /* 始点, 終点 */
        
        //頭(上あご) head
        0.5f,0.5f,
        //下顎 jaw
        0.1f,1.0f,
        //首 neck
        1.0f,1.0f,
        //胴体 body
        2.0f,0.4f,
        //尾 tail
        1.0f,1.0f,

        /* 左側 */
        //上腕 left_upper_arm
        1.2f,0.0f,
        //前腕 left_forearm
        1.0f,1.0f,
        //大腿 left_thigh
        2.8f,0.6f,
        //下腿 left_lower_leg
        0.2f,0.2f,
        //足 left_foot
        1.0f,1.0f,
    };
    //方向
    public float[] mindirection =
    {   /* 始点, 終点 */

        //頭(上あご) head
        0.0f,0.0f,
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
        1.0f,1.0f,
        //前腕 left_forearm
        0.0f,0.0f,
        //大腿 left_thigh
        -0.4f,1.0f,
        //下腿 left_lower_leg
        0.0f,-1.0f,
        //足 left_foot
        0.0f,0.0f,

    };

    /* ドラゴンボーンの最大値 */
    public Vector3[] maxposition =
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
        new Vector3(-1.14f, -4.73f, 0.0f),
        //左前腕 left_forearm
        new Vector3(-1.14f, -4.73f, 0.0f),
        new Vector3(-2.79f, -5.23f, 0.0f),
        //左大腿 left_thigh
        new Vector3(4.28f, -1.58f, 0.0f),
        new Vector3(4.51f, -4.22f, 0.0f),
        //左下腿 left_lower_leg
        new Vector3(4.51f, -3.22f, 0.0f),
        new Vector3(4.28f, -4.74f, 0.0f),
        //左足 left_foot
        new Vector3(4.28f, -4.74f, 0.0f),
        new Vector3(2.85f, -5.42f, 0.0f)
    };
    //張り
    public float[] maxtension =
    {   /* 始点, 終点 */
        
        //頭(上あご) head
        0.0f,5.5f,
        //下顎 jaw
        0.4f,-2.7f,
        //首 neck
        10.0f,4.8f,
        //胴体 body
        10.0f,10.0f,
        //尾 tail
        1.0f,1.0f,

        /* 左側 */
        //上腕 left_upper_arm
        1.6f,-3.0f,
        //前腕 left_forearm
        1.0f,1.0f,
        //大腿 left_thigh
        5.3f,0.6f,
        //下腿 left_lower_leg
        1.0f,-0.8f,
        //足 left_foot
        1.0f,1.0f,
    };
    //方向
    public float[] maxdirection =
    {   /* 始点, 終点 */

        //頭(上あご) head
        0.0f,0.0f,
        //下顎 jaw
        0.0f,-0.4f,
        //首 neck
        0.0f,-0.3f,
        //胴体 body
        0.7f,-0.5f,
        //尾 tail
        0.0f,0.0f,

        /* 左側 */
        //上腕 left_upper_arm
        0.0f,1.0f,
        //前腕 left_forearm
        0.0f,0.0f,
        //大腿 left_thigh
        -0.2f,1.0f,
        //下腿 left_lower_leg
        0.0f,-1.0f,
        //足 left_foot
        0.0f,0.0f,
    };
}