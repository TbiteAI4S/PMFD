using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootParameterUI : MonoBehaviour
{
    //スライダー
    Slider thigh_long;
    Slider thigh_size;
    Slider lower_leg_long;
    Slider lower_leg_size;
    Slider footnail_size;

    //渡す値
    public int thigh_long_throwValue = 0;
    public int thigh_size_throwValue = 0;
    public int lower_leg_long_throwValue = 0;
    public int lower_leg_size_throwValue = 0;
    public int footnail_size_throwValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        thigh_long = GameObject.Find("thigh_longSlider").GetComponent<Slider>();
        thigh_size = GameObject.Find("thigh_sizeSlider").GetComponent<Slider>();
        lower_leg_long = GameObject.Find("lower_leg_longSlider").GetComponent<Slider>();
        lower_leg_size = GameObject.Find("lower_leg_sizeSlider").GetComponent<Slider>();
        footnail_size = GameObject.Find("footnail_sizeSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        thigh_long_throwValue = (int)thigh_long.value;
        thigh_size_throwValue = (int)thigh_size.value;
        lower_leg_long_throwValue = (int)lower_leg_long.value;
        lower_leg_size_throwValue = (int)lower_leg_size.value;
        footnail_size_throwValue = (int)footnail_size.value;
    }
}
