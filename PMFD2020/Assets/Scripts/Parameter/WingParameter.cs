using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingParameter : MonoBehaviour
{
    WingBorneData wbd;
    WingParameterUI wingParameterUI;

    public Vector3[] left_wing_position =
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

    public float[] wing_tension =
    {   /* 始点, 終点 */

        //翼の腕
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

    public float[] wing_direction =
    {   /* 始点, 終点 */

        //翼の腕
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

    //線形補間用のデータ
    // 0:wingunderarm_startmin,  1:wingunderarm_startmax
    // 2:wingunderarm_endmin,    3:wingunderarm_endmax
    // 4:wingforearm_startmin,   5:wingforearm_startmin
    // 6:wingforearm_endmax,     7:wingforearm_endmax
    // 8:wing_finger1a_startmin, 9:wing_finger1a_startmin
    //10:wing_finger1a_endmax,   11:wing_finger1a_endmax
    //12:wing_finger1b_startmin, 13:wing_finger1b_startmin
    //14:wing_finger1b_endmax,   15:wing_finger1b_endmax
    //16:wing_finger2a_startmin, 17:wing_finger2a_startmin
    //18:wing_finger2a_endmax,   19:wing_finger2a_endmax
    //20:wing_finger2b_startmin, 21:wing_finger2b_startmin
    //22:wing_finger2b_endmax,   23:wing_finger2b_endmax
    //24:wing_finger3a_startmin, 25:wing_finger3a_startmin
    //26:wing_finger3a_endmax,   27:wing_finger3a_endmax
    //28:wing_finger3b_startmin, 29:wing_finger3b_startmin
    //30:wing_finger3b_endmax,   31:wing_finger3b_endmax
    Vector3[] wing_positiondistance = new Vector3[32];
    float[] wing_tension_abs = new float[32];
    float[] wing_direction_abs = new float[32];

    //パラメータの変更を確認する
    int check_wingunderarm_long;
    int check_wingunderarm_size;
    int check_wingforearm_long;
    int check_wingforearm_size;
    int check_wing_size;
    int check_wing_number;
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

    //下腕の長さを変更
    void change_wingunderarm_long(Vector3[] baseP, float[] baseT, float[] baseD, Vector3[] maxP, float[] maxT, float[] maxD,
        Vector3[] minP, float[] minT, float[] minD, int parameterNum)
    {
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 0; i < 2; i++)
                {
                    left_wing_position[i] = baseP[i];
                    wing_tension[i] = baseT[i];
                    wing_direction[i] = baseD[i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の1,3を使う
                    left_wing_position[i] = wing_positiondistance[2 * i + 1];
                    wing_tension[i] = wing_tension_abs[i];
                    wing_direction[i] = wing_direction_abs[i];
                }
                break;

            case 2://maxにする
                for (int i = 0; i < 2; i++)
                {
                    left_wing_position[i] = maxP[i];
                    wing_tension[i] = maxT[i];
                    wing_direction[i] = maxD[i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の0,2を使う
                    left_wing_position[i] = wing_positiondistance[2 * i];
                    wing_tension[i] = wing_tension_abs[i];
                    wing_direction[i] = wing_direction_abs[i];
                }
                break;

            case -2://minにする
                for (int i = 0; i < 2; i++)
                {
                    left_wing_position[i] = minP[i];
                    wing_tension[i] = minT[i];
                    wing_direction[i] = minD[i];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
    }

    //上腕の長さを変更
    void change_wingforearm_long(Vector3[] baseP, float[] baseT, float[] baseD, Vector3[] maxP, float[] maxT, float[] maxD,
        Vector3[] minP, float[] minT, float[] minD, int parameterNum, Vector3 left_long)
    {
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 2; i < 4; i++)
                {
                    left_wing_position[i] = baseP[i];
                    wing_tension[i] = baseT[i];
                    wing_direction[i] = baseD[i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の1,3を使う
                    left_wing_position[i] = wing_positiondistance[2 * i + 1];
                    wing_tension[i] = wing_tension_abs[i];
                    wing_direction[i] = wing_direction_abs[i];
                }
                break;

            case 2://maxにする
                for (int i = 2; i < 4; i++)
                {
                    left_wing_position[i] = maxP[i];
                    wing_tension[i] = maxT[i];
                    wing_direction[i] = maxD[i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の0,2を使う
                    left_wing_position[i] = wing_positiondistance[2 * i];
                    wing_tension[i] = wing_tension_abs[i];
                    wing_direction[i] = wing_direction_abs[i];
                }
                break;

            case -2://minにする
                for (int i = 2; i < 4; i++)
                {
                    left_wing_position[i] = minP[i];
                    wing_tension[i] = minT[i];
                    wing_direction[i] = minD[i];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
        //下腕の変形による上腕の座標を補正
        for (int i = 2; i < 4; i++)
        {
            Vector3 left_wing_correction = left_wing_position[i] + left_long;
            left_wing_position[i] = left_wing_correction;
        }
    }

    //翼のサイズを変更
    void change_wing_size(Vector3[] baseP, float[] baseT, float[] baseD, Vector3[] maxP, float[] maxT, float[] maxD,
        Vector3[] minP, float[] minT, float[] minD, int parameterNum, Vector3 left_long)
    {
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 4; i < 16; i++)
                {
                    left_wing_position[i] = baseP[i];
                    wing_tension[i] = baseT[i];
                    wing_direction[i] = baseD[i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 4; i < 16; i++)
                {
                    //線形補間の1,3を使う
                    left_wing_position[i] = wing_positiondistance[2 * i + 1];
                    wing_tension[i] = wing_tension_abs[i];
                    wing_direction[i] = wing_direction_abs[i];
                }
                break;

            case 2://maxにする
                for (int i = 4; i < 16; i++)
                {
                    left_wing_position[i] = maxP[i];
                    wing_tension[i] = maxT[i];
                    wing_direction[i] = maxD[i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 4; i < 16; i++)
                {
                    //線形補間の0,2を使う
                    left_wing_position[i] = wing_positiondistance[2 * i];
                    wing_tension[i] = wing_tension_abs[i];
                    wing_direction[i] = wing_direction_abs[i];
                }
                break;

            case -2://minにする
                for (int i = 4; i < 16; i++)
                {
                    left_wing_position[i] = minP[i];
                    wing_tension[i] = minT[i];
                    wing_direction[i] = minD[i];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
        //上腕の変形による翼の座標を補正
        for (int i = 4; i < 16; i++)
        {
            Vector3 left_wing_correction = left_wing_position[i] + left_long;
            left_wing_position[i] = left_wing_correction;
        }
    }

    //翼の枚数の変更

    // Start is called before the first frame update
    void Start()
    {
        //データのスクリプトを取得
        wbd = GameObject.Find("Doragon").GetComponent<WingBorneData>();
        wingParameterUI = GameObject.Find("wingPanel").GetComponent<WingParameterUI>();

        //翼に関係するデータを取得し線形補間のデータを作る
        for (int i = 0; i < 16; i++)
        {
            //座標の最大と最小の幅を計算
            int j = 2 * i;
            wing_positiondistance[j] = (wbd.wing_nomalposition[i] + wbd.wing_minposition[i]) * 0.5f;
            wing_positiondistance[j + 1] = (wbd.wing_maxposition[i] + wbd.wing_nomalposition[i]) * 0.5f;

            //tensionとvelocityの最大と最小の幅を計算
            wing_tension_abs[j] = (wbd.wing_nomaltension[i] + wbd.wing_mintension[i]) * 0.5f;
            wing_tension_abs[j + 1] = (wbd.wing_nomaltension[i] + wbd.wing_maxtension[i]) * 0.5f;
            wing_direction_abs[j] = (wbd.wing_nomaldirection[i] + wbd.wing_mindirection[i]) * 0.5f;
            wing_direction_abs[j + 1] = (wbd.wing_nomaldirection[i] + wbd.wing_maxdirection[i]) * 0.5f;
        }

        //初期化
        check_wingunderarm_long = 0;
        check_wingunderarm_size = 0;
        check_wingforearm_long = 0;
        check_wingforearm_size = 0;
        check_wing_size = 0;
        check_wing_number = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // パラメータが変化したか確認
        check_wingunderarm_long = CheckChengeParameter(check_wingunderarm_long, wingParameterUI.wingunderarm_long_throwValue);
        check_wingunderarm_size = CheckChengeParameter(check_wingunderarm_size, wingParameterUI.wingunderarm_size_throwValue);
        check_wingforearm_long = CheckChengeParameter(check_wingforearm_long, wingParameterUI.wingforearm_long_throwValue);
        check_wingforearm_size = CheckChengeParameter(check_wingforearm_size, wingParameterUI.wingforearm_size_throwValue);
        check_wing_number = CheckChengeParameter(check_wing_number, wingParameterUI.wing_numberSlider_throwValue);

        //下腕の長さを変更
        change_wingunderarm_long(wbd.wing_nomalposition, wbd.wing_nomaltension, wbd.wing_nomaldirection, 
            wbd.wing_maxposition, wbd.wing_maxtension, wbd.wing_maxdirection,
            wbd.wing_minposition, wbd.wing_mintension, wbd.wing_mindirection,
            check_wingunderarm_long);

        //下腕の長さを取得
        Vector3 left_wingunderarm_long = left_wing_position[1] - wbd.wing_nomalposition[1];

        //上腕の長さを変更
        change_wingforearm_long(wbd.wing_nomalposition, wbd.wing_nomaltension, wbd.wing_nomaldirection, 
            wbd.wing_maxposition, wbd.wing_maxtension, wbd.wing_maxdirection, 
            wbd.wing_minposition, wbd.wing_mintension, wbd.wing_mindirection,
            check_wingforearm_long, left_wingunderarm_long);

        //上腕の長さを変更
        Vector3 left_wingforearm_long = left_wing_position[3] - wbd.wing_nomalposition[3];

        //翼のサイズを変更
        change_wing_size(wbd.wing_nomalposition, wbd.wing_nomaltension, wbd.wing_nomaldirection,
            wbd.wing_maxposition, wbd.wing_maxtension, wbd.wing_maxdirection,
            wbd.wing_minposition, wbd.wing_mintension, wbd.wing_mindirection,
            check_wingforearm_size, left_wingforearm_long);
    }
}
