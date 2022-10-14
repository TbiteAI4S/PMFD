using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDragonBoneDetail : MonoBehaviour
{
    CurveManage cm;
    Curve curvescript;
    MakeDragonBone makeDragonBone;
    /*----ディテールのドラゴンボーンのデータ----*/
    Vector3[] detailposition =
    {
        /* 頭部 */
        //角

        //目

        /* 胴体 */
        //背びれ

        /* 尾 */
        //背びれの延長

        /* 左腕 */

    };

    // Start is called before the first frame update
    void Start()
    {
        //CueveManagerを取得
        cm = GameObject.Find("SliderManager").GetComponent<CurveManage>();

        //curveのスクリプト取得
        curvescript = GameObject.Find("Curve").GetComponent<Curve>();

        //MakeDragonBoneの取得
        makeDragonBone = GameObject.Find("Doragon").GetComponent<MakeDragonBone>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
