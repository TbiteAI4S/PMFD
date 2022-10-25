using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadParameterUI : MonoBehaviour
{
    //スライダー
    Slider forehead_height;
    Slider jaw_tension;
    Slider upper_mouse;
    Slider under_mouse;

    //渡す値
    public int forehead_height_throwValue = 0;
    public int jaw_tension_throwValue = 0;
    public int upper_mouse_throwValue = 0;
    public int under_mouse_throwValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        //スライダーを探して取得
        forehead_height = GameObject.Find("forehead_heightSlider").GetComponent<Slider>(); 
        jaw_tension = GameObject.Find("jaw_tensionSlider").GetComponent<Slider>();
        upper_mouse = GameObject.Find("upper_mouseSlider").GetComponent<Slider>();
        under_mouse = GameObject.Find("under_mouseSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        forehead_height_throwValue = (int)forehead_height.value;
        jaw_tension_throwValue = (int)jaw_tension.value;
        upper_mouse_throwValue = (int)upper_mouse.value;
        under_mouse_throwValue = (int)under_mouse.value;
    }
}
