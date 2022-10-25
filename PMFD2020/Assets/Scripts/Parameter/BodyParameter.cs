using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParameter : MonoBehaviour
{
    DragonBorneData dbd;
    BodyParameterUI bodyParameterUI;

    //頭に関するドラゴンボーンのデータ
    public Vector3[] body_position =
    {
        //首 neck
        new Vector3(-4.4f, -1.2f, 0.0f),
        new Vector3(-2.3f, -1.1f, 0.0f),
        //胴体 body
        new Vector3(-2.3f, -1.1f, 0.0f),
        new Vector3(5.1f, -0.5f, 0.0f),
        //尾 tail
        new Vector3(5.1f, -0.5f, 0.0f),
        new Vector3(12.8f, -0.9f, 0.0f),
    };
    public float[] body_tension =
    {
         //首 neck
        1.0f,1.0f,
        //胴体 body
        3.6f,-2.6f,
        //尾 tail
        1.0f,1.0f,
    };
    public float[] body_direction =
    {
        //首 neck
        0.0f,0.0f,
        //胴体 body
        0.8f,0.4f,
        //尾 tail
        0.0f,0.0f,
    };

    //線形補間用のデータ
    //0:neck_startmin,  1:neck_startmax
    //2:neck_endmin,    3:neck_endmax
    //4:body_startmin,  5:body_startmin
    //6:body_endmax,    7:body_endmax
    //8:tail_startmin,  9:tail_startmin
    //10:tail_endmax,   11:tail_endmax
    Vector3[] body_positiondistance = new Vector3[12];
    float[] body_tension_abs = new float[12];
    float[] body_direction_abs = new float[12];

    //パラメータの変更を確認する
    int check_neck_height;
    int check_body_long;
    int check_bodyback;
    int check_tail_long;
    int CheckChengeParameter(int checkparameterValue, int throwValue/*スライダーから来る値*/)
    {
        //値が異なれば変更
        if (throwValue < checkparameterValue || throwValue > checkparameterValue)
        {
            //更新
            checkparameterValue = throwValue;
        }
        return checkparameterValue;
    }

    //首の高さを変更
    void change_neck_height(Vector3[] baseP, float[] baseT, float[] baseD, Vector3[] maxP, float[] maxT, float[] maxD,
        Vector3[] minP, float[] minT, float[] minD, int parameterNum)
    {
        int k = 4;
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 0; i < 2; i++)
                {
                    body_position[i] = baseP[k + i];
                    body_tension[i] = baseT[k + i];
                    body_direction[i] = baseD[k + i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の1,3を使う
                    body_position[i] = body_positiondistance[2 * i + 1];
                    body_tension[i] = body_tension_abs[2 * i + 1];
                    body_direction[i] = body_direction_abs[2 * i + 1];
                }
                break;

            case 2://maxにする
                for (int i = 0; i < 2; i++)
                {
                    body_position[i] = maxP[k + i];
                    body_tension[i] = maxT[k + i];
                    body_direction[i] = maxD[k + i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の0,2を使う
                    body_position[i] = body_positiondistance[2 * i];
                    body_tension[i] = body_tension_abs[2 * i];
                    body_direction[i] = body_direction_abs[2 * i];
                }
                break;

            case -2://minにする
                for (int i = 0; i < 2; i++)
                {
                    body_position[i] = minP[k + i];
                    body_tension[i] = minT[k + i];
                    body_direction[i] = minD[k + i];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
    }

    //胴体の長さを変更
    void change_body_long(Vector3[] baseP, Vector3[] maxP, Vector3[] minP, int parameterNum)
    {
        int k = 4;
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 2; i < 4; i++)
                {
                    body_position[i] = baseP[k + i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の1,3を使う
                    body_position[i] = body_positiondistance[2 * i + 1];
                }
                break;

            case 2://maxにする
                for (int i = 2; i < 4; i++)
                {
                    body_position[i] = maxP[k + i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の0,2を使う
                    body_position[i] = body_positiondistance[2 * i];
                }
                break;

            case -2://minにする
                for (int i = 2; i < 4; i++)
                {
                    body_position[i] = minP[k + i];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
    }

    //胴体の曲がりを設定
    void change_bodyback(float[] baseT, float[] baseD, float[] maxT, float[] maxD, float[] minT, float[] minD, int parameterNum)
    {
        int k = 4;
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 2; i < 4; i++)
                {
                    body_tension[i] = baseT[k + i];
                    body_direction[i] = baseD[k + i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の1,3を使う
                    body_tension[i] = body_tension_abs[2 * i + 1];
                    body_direction[i] = body_direction_abs[2 * i + 1];
                }
                break;

            case 2://maxにする
                for (int i = 2; i < 4; i++)
                {
                    body_tension[i] = maxT[k + i];
                    body_direction[i] = maxD[k + i];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
    }

    //尾の変形
    void change_tail(Vector3[] baseP, float[] baseT, float[] baseD, Vector3[] maxP, float[] maxT, float[] maxD,
        Vector3[] minP, float[] minT, float[] minD, int parameterNum, Vector3 body_long)
    {
        int k = 4;
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 4; i < 6; i++)
                {
                    body_position[i] = baseP[k + i];
                    body_tension[i] = baseT[k + i];
                    body_direction[i] = baseD[k + i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 4; i < 6; i++)
                {
                    //線形補間の1,3を使う
                    body_position[i] = body_positiondistance[2 * i + 1];
                    body_tension[i] = body_tension_abs[2 * i + 1];
                    body_direction[i] = body_direction_abs[2 * i + 1];
                }
                break;

            case 2://maxにする
                for (int i = 4; i < 6; i++)
                {
                    body_position[i] = maxP[k + i];
                    body_tension[i] = maxT[k + i];
                    body_direction[i] = maxD[k + i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 4; i < 6; i++)
                {
                    //線形補間の0,2を使う
                    body_position[i] = body_positiondistance[2 * i];
                    body_tension[i] = body_tension_abs[2 * i];
                    body_direction[i] = body_direction_abs[2 * i];
                }
                break;

            case -2://minにする
                for (int i = 4; i < 6; i++)
                {
                    body_position[i] = minP[k + i];
                    body_tension[i] = minT[k + i];
                    body_direction[i] = minD[k + i];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
        for (int i = 4; i < 6; i++)
        {
            Debug.Log("body_position[" + i + "]"+body_position[i]);
            Debug.Log("body_tension[" + i + "]" + body_tension[i]);
            Debug.Log("body_direction[" + i + "]" + body_direction[i]);
        }
        //胴体の変形を補正
        for (int i = 4; i < 6; i++)
        {
            Vector3 body_correction = body_position[i] + body_long;
            body_position[i] = body_correction;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //データのスクリプトを取得
        dbd = GameObject.Find("Doragon").GetComponent<DragonBorneData>();
        bodyParameterUI = GameObject.Find("bodyPanel").GetComponent<BodyParameterUI>();
        
        //胴体に関係するデータを取得し線形補間のデータを作る
        for (int i = 0; i < 6; i++)
        {
            int k = 4 + i;

            //座標の最大と最小の幅を計算
            int j = 2 * i;
            body_positiondistance[j] = (dbd.nomalposition[k] + dbd.minposition[k]) * 0.5f;
            body_positiondistance[j + 1] = (dbd.maxposition[k] + dbd.nomalposition[k]) * 0.5f;
            
            //tensionとvelocityの最大と最小の幅を計算
            body_tension_abs[j] = (dbd.nomaltension[k] + dbd.mintension[k]) * 0.5f;
            body_tension_abs[j + 1] = (dbd.nomaltension[k] + dbd.maxtension[k]) * 0.5f;
            body_direction_abs[j] = (dbd.nomaldirection[k] + dbd.mindirection[k]) * 0.5f;
            body_direction_abs[j + 1] = (dbd.nomaldirection[k] + dbd.maxdirection[k]) * 0.5f;
        }
        
        //初期化
        check_neck_height = 0;
        check_body_long = 0;
        check_bodyback = 0;
        check_tail_long = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //パラメータが変化したか確認
        check_neck_height = CheckChengeParameter(check_neck_height, bodyParameterUI.neck_height_throwValue);
        check_body_long = CheckChengeParameter(check_body_long, bodyParameterUI.body_long_throwValue);
        check_bodyback = CheckChengeParameter(check_bodyback, bodyParameterUI.bodyback_throwValue);
        check_tail_long = CheckChengeParameter(check_tail_long, bodyParameterUI.tail_long_throwValue);

        //首の高さを変える
        change_neck_height(dbd.nomalposition, dbd.nomaltension, dbd.nomaldirection, dbd.maxposition,
            dbd.maxtension, dbd.maxdirection, dbd.minposition, dbd.mintension, dbd.mindirection, check_neck_height);

        //胴体の長さを変える
        change_body_long(dbd.nomalposition, dbd.maxposition, dbd.minposition, check_body_long);

        //胴体の曲がりを変える
        change_bodyback(dbd.nomaltension, dbd.nomaldirection, dbd.maxtension, dbd.maxdirection, dbd.mintension, dbd.mindirection, check_bodyback);

        //胴体の長さを取得
        Vector3 bodylong = body_position[4] - dbd.nomalposition[8];

        //尾の長さを変える
        change_tail(dbd.nomalposition, dbd.nomaltension, dbd.nomaldirection, dbd.maxposition,
            dbd.maxtension, dbd.maxdirection, dbd.minposition, dbd.mintension, dbd.mindirection, check_tail_long, bodylong);
    }
}
