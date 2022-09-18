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
        float p = (((dx * 2.0f + m0 + m1) * t - dx * 3.0f - m0 * 2.0f - m1) * t + m0) * t + p0;
        
        return p;
    }

    //3次エルミートスプラインによる点列の補間
    //引数：p0(始点), m0(始点の接ベクトル), p1(終点), m1(終点の接ベクトル), t(ステップ数)
    //返り値：p(補完結果の座標)
    public Vector3 curve(Vector3 p0, Vector3 m0, Vector3 p1, Vector3 m1, float t)
    {
        float x = cubic_hermite(p0.x, m0.x, p1.x, m1.x, t);
        float y = cubic_hermite(p0.y, m0.y, p1.y, m1.y, t);
        float z = cubic_hermite(p0.z, m0.z, p1.z, m1.z, t);
        //Vector3型にする
        Vector3 p = new Vector3(x, y, z);

        return p;
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

    // 曲線の節点数
    int step = 20;

    //曲線の座標を入れる
    public Vector3[] curvePosition;

    //スライダーからのデータ管理スクリプト
    CurveManage cm;


    // Start is called before the first frame update
    void Start()
    {
        //CueveManagerを取得
        cm = GameObject.Find("SliderManager").GetComponent<CurveManage>();

        //曲線を作る
        velocity[0].x = tension[0] * Mathf.Cos(direction[0]);
        velocity[0].y = tension[0] * Mathf.Sin(direction[0]);
        velocity[1].x = tension[1] * Mathf.Cos(direction[1]);
        velocity[1].y = tension[1] * Mathf.Sin(direction[1]);

        

        Vector3[] p = new Vector3[step];
        for(int i= 0; i < step; i++)
        {
            float t = i / (float)(step - 1.0f);
            p[i] = curve(position[0], velocity[0], position[1], velocity[1], t);
            //Debug.Log("p[" + i + "]" + p[i]);
        }

        //曲線を渡す
        curvePosition = p;
        /*
        int a = 1;
        foreach (var point in p)
        {
            Debug.Log("p" + a + " " + point);
            a += 1;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        //始点の位置・張り・向きを取得
        position[0] = cm.nowPostion[0];
        tension[0] = cm.nowtension[0];
        direction[0] = cm.nowdirection[0];

        //終点の位置・張り・向きを取得
        position[1] = cm.nowPostion[1];
        tension[1] = cm.nowtension[1];
        direction[1] = cm.nowdirection[1];

        //曲線を作る
        velocity[0].x = tension[0] * Mathf.Cos(direction[0]);
        velocity[0].y = tension[0] * Mathf.Sin(direction[0]);
        velocity[1].x = tension[1] * Mathf.Cos(direction[1]);
        velocity[1].y = tension[1] * Mathf.Sin(direction[1]);

        Vector3[] p = new Vector3[step];
        for (int i = 0; i < step; i++)
        {
            float t = i / (float)(step - 1);
            p[i] = curve(position[0], velocity[0], position[1], velocity[1], t);
        }

        //曲線を渡す
        curvePosition = p;
        
    }

    //Gizmoで頂点確認
    void OnDrawGizmos()
    {
        /*
        Gizmos.color = Color.red;
        foreach (var point in curvePosition)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
        */
        
    }
}
