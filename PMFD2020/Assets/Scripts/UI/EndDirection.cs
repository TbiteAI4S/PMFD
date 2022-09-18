using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndDirection : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        cm = GameObject.Find("SliderManager").GetComponent<CurveManage>();

        curveSlider = this.GetComponent<Slider>();
        //スライダーの値を設定
        float valueMax = 1.0f;
        float valueMin = -1.0f;

        curveSlider.maxValue = valueMax;
        curveSlider.minValue = valueMin;

        //値を渡す
        throwValue = curveSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        cm.ChangeSliderText(curveSlider, curveTexts, sliderValue);
        throwValue = cm.CheckSlider(curveSlider, throwValue);
        //Debug.Log("throwValue"+ throwValue);
    }
}
