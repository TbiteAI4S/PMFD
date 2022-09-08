using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{

    //3次エルミートスプライン
    //引数：p0(始点), m0(始点の接ベクトル), p1(終点), m1(終点の接ベクトル), t(ステップ数)
    //返り値：計算結果
    public float cubic_hermite(float p0, float m0, float p1, float m1, float t)
    {
        float dx = p0 - p1;
        return (((dx * 2.0f + m0 + m1) * t - dx * 3.0f - m0 * 2.0f - m1) * t + m0) * t + p0;
    }

    //3次エルミートスプラインによる点列の補間
    //引数：p(補完結果の座標), p0(始点), m0(始点の接ベクトル), p1(終点), m1(終点の接ベクトル), t(ステップ数)
    //返り値：
    public void curve(Vector3 p, Vector3 p0, Vector3 m0, Vector3 p1, Vector3 m1, float t)
    {
        p[0] = cubic_hermite(p0[0], m0[0], p1[0], m1[0], t);
        p[1] = cubic_hermite(p0[1], m0[1], p1[1], m1[1], t);
        p[2] = cubic_hermite(p0[2], m0[2], p1[2], m1[2], t);
    }

    /*----初期値---------------------------------*/

    // 曲線の端点の位置
    Vector3[] position = {
        new Vector3(-1.0f, -1.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 0.0f)
    };

    // 曲線の端点の速度
    Vector3[] velocity ={
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f)
    };

    // 曲線の端点の張り
    float[] tension = { 1.0f, 1.0f };
    float[] direction = { 0.0f, 0.0f };

    /*------------------------------------------*/

    //曲線の端点の位置
    Vector3[] nowPostion;

    //曲線の端点の速度
    Vector3[] nowVelocity;

    //曲線の端点の張り
    float[] nowtension;
    float[] nowdirection;


    // 曲線の節点数
    int step = 20;

    //曲線の座標を入れる
    public Vector3[] curvePosition;


// Start is called before the first frame update
    void Start()
    {
        //曲線を作る
        velocity[0][0] = tension[0] * Mathf.Cos(direction[0]);
        velocity[0][1] = tension[0] * Mathf.Sin(direction[0]);
        velocity[1][0] = tension[1] * Mathf.Cos(direction[1]);
        velocity[1][1] = tension[1] * Mathf.Sin(direction[1]);

        Vector3[] p = new Vector3[step];
        for(int i= 0; i < step; i++)
        {
            float t = (float)i / (float)(step - 1);
            curve(p[i], position[0], velocity[0], position[1], velocity[1], t);
        }

        //曲線を渡す
        curvePosition = p;
    }

    // Update is called once per frame
    void Update()
    {
        //始点の位置・張り・向きを取得
        //終点の位置・張り・向きを取得
        //曲線を作る
        velocity[0][0] = tension[0] * Mathf.Cos(direction[0]);
        velocity[0][1] = tension[0] * Mathf.Sin(direction[0]);
        velocity[1][0] = tension[1] * Mathf.Cos(direction[1]);
        velocity[1][1] = tension[1] * Mathf.Sin(direction[1]);

        Vector3[] p = new Vector3[step];
        for (int i = 0; i < step; i++)
        {
            float t = (float)i / (float)(step - 1);
            curve(p[i], position[0], velocity[0], position[1], velocity[1], t);
        }

        //曲線を渡す
        curvePosition = p;
    }
}
