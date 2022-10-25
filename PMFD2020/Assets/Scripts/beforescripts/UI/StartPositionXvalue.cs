using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPositionXvalue : MonoBehaviour
{
    /*---スライダーの関数---*/
    CurveManage cm;


    /*---スライダー---*/
    Slider curveSlider;

    /*---テキスト---*/
    public Text curveTexts;

    /*---スライダーの値---*/
    //現在の値
    float sliderValue;
    //渡す値
    public float throwValue;

    //スライダーの値をテキストに反映する
    private void ChangeSliderText()
    {
        //スライダーの値を取得
        sliderValue = curveSlider.value;

        //スライダーの値をテキストに反映
        curveTexts.text = "値：" + sliderValue;
    }

    //変更を適用
    private float CheckSlider()
    {
        float nowValue = curveSlider.value;
        //値が異なれば変更
        if (throwValue < nowValue || throwValue > nowValue)
        {
            //更新
            throwValue = nowValue;
        }

        return throwValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        cm = GameObject.Find("SliderManager").GetComponent<CurveManage>();

        curveSlider = this.GetComponent<Slider>();
        //スライダーの値を設定
        float valueMax = 5.0f;
        float valueMin = -5.0f;

        curveSlider.maxValue = valueMax;
        curveSlider.minValue = valueMin;
        curveSlider.value = 0.0f;

        //値を渡す
        throwValue = curveSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        cm.ChangeSliderText(curveSlider, curveTexts, sliderValue);
        throwValue = cm.CheckSlider(curveSlider, throwValue);
        //ChangeSliderText();
        //CheckSlider();
        //Debug.Log("throwValue"+ throwValue);
    }
}
