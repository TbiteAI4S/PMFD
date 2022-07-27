using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * ***********************************************
 *
 * マウスによる視点移動
 *
 * ***********************************************
 */

public class MouseController : MonoBehaviour
{
    [SerializeField]
    private Transform pivot;    //キャラクターの中心にある空のオブジェクトを選択してください

    void Start()
    {
        //エラーが起きないようにNullだった場合、それぞれ設定
        if (pivot == null)
            pivot = transform;
    }
    // Update is called once per frame
    void Update()
    {
        //マウスの右が押されている間
        if (Input.GetMouseButton(1))
        {
            //マウスのX,Y軸がどれほど移動したかを取得します
            float X_Rotation = Input.GetAxis("Mouse X");
            float Y_Rotation = Input.GetAxis("Mouse Y");
            //Y軸を更新します（キャラクターを回転）取得したX軸の変更をキャラクターのY軸に反映します
            pivot.transform.Rotate(0, X_Rotation, 0);

            //次はY軸の設定です。
            float nowAngle = pivot.transform.localRotation.x;
            //最大値、または最小値を超えた場合、カメラをそれ以上動かない用にしています。
            //キャラクターの中身が見えたり、カメラが一回転しないようにするのを防ぎます。
            if (-Y_Rotation != 0)
            {
                if (0 < Y_Rotation)
                {
                    if (-0.5 <= nowAngle)
                    {
                        pivot.transform.Rotate(Y_Rotation, 0, 0);
                    }
                }
                else
                {
                    if (nowAngle <= 0.5)
                    {
                        pivot.transform.Rotate(Y_Rotation, 0, 0);
                    }
                }
            }
        }

        //操作していると、Z軸がだんだん動いていくので、0に設定してください。
        pivot.eulerAngles = new Vector3(pivot.eulerAngles.x, pivot.eulerAngles.y, 0f);
    }
}
