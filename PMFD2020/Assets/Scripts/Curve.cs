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
}
