using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingParameter : MonoBehaviour
{
    DragonBorneData dbd;
    WingBorneData wbd;
    WingParameterUI wingParameterUI;

    public Vector3[] left_wing_position =
    {
        //翼の下腕
        new Vector3(-1.2f,-0.2f,0.0f),
        new Vector3(0.0f,3.0f,0.0f),
        //翼の上腕
        new Vector3(0.0f,3.0f,0.0f),
        new Vector3(1.2f,6.0f,0.0f),
        //翼の指中央1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(3.9f,5.7f,0.0f),
        //翼の指中央2
        new Vector3(3.9f,5.7f,0.0f),
        new Vector3(7.7f,4.4f,0.0f),
        //翼の指内1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(2.2f,3.7f,0.0f),
        //翼の指内2
        new Vector3(2.2f,3.7f,0.0f),
        new Vector3(4.6f,1.3f,0.0f),
        //翼の指外1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(5.2f,8.0f,0.0f),
        //翼の指外2
        new Vector3(5.2f,8.0f,0.0f),
        new Vector3(9.4f,7.5f,0.0f),
    };

    public Vector3[] right_wing_position =
    {
       //翼の下腕
        new Vector3(-1.2f,-0.2f,0.0f),
        new Vector3(0.0f,3.0f,0.0f),
        //翼の上腕
        new Vector3(0.0f,3.0f,0.0f),
        new Vector3(1.2f,6.0f,0.0f),
        //翼の指中央1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(3.9f,5.7f,0.0f),
        //翼の指中央2
        new Vector3(3.9f,5.7f,0.0f),
        new Vector3(7.7f,4.4f,0.0f),
        //翼の指内1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(2.2f,3.7f,0.0f),
        //翼の指内2
        new Vector3(2.2f,3.7f,0.0f),
        new Vector3(4.6f,1.3f,0.0f),
        //翼の指外1
        new Vector3(1.2f,6.0f,0.0f),
        new Vector3(5.2f,8.0f,0.0f),
        //翼の指外2
        new Vector3(5.2f,8.0f,0.0f),
        new Vector3(9.4f,7.5f,0.0f),
    };

    public float[] wing_tension =
    {   /* 始点, 終点 */

        //翼の上腕
        1.0f,1.0f,
        1.0f,1.0f,
        //翼の指中央
        1.0f,1.0f,
        1.0f,1.0f,
        //翼の指内
        1.0f,1.0f,
        1.0f,1.0f,
        //翼の指外
        1.0f,1.0f,
        1.0f,1.0f,

    };

    public float[] wing_direction =
    {   /* 始点, 終点 */

        //翼の上腕
        0.0f,0.0f,
        0.0f,0.0f,
        //翼の指中央
        0.0f,0.0f,
        0.0f,0.0f,
        //翼の指内
        0.0f,0.0f,
        0.0f,0.0f,
        //翼の指外
        0.0f,0.0f,
        0.0f,0.0f,
    };

    //線形補間用のデータ
    // 0:wingupperarm_startmin,  1:wingupperarm_startmax
    // 2:wingupperarm_endmin,    3:wingupperarm_endmax
    // 4:wingforearm_startmin,   5:wingforearm_startmin
    // 6:wingforearm_endmax,     7:wingforearm_endmax
    // 8:wing_finger1_startmin,  9:wing_finger1_startmin
    //10:wing_finger1_endmax,   11:wing_finger1_endmax
    //12:wing_finger2_startmin, 13:wing_finger2_startmin
    //14:wing_finger2_endmax,   15:wing_finger2_endmax
    //16:wing_finger3_startmin, 17:wing_finger3_startmin
    //18:wing_finger3_endmax,   19:wing_finger3_endmax
    Vector3[] wing_positiondistance = new Vector3[20];
    float[] wing_tension_abs = new float[20];
    float[] wing_direction_abs = new float[20];

    //パラメータの変更を確認する
    int check_wingupperarm_long;
    int check_wingupperarm_size;
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

    void change_wing_size(Vector3[] baseP, float[] baseT, float[] baseD, Vector3[] maxP, float[] maxT, float[] maxD,
        Vector3[] minP, float[] minT, float[] minD, int parameterNum)
    {
        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 0; i < 2; i++)
                {
                    left_wing_position[i] = baseP[i];
                    right_wing_position[i] = baseP[i];
                    wing_tension[i] = baseT[i];
                    wing_direction[i] = baseD[i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の1,3を使う
                    left_wing_position[i] = wing_positiondistance[2 * i + 1];
                    right_wing_position[i] = wing_positiondistance[2 * i + 1];
                    wing_tension[i] = wing_tension_abs[i];
                    wing_direction[i] = wing_direction_abs[i];
                }
                break;

            case 2://maxにする
                for (int i = 0; i < 2; i++)
                {
                    left_wing_position[i] = maxP[i];
                    right_wing_position[i] = maxP[i];
                    wing_tension[i] = maxT[i];
                    wing_direction[i] = maxD[i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 0; i < 2; i++)
                {
                    //線形補間の0,2を使う
                    left_wing_position[i] = wing_positiondistance[2 * i];
                    right_wing_position[i] = wing_positiondistance[2 * i];
                    wing_tension[i] = wing_tension_abs[i];
                    wing_direction[i] = wing_direction_abs[i];
                }
                break;

            case -2://minにする
                for (int i = 0; i < 2; i++)
                {
                    left_wing_position[i] = minP[i];
                    right_wing_position[i] = minP[1];
                    wing_tension[i] = minT[i];
                    wing_direction[i] = minD[i];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //データのスクリプトを取得
        dbd = GameObject.Find("Doragon").GetComponent<DragonBorneData>();
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
        check_wingupperarm_long = 0;
        check_wingupperarm_size = 0;
        check_wingforearm_long = 0;
        check_wingforearm_size = 0;
        check_wing_size = 0;
        check_wing_number = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
