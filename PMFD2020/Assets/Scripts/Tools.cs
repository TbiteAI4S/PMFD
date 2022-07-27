using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    //内積
    public float InnerProduct(Vector3 a, Vector3 b)
    {
        float innerVec = a.x * b.x + a.y * b.y + a.z * b.z;

        //内積結果
        return innerVec;
    }

    //外積
    public Vector3 CrossProduct(Vector3 a, Vector3 b)
    {
        Vector3 crossVec;

        crossVec.x = a.y * b.z - a.z * b.y;
        crossVec.y = a.z * b.x - a.x * b.z;
        crossVec.z = a.x * b.y - a.y * b.x;

        //外積結果
        return crossVec;
    }

    //行列の積
    public float[][] GetMatrices(float[][] A, float[][] B)
    {
        //計算した積を入れる配列
        float[][] matrix = new float[3][]; // 3行3列の行列
        for (int i = 0; i < matrix.Length; ++i)
        {
            matrix[i] = new float[3];
        }


        //行列の要素ごとの積
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    matrix[i][j] += A[i][k] * B[k][j];
                }
            }
        }

        return matrix;
    }

    //平行移動を求める
    //引数：移動前(beforeP)，移動後(afterP)
    public Vector3 GetTranslation(Vector3 beforeP, Vector3 afterP)
    {
        Vector3 translation = afterP - beforeP;

        //平行移動
        return translation;
    }

    //x軸での回転を求める
    //引数：回転前(beforeP)，回転後(afterP)
    public float[][] GetRotationX(Vector3 beforeP, Vector3 afterP)
    {
        //各ベクトルの絶対値
        float absBeforeP = Mathf.Sqrt(beforeP.x * beforeP.x + beforeP.y * beforeP.y + beforeP.z * beforeP.z);
        float absafterP = Mathf.Sqrt(afterP.x * afterP.x + afterP.y * afterP.y + afterP.z * afterP.z);

        float cos = InnerProduct(beforeP, afterP) / (absBeforeP * absafterP);
        float sin = Mathf.Sqrt(1 - cos * cos);

        //3x3の回転行列
        float[][] rotation =
        {
            new[]{1.0f, 0.0f, 0.0f},
            new[]{0.0f, cos, sin},
            new[]{0.0f, sin, cos}
        };

        return rotation;
    }

    //y軸での回転を求める
    //引数：回転前(beforeP)，回転後(afterP)
    public float[][] GetRotationY(Vector3 beforeP, Vector3 afterP)
    {
        //各ベクトルの絶対値
        float absBeforeP = Mathf.Sqrt(beforeP.x * beforeP.x + beforeP.y * beforeP.y + beforeP.z * beforeP.z);
        float absafterP = Mathf.Sqrt(afterP.x * afterP.x + afterP.y * afterP.y + afterP.z * afterP.z);

        float cos = InnerProduct(beforeP, afterP) / (absBeforeP * absafterP);
        float sin = Mathf.Sqrt(1 - cos * cos);

        //3x3の回転行列
        float[][] rotation =
        {
            new[]{cos, 0.0f, sin},
            new[]{0.0f, 1.0f, 0.0f},
            new[]{sin, 0.0f, cos}
        };

        return rotation;
    }

    //z軸での回転を求める
    //引数：回転前(beforeP)，回転後(afterP)
    public float[][] GetRotationZ(Vector3 beforeP, Vector3 afterP)
    {
        //各ベクトルの絶対値
        float absBeforeP = Mathf.Sqrt(beforeP.x * beforeP.x + beforeP.y * beforeP.y + beforeP.z * beforeP.z);
        float absafterP = Mathf.Sqrt(afterP.x * afterP.x + afterP.y * afterP.y + afterP.z * afterP.z);

        float cos = InnerProduct(beforeP, afterP) / (absBeforeP * absafterP);
        float sin = Mathf.Sqrt(1 - cos * cos);

        //3x3の回転行列
        float[][] rotation =
        {
            new[]{cos, -sin, 0.0f},
            new[]{sin,  cos, 0.0f},
            new[]{0.0f, 0.0f, 1.0f}
        };

        return rotation;
    }


    //CatmullRom曲線を作成しリストに格納
    //引数：制御点(p0～p3)，分割数(dv)，格納するリスト(list)
    public void CatmullRomSplineCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float dv, List<Vector3> list)
    {
        //曲線の係数
        Vector3 a4 = p1;
        Vector3 a3 = (p2 - p0) / 2.0f;
        Vector3 a1 = (p3 - p1) / 2.0f - 2.0f * p2 + a3 + 2.0f * a4;
        Vector3 a2 = 3.0f * p2 - (p3 - p1) / 2.0f - 2.0f * a3 - 3.0f * a4;

        //曲線上の点
        Vector3 position;
        //曲線上の点を計算し曲線のリストに格納
        for (int i = 0; i < dv; i++)
        {
            float s = i / dv;
            position = a1 * s * s * s + a2 * s * s + a3 * s + a4;
            list.Add(position);
        }
    }

    //CatmullRom曲線の接ベクトルを作りリストに格納
    //引数：制御点(p0～p3)，分割数(dv)，格納するリスト(list)
    public void CatmullRomSplineCurve_TangentVec(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float dv, List<Vector3> list)
    {
        //曲線の係数
        Vector3 m0 = (p2 - p0) / 2.0f;
        Vector3 m1 = (p3 - p1) / 2.0f;

        Vector3 a3 = m0;
        Vector3 a2 = -6.0f * p1 - 4.0f * m0 + 6.0f * p2 - 2 * m1;
        Vector3 a1 = 6.0f * p1 + 3.0f * m0 - 6.0f * p2 + 3.0f * m1;

        //接ベクトル
        Vector3 tangentVec;
        //接ベクトルを計算し接ベクトルのリストに格納
        for (int i = 0; i < dv; i++)
        {
            float s = i / dv;
            tangentVec = a1 * s * s + a2 * s + a3;
            list.Add(tangentVec);
        }
    }

    //直交平面を求める
    //引数：座標(Vec)，座標の接ベクトル(Tangentvec)
    public void OrthogonalPlane(Vector3 Vec, Vector3 Tangentvec)
    {
        float a = Tangentvec.x;
        float b = Tangentvec.y;
        float c = Tangentvec.z;
        float d = -a * Vec.x - b * Vec.y - c * Vec.z;
    }

    //シグモイド曲線を作る
    //引数：制御点のリスト(controllPoints)，曲線の分割数(dv)，曲がりのパラメータ(a, 0<=a<=1)
    //返り値：曲線の座標リスト
    public float[] Sigmoid(List<Vector3> controllPoints, int dv, float a)
    {
        //制御点の個数
        int cpNecks = controllPoints.Count;

        //シグモイド曲線を作る
        //曲線の点の個数
        int curvePoints = cpNecks * dv;
        //新しい制御点のy座標
        float[] newControllPoints = new float[curvePoints];
        //ステップ数
        float step = 6.0f / ((float)curvePoints - 1.0f);
        //変数x
        float x = -3.0f;
        //曲線を求めて格納
        for (int i = 0; i < curvePoints; i++)
        {
            float y = 1.0f * x / (1.0f + Mathf.Exp(-a * x));
            newControllPoints[i] = y;
            x = x + step;
            Debug.Log("x[" + i + "]:" + x + ",y[" + i + "]:" + y);

        }
        return newControllPoints;
    }
}
