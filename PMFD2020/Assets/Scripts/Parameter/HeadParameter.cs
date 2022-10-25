using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.Rendering;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class HeadParameter : MonoBehaviour
{
    DragonBorneData dbd;
    HeadParameterUI headParameterUI;
    BodyParameter bodyParameter;

    //頭に関するドラゴンボーンのデータ
    public Vector3[] head_position =
    {
        //頭(上あご) head
        new Vector3(-6.3f, -2.5f, 0.0f),
        new Vector3(-4.6f, -0.7f, 0.0f),
        //下顎 jaw
        new Vector3(-5.5f, -3.1f, 0.0f),
        new Vector3(-4.7f, -1.5f, 0.0f),
    };
    public float[] head_tension = 
    {
        //頭(上あご) head
        -0.4f,1.0f,
        //下顎 jaw
        1.6f,1.0f
    };
    public float[] head_direction =
    {
        //頭(上あご) head
        -0.4f,1.0f,
        //下顎 jaw
        1.6f,1.0f
    };

    //線形補間用のデータ(全て1/2倍)
    //0:head_startmin, 1:head_startmax
    //2:head_endmin,   3:head_endmax
    //4:jaw_startmin,  5:jaw_startmin
    //6:jaw_endmax,    7:jaw_endmax
    Vector3[] head_positiondistance = new Vector3[8];
    float[] head_tension_abs = new float[8];
    float[] head_direction_abs = new float[8];

    //パラメータの変更を確認する
    int check_forehead_height;
    int check_jaw_tension;
    int check_upper_mouse;
    int check_under_mouse;
    float check_neck;
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

    /* 変形するパラメータ関数 */
    //額の高さの変更
    //base:dbdのbase, max:dbdのmax, min:dbdのmin
    void change_forehead_height(float[] baseT, float[] baseD, float[] maxT, float[] maxD, float[] minT, float[] minD, int parameterNum)
    {
        if(parameterNum == 1)
        {
            //max方向に1つ増やす
            //線形補間の1,3を使う
            for (int i = 0; i < 2; i++)
            {
                head_tension[i] = head_tension_abs[2 * i + 1];
                head_direction[i] = head_direction_abs[2 * i + 3];
            }
        }
        else if(parameterNum == 2)
        {
            //maxにする
            for (int i = 0; i < 2; i++)
            {
                head_tension[i] = maxT[i];
                head_direction[i] = maxD[i];
            }
        }
        else if(parameterNum == -1)
        {
            //min方向に1つ増やす
            for (int i = 0; i < 2; i++)
            {
                //線形補間の0,2を使う
                head_tension[i] = head_tension_abs[2 * i];
                head_direction[i] = head_direction_abs[2 * i];
            }
        }
        else if(parameterNum == -2)
        {
            //minにする
            for (int i = 0; i < 2; i++)
            {
                head_tension[i] = minT[i];
                head_direction[i] = minD[i];
            }
        }
        else
        {
            //baseにする
            for (int i = 0; i < 2; i++)
            {
                head_tension[i] = baseD[i];
                head_direction[i] = baseT[i];
            }
        }
    }

    //顎の張りの変更
    //base:dbdのbase, max:dbdのmax, min:dbdのmin
    void change_jaw_tension(float[] baseT, float[] baseD,float[] maxT, float[] maxD, float[] minT, float[] minD, int parameterNum)
    {
        if(parameterNum == 1)
        {
            //max方向に1つ増やす
            for (int i = 2; i < 4; i++)
            {
                //線形補間の5,7を使う
                head_tension[i] = head_tension_abs[2 * i + 1];
                head_direction[i] = head_direction_abs[2 * i + 1];
            }            
        }else if (parameterNum == 2)
        {
            //maxにする
            for (int i = 2; i < 4; i++)
            {
                head_tension[i] = maxT[i];
                head_direction[i] = maxD[i];
            }
        }else if(parameterNum == -1)
        {
            //min方向に1つ増やす
            for (int i = 2; i < 4; i++)
            {
                //線形補間の4,6を使う
                head_tension[i] = head_tension_abs[2 * i + 1];
                head_direction[i] = head_direction_abs[2 * i + 1];
            }
        }else if (parameterNum == -2)
        {
            //minにする
            for (int i = 2; i < 4; i++)
            {
                head_tension[i] = minT[i];
                head_direction[i] = minD[i];
            }
        }
        else
        {
            //baseにする
            for (int i = 2; i < 4; i++)
            {
                head_tension[i] = baseT[i];
                head_direction[i] = baseD[i];
            }
        }
        
    }

    //上口の長さの変更
    //base:dbdのbase, max:dbdのmax, min:dbdのmin
    void change_upper_mouse(Vector3[] baseP, Vector3[] maxP, Vector3[] minP, int parameterNum, float neckheght)
    {
        Vector3 neckY = new Vector3(0f, neckheght, 0f);

        if(parameterNum == 1)//max方向に1つ増やす
        {
            for (int i = 0; i < 2; i++)
            {
                //線形補間の1,3を使う
                head_position[i] = head_positiondistance[2 * i + 1];
            }
        }else if(parameterNum == 2)//maxにする
        {
            for (int i = 0; i < 2; i++)
            {
                head_position[i] = maxP[i];
            }
        }else if (parameterNum == -1)//min方向に1つ増やす
        {
            for (int i = 0; i < 2; i++)
            {
                //線形補間の0,2を使う
                head_position[i] = head_positiondistance[2 * i];
            }
        }else if (parameterNum == -2)//minにする
        {
            for (int i = 0; i < 2; i++)
            {
                head_position[i] = minP[i];
            }
        }
        else
        {
            //baseにする
            for (int i = 0; i < 2; i++)
            {
                head_position[i] = baseP[i];
            }
        }
        
        //首の高さにそろえる
        for(int i = 0; i < 2; i++)
        {
            Vector3 neck_correction = head_position[i] + neckY;
            head_position[i] = neck_correction;
        }
        
    }

    //下口の長さの変更
    //base:dbdのbase, max:dbdのmax, min:dbdのmin
    void change_under_mouse(Vector3[] baseP, Vector3[] maxP, Vector3[] minP, int parameterNum, float neckheght)
    {
        Vector3 neckY = new Vector3(0f, neckheght, 0f);

        switch (parameterNum)
        {
            case 0://baseにする
                for (int i = 2; i < 4; i++)
                {
                    head_position[i] = baseP[i];
                }
                break;

            case 1://max方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の5,7を使う
                    head_position[i] = head_positiondistance[2 * i + 1];
                }
                break;

            case 2://maxにする
                for (int i = 2; i < 4; i++)
                {
                    head_position[i] = maxP[i];
                }
                break;

            case -1://min方向に1つ増やす
                for (int i = 2; i < 4; i++)
                {
                    //線形補間の4,6を使う
                    head_position[i] = head_positiondistance[2 * i];
                }
                break;

            case -2://minにする
                for (int i = 2; i < 4; i++)
                {
                    head_position[i] = head_positiondistance[2 * i];
                }
                break;

            default:
                Debug.Log("parameterNumが正常入力でない");
                break;

        }
        
        //首の高さにそろえる
        for (int i = 2; i < 4; i++)
        {
            head_position[i] = head_position[i] + neckY;
        }
        
    }




    // Start is called before the first frame update
    void Start()
    {
        //データのスクリプトを取得
        dbd = GameObject.Find("Doragon").GetComponent<DragonBorneData>();
        headParameterUI = GameObject.Find("headPanel").GetComponent<HeadParameterUI>();
        bodyParameter = GameObject.Find("bodyPanel").GetComponent<BodyParameter>();

        //頭部に関係するデータを取得し線形補間のデータを作る
        for (int i = 0; i < 4; i++)
        {
            
            //座標の最大と最小の幅を計算
            int j = 2 * i;
            head_positiondistance[j] = (dbd.nomalposition[i] + dbd.minposition[i]) * 0.5f;
            head_positiondistance[j + 1] = (dbd.maxposition[i] + dbd.nomalposition[i]) * 0.5f;

            //tensionとvelocityの最大と最小の幅を計算
            head_tension_abs[j] = (dbd.nomaltension[i] + dbd.mintension[i]) * 0.5f;
            head_tension_abs[j + 1] = (dbd.nomaltension[i] + dbd.maxtension[i]) * 0.5f;
            head_direction_abs[j] = (dbd.nomaldirection[i] + dbd.mindirection[i]) * 0.5f;
            head_direction_abs[j + 1] = (dbd.nomaldirection[i] + dbd.maxdirection[i]) * 0.5f;
            
        }

        //パラメータの確認を初期化
        check_forehead_height = 0;
        check_jaw_tension = 0;
        check_upper_mouse = 0;
        check_under_mouse = 0;
        check_neck = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        //パラメータが変化したか確認
        check_forehead_height = CheckChengeParameter(check_forehead_height, headParameterUI.forehead_height_throwValue);
        check_jaw_tension = CheckChengeParameter(check_jaw_tension, headParameterUI.jaw_tension_throwValue);
        check_upper_mouse = CheckChengeParameter(check_upper_mouse, headParameterUI.upper_mouse_throwValue);
        check_under_mouse = CheckChengeParameter(check_under_mouse, headParameterUI.under_mouse_throwValue);

        //首の高さを取得
        check_neck = bodyParameter.body_position[0].y - dbd.nomalposition[4].y;

        //額の張りを変える
        change_forehead_height(dbd.nomaltension, dbd.nomaldirection, dbd.maxtension, dbd.maxdirection,
            dbd.mintension, dbd.mindirection, check_forehead_height);

        //顎の張りを変える
        change_jaw_tension(dbd.nomaltension, dbd.nomaldirection, dbd.maxtension, dbd.maxdirection,
            dbd.mintension, dbd.mindirection, check_jaw_tension);
        
        //上口の長さを変える
        change_upper_mouse(dbd.nomalposition, dbd.maxposition, dbd.minposition, check_upper_mouse, check_neck);

        //下口の長さを変える
        change_under_mouse(dbd.nomalposition, dbd.maxposition, dbd.minposition, check_under_mouse, check_neck);
    }
}
