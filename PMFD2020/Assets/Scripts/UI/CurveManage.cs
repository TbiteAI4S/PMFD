using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurveManage : MonoBehaviour
{
    /*---スライダーのスクリプト---*/
    StartPositionXvalue spx;
    StartPositionYvalue spy;
    EndPositionXvalue epx;
    EndPositionYvalue epy;
    StartTension st;
    EndTension et;
    StartDirection sd;
    EndDirection ed;

    /*---スライダーの値---*/
    //曲線の端点の位置
    public Vector3[] nowPostion = {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 0.0f)
    };

    //曲線の端点の張り
    public float[] nowtension = { 1.0f, 1.0f };
    public float[] nowdirection = { 0.0f, 0.0f };

    //スライダーの値をテキストに反映する
    public void ChangeSliderText(Slider slider, Text curveText, float sliderValue)
    {
        //スライダーの値を取得
        sliderValue = slider.value;

        //スライダーの値をテキストに反映
        curveText.text = "値：" + sliderValue;
    }

    //変更を適用
    public float CheckSlider(Slider slider, float throwValue)
    {
        float nowValue = slider.value;
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
        spx = GameObject.Find("StartPositionXSlider").GetComponent<StartPositionXvalue>();
        spy = GameObject.Find("StartPositionYSlider").GetComponent<StartPositionYvalue>();
        epx = GameObject.Find("EndPositionXSlider").GetComponent<EndPositionXvalue>();
        epy = GameObject.Find("EndPositionYSlider").GetComponent<EndPositionYvalue>();
        st = GameObject.Find("StartTensionSlider").GetComponent<StartTension>();
        et = GameObject.Find("EndTensionSlider").GetComponent<EndTension>();
        sd = GameObject.Find("StartDirectionSlider").GetComponent<StartDirection>();
        ed = GameObject.Find("EndDirectionSlider").GetComponent<EndDirection>();

        //各スライダーからの値を格納
        //座標
        nowPostion[0][0] = spx.throwValue;
        nowPostion[0][1] = spy.throwValue;
        nowPostion[1][0] = epx.throwValue;
        nowPostion[1][1] = epy.throwValue;

        //張り
        nowtension[0] = st.throwValue;
        nowtension[1] = et.throwValue;

        //方向
        nowdirection[0] = sd.throwValue;
        nowdirection[1] = ed.throwValue;
    }

    // Update is called once per frame
    void Update()
    {
        //各スライダーの値を更新
        nowPostion[0][0] = spx.throwValue;
        nowPostion[0][1] = spy.throwValue;
        nowPostion[1][0] = epx.throwValue;
        nowPostion[1][1] = epy.throwValue;

        nowtension[0] = st.throwValue;        
        nowtension[1] = et.throwValue;

        nowdirection[0] = sd.throwValue;
        nowdirection[1] = ed.throwValue;
    }    
}
