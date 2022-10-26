using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyParameterUI : MonoBehaviour
{
    //スライダー
    Slider neck_height;
    Slider body_long;
    Slider bodyback;
    Slider tail_long;

    //渡す値
    public int neck_height_throwValue = 0;
    public int body_long_throwValue = 0;
    public int bodyback_throwValue = 0;
    public int tail_long_throwValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        //スライダーを探して取得
        neck_height = GameObject.Find("neck_heightSlider").GetComponent<Slider>();
        body_long = GameObject.Find("body_longSlider").GetComponent<Slider>();
        bodyback = GameObject.Find("bodybackSlider").GetComponent<Slider>();
        tail_long = GameObject.Find("tail_longSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        neck_height_throwValue = (int)neck_height.value;
        body_long_throwValue = (int)body_long.value;
        bodyback_throwValue = (int)bodyback.value;
        tail_long_throwValue = (int)tail_long.value;
    }
}
