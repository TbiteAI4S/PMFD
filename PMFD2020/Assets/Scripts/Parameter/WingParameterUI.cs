using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WingParameterUI : MonoBehaviour
{
    //スライダー
    Slider wingunderarm_long;
    Slider wingunderarm_size;
    Slider wingforearm_long;
    Slider wingforearm_size;
    Slider wing_number;

    //渡す値
    public int wingunderarm_long_throwValue = 0;
    public int wingunderarm_size_throwValue = 0;
    public int wingforearm_long_throwValue = 0;
    public int wingforearm_size_throwValue = 0;
    public int wing_numberSlider_throwValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        wingunderarm_long = GameObject.Find("wingunderarm_longSlider").GetComponent<Slider>();
        wingunderarm_size = GameObject.Find("wingunderarm_sizeSlider").GetComponent<Slider>();
        wingforearm_long = GameObject.Find("wingforearm_longSlider").GetComponent<Slider>();
        wingforearm_size = GameObject.Find("wingforearm_sizeSlider").GetComponent<Slider>();
        wing_number = GameObject.Find("wing_numberSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        wingunderarm_long_throwValue = (int)wingunderarm_long.value;
        wingunderarm_size_throwValue = (int)wingunderarm_size.value;
        wingforearm_long_throwValue = (int)wingforearm_long.value;
        wingforearm_size_throwValue = (int)wingforearm_size.value;
        wing_numberSlider_throwValue = (int)wing_number.value;
    }
}
