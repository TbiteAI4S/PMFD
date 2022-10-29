using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootParameter : MonoBehaviour
{
    DragonBorneData dbd;
    FootParameterUI footParameterUI;

    public Vector3[] left_foot_position =
    {
        //大腿 thigh
        new Vector3(4.3f, -1.6f, 0.0f),
        new Vector3(4.5f, -3.2f, 0.0f),
        //下腿 lower_leg
        new Vector3(4.5f, -3.2f, 0.0f),
        new Vector3(4.3f, -4.7f, 0.0f),
        //足 foot
        new Vector3(4.3f, -4.7f, 0.0f),
        new Vector3(2.9f, -5.4f, 0.0f)
    };

    public Vector3[] right_foot_position =
    {
        //大腿 thigh
        new Vector3(4.3f, -1.6f, 0.0f),
        new Vector3(4.5f, -3.2f, 0.0f),
        //下腿 lower_leg
        new Vector3(4.5f, -3.2f, 0.0f),
        new Vector3(4.3f, -4.7f, 0.0f),
        //足 foot
        new Vector3(4.3f, -4.7f, 0.0f),
        new Vector3(2.9f, -5.4f, 0.0f)
    };

    public float[] foot_tension =
    {
        //大腿 thigh
        3.7f,0.6f,
        //下腿 lower_leg
        1.0f,-0.6f,
        //足 foot
        1.0f,1.0f,
    };

    public float[] foot_direction =
    {
        //大腿 thigh
        0.1f,1.0f,
        //下腿 lower_leg
        0.0f,-1.0f,
        //足 foot
        0.0f,0.0f,
    };

    //線形補間用のデータ
    //0:thigh_startmin,     1:thigh_startmax
    //2:thigh_endmin,       3:thigh_endmax
    //4:lower_leg_startmin, 5:lower_leg_startmin
    //6:lower_leg_endmax,   7:lower_leg_endmax
    //8:foot_startmin,      9:foot_startmin
    //10:foot_endmax,       11:foot_endmax
    Vector3[] foot_positiondistance = new Vector3[12];
    float[] foot_tension_abs = new float[12];
    float[] foot_direction_abs = new float[12];

    //パラメータの変更を確認する
    int check_thigh_long;
    int check_thigh_size;
    int check_lower_leg_long;
    int check_lower_leg_size;
    int check_foot_size;
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

    //大腿の長さを変更
    void change_thigh_long(Vector3[] baseP, Vector3[] maxP, Vector3[] minP, int parameterNum)
    {
        int k = 14;
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 0; i < 2; i++)
                {
                    left_foot_position[i] = baseP[k + i];
                    right_foot_position[i] = baseP[k + i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の1,3を使う
                    left_foot_position[i] = foot_positiondistance[2 * i + 1];
                    right_foot_position[i] = foot_positiondistance[2 * i + 1];
                }
                break;

            case 2://maxにする
                for (int i = 0; i < 2; i++)
                {
                    left_foot_position[i] = maxP[k + i];
                    right_foot_position[i] = maxP[k + i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の0,2を使う
                    left_foot_position[i] = foot_positiondistance[2 * i];
                    right_foot_position[i] = foot_positiondistance[2 * i];
                }
                break;

            case -2://minにする
                for (int i = 0; i < 2; i++)
                {
                    left_foot_position[i] = minP[k + i];
                    right_foot_position[i] = minP[k + 1];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
    }

    //下腿の長さを変更
    void change_lower_leg_long(Vector3[] baseP, Vector3[] maxP, Vector3[] minP, int parameterNum, Vector3 left_long, Vector3 right_long)
    {
        int k = 14;
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 2; i < 4; i++)
                {
                    left_foot_position[i] = baseP[k + i];
                    right_foot_position[i] = baseP[k + i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の1,3を使う
                    left_foot_position[i] = foot_positiondistance[2 * i + 1];
                    right_foot_position[i] = foot_positiondistance[2 * i + 1];
                }
                break;

            case 2://maxにする
                for (int i = 2; i < 4; i++)
                {
                    left_foot_position[i] = maxP[k + i];
                    right_foot_position[i] = maxP[k + i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の0,2を使う
                    left_foot_position[i] = foot_positiondistance[2 * i];
                    right_foot_position[i] = foot_positiondistance[2 * i];
                }
                break;

            case -2://minにする
                for (int i = 2; i < 4; i++)
                {
                    left_foot_position[i] = minP[k + i];
                    right_foot_position[i] = minP[k + 1];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
        //大腿の変形を補正
        for (int i = 2; i < 4; i++)
        {
            Vector3 leftupper_correction = left_foot_position[i] + left_long;
            left_foot_position[i] = leftupper_correction;
            Vector3 rightupper_correction = right_foot_position[i] + right_long;
            right_foot_position[i] = rightupper_correction;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //データのスクリプトを取得
        dbd = GameObject.Find("Doragon").GetComponent<DragonBorneData>();
        footParameterUI = GameObject.Find("footPanel").GetComponent<FootParameterUI>();

        //後ろ足に関係するデータを取得し線形補間のデータを作る
        for (int i = 0; i < 4; i++)
        {
            int k = 10 + i;

            //座標の最大と最小の幅を計算
            int j = 2 * i;
            foot_positiondistance[j] = (dbd.nomalposition[k] + dbd.minposition[k]) * 0.5f;
            foot_positiondistance[j + 1] = (dbd.maxposition[k] + dbd.nomalposition[k]) * 0.5f;

            //tensionとvelocityの最大と最小の幅を計算
            foot_tension_abs[j] = (dbd.nomaltension[k] + dbd.mintension[k]) * 0.5f;
            foot_tension_abs[j + 1] = (dbd.nomaltension[k] + dbd.maxtension[k]) * 0.5f;
            foot_direction_abs[j] = (dbd.nomaldirection[k] + dbd.mindirection[k]) * 0.5f;
            foot_direction_abs[j + 1] = (dbd.nomaldirection[k] + dbd.maxdirection[k]) * 0.5f;
        }

        //初期化
        check_thigh_long = 0;
        check_thigh_size = 0;
        check_lower_leg_long = 0;
        check_lower_leg_size = 0;
        check_foot_size = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // パラメータが変化したか確認
        check_thigh_long = CheckChengeParameter(check_thigh_long, footParameterUI.thigh_long_throwValue);
        check_thigh_size = CheckChengeParameter(check_thigh_size, footParameterUI.thigh_size_throwValue);
        check_lower_leg_long = CheckChengeParameter(check_lower_leg_long, footParameterUI.lower_leg_long_throwValue);
        check_lower_leg_size = CheckChengeParameter(check_lower_leg_size, footParameterUI.lower_leg_size_throwValue);
        check_foot_size = CheckChengeParameter(check_foot_size, footParameterUI.footnail_size_throwValue);

        //大腿の長さを変更
        change_thigh_long(dbd.nomalposition, dbd.maxposition, dbd.minposition, check_thigh_long);

        //大腿の長さを取得
        Vector3 left_thighlong = left_foot_position[1] - dbd.nomalposition[14];
        Vector3 right_thighlong = right_foot_position[1] - dbd.nomalposition[14];

        //下腿の長さを変更
        change_lower_leg_long(dbd.nomalposition, dbd.maxposition, dbd.minposition, 
            check_lower_leg_long, left_thighlong, right_thighlong);
    }
}
