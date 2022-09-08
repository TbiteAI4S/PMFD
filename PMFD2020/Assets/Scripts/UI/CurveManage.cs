using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurveManage : MonoBehaviour
{/*---モデルを操作するスクリプト---*/
    MakeModel makeModelScript;

    /*---スライダー---*/
    Slider curveSlider;

    /*---テキスト---*/
    public Text curveTexts;

    /*---スライダーの値---*/
    //現在の値
    float sliderValue;
    //渡す値
    float throwValue;

    //スライダーの値をテキストに反映する
    private void ChangeSliderText()
    {
        //スライダーの値を取得
        sliderValue = curveSlider.value;

        //スライダーの値をテキストに反映
        curveTexts.text = "値：" + sliderValue;
    }

    //制御点の増減を確認する
    private void CheckSlider()
    {
        float nowValue = curveSlider.value;
        //値が異なれば変更
        if (throwValue < nowValue || throwValue > nowValue)
        {
            //更新
            throwValue = nowValue;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        makeModelScript = GameObject.Find("MakeModel").GetComponent<MakeModel>();
        curveSlider = this.GetComponent<Slider>();
        throwValue = curveSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSliderText();
        CheckSlider();
    }
}
