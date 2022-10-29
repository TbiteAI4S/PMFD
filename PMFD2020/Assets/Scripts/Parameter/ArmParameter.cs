using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmParameter : MonoBehaviour
{
    DragonBorneData dbd;
    ArmParameterUI armParameterUI;

    public Vector3[] left_arm_position =
    {
        //上腕 upper_arm
        new Vector3(-1.2f, -1.8f, 0.0f),
        new Vector3(-1.1f, -3.7f, 0.0f),
        //前腕 forearm
        new Vector3(-1.1f, -3.7f, 0.0f),
        new Vector3(-2.8f, -5.2f, 0.0f),
    };

    public Vector3[] right_arm_position =
    {
        //上腕 upper_arm
        new Vector3(-1.2f, -1.8f, 0.0f),
        new Vector3(-1.1f, -3.7f, 0.0f),
        //前腕 forearm
        new Vector3(-1.1f, -3.7f, 0.0f),
        new Vector3(-2.8f, -5.2f, 0.0f),
    };

    public float[] arm_tension =
    {
        //上腕 upper_arm
        3.2f,-0.2f,
        //前腕 forearm
        1.0f,1.0f,
    };

    public float[] arm_direction =
    {
        //上腕 upper_arm
        0.0f,0.0f,
        //前腕 forearm
        0.4f,0.6f,
    };

    //線形補間用のデータ
    //0:upper_arm_startmin, 1:upper_arm_startmax
    //2:upper_arm_endmin,   3:upper_arm_endmax
    //4:forearm_startmin,   5:forearm_startmin
    //6:forearm_endmax,     7:forearm_endmax
    Vector3[] arm_positiondistance = new Vector3[8];
    float[] arm_tension_abs = new float[8];
    float[] arm_direction_abs = new float[8];

    //パラメータの変更を確認する
    int check_upperarm_long;
    int check_upperarm_size;
    int check_forearm_long;
    int check_forearm_size;
    int check_nail_size;
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

    //上腕の長さを変更
    void change_upperarm_long(Vector3[] baseP, Vector3[] maxP, Vector3[] minP, int parameterNum)
    {
        int k = 10;
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 0; i < 2; i++)
                {
                    left_arm_position[i] = baseP[k + i];
                    right_arm_position[i] = baseP[k + i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の1,3を使う
                    left_arm_position[i] = arm_positiondistance[2 * i + 1];
                    right_arm_position[i] = arm_positiondistance[2 * i + 1];
                }
                break;

            case 2://maxにする
                for (int i = 0; i < 2; i++)
                {
                    left_arm_position[i] = maxP[k + i];
                    right_arm_position[i] = maxP[k + i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の0,2を使う
                    left_arm_position[i] = arm_positiondistance[2 * i];
                    right_arm_position[i]= arm_positiondistance[2 * i];
                }
                break;

            case -2://minにする
                for (int i = 0; i < 2; i++)
                {
                    left_arm_position[i] = minP[k + i];
                    right_arm_position[i] = minP[k + 1];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
    }

    //前腕の長さを変更
    void change_forearm_long(Vector3[] baseP, Vector3[] maxP, Vector3[] minP, int parameterNum, Vector3 left_long, Vector3 right_long)
    {
        int k = 10;
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 2; i < 4; i++)
                {
                    left_arm_position[i] = baseP[k + i];
                    right_arm_position[i] = baseP[k + i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の1,3を使う
                    left_arm_position[i] = arm_positiondistance[2 * i + 1];
                    right_arm_position[i] = arm_positiondistance[2 * i + 1];
                }
                break;

            case 2://maxにする
                for (int i = 2; i < 4; i++)
                {
                    left_arm_position[i] = maxP[k + i];
                    right_arm_position[i] = maxP[k + i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の0,2を使う
                    left_arm_position[i] = arm_positiondistance[2 * i];
                    right_arm_position[i] = arm_positiondistance[2 * i];
                }
                break;

            case -2://minにする
                for (int i = 2; i < 4; i++)
                {
                    left_arm_position[i] = minP[k + i];
                    right_arm_position[i] = minP[k + 1];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
        //上腕の変形を補正
        for (int i = 2; i < 4; i++)
        {
            Vector3 leftupper_correction = left_arm_position[i] + left_long;
            left_arm_position[i] = leftupper_correction;
            Vector3 rightupper_correction = right_arm_position[i] + right_long;
            right_arm_position[i] = rightupper_correction;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //データのスクリプトを取得
        dbd = GameObject.Find("Doragon").GetComponent<DragonBorneData>();
        armParameterUI = GameObject.Find("armPanel").GetComponent<ArmParameterUI>();

        //前足に関係するデータを取得し線形補間のデータを作る
        for (int i = 0; i < 4; i++)
        {
            int k = 10 + i;

            //座標の最大と最小の幅を計算
            int j = 2 * i;
            arm_positiondistance[j] = (dbd.nomalposition[k] + dbd.minposition[k]) * 0.5f;
            arm_positiondistance[j + 1] = (dbd.maxposition[k] + dbd.nomalposition[k]) * 0.5f;

            //tensionとvelocityの最大と最小の幅を計算
            arm_tension_abs[j] = (dbd.nomaltension[k] + dbd.mintension[k]) * 0.5f;
            arm_tension_abs[j + 1] = (dbd.nomaltension[k] + dbd.maxtension[k]) * 0.5f;
            arm_direction_abs[j] = (dbd.nomaldirection[k] + dbd.mindirection[k]) * 0.5f;
            arm_direction_abs[j + 1] = (dbd.nomaldirection[k] + dbd.maxdirection[k]) * 0.5f;
        }

        //初期化
        check_upperarm_long = 0;
        check_upperarm_size = 0;
        check_forearm_long = 0;
        check_forearm_size = 0;
        check_nail_size = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //パラメータが変化したか確認
        check_upperarm_long = CheckChengeParameter(check_upperarm_long, armParameterUI.upperarm_long_throwValue);
        check_upperarm_size = CheckChengeParameter(check_upperarm_size, armParameterUI.upperarm_size_throwValue);
        check_forearm_long = CheckChengeParameter(check_forearm_long, armParameterUI.forearm_long_throwValue);
        check_forearm_size = CheckChengeParameter(check_forearm_size, armParameterUI.forearm_size_throwValue);
        check_nail_size = CheckChengeParameter(check_nail_size, armParameterUI.nail_size_throwValue);

        //上腕の長さを変更
        change_upperarm_long(dbd.nomalposition, dbd.maxposition, dbd.minposition, check_upperarm_long);

        //上腕の長さを取得
        Vector3 left_upperlong= left_arm_position[1] - dbd.nomalposition[11];
        Vector3 right_upperlong = left_arm_position[1] - dbd.nomalposition[11];

        //前腕の長さを変更
        change_forearm_long(dbd.nomalposition, dbd.maxposition, dbd.minposition, 
            check_forearm_long, left_upperlong, right_upperlong);
    }
}
