using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadParameter : MonoBehaviour
{
    DragonBorneData dbd;

    //頭に関するドラゴンボーンのデータ
    Vector3[] head_baseposition = new Vector3[4];
    float[] head_basetension = new float[4];
    float[] head_basevelocity = new float[4];

    Vector3[] head_minposition = new Vector3[4];
    float[] head_mintension = new float[4];
    float[] head_minvelocity = new float[4];

    Vector3[] head_maxposition = new Vector3[4];
    float[] head_maxtension = new float[4];
    float[] head_maxvelocity = new float[4];

    /* 変形するパラメータ */
    //額の高さの変更
    void change_forehead_height()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //データのスクリプトを取得
        DragonBorneData dbd = GameObject.Find("Doragon").GetComponent<DragonBorneData>();

        //頭部に関係するデータを取得
        for (int i = 0; i < 4; i++)
        {
            head_baseposition[i] = dbd.baseposition[i];
            
            head_minposition[i] = dbd.minposition[i];

            head_maxposition[i] = dbd.maxposition[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
