using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WingParameterUI : MonoBehaviour
{
    //スライダー
    Slider wingupperarm_long;
    Slider wingupperarm_size;
    Slider wingforearm_long;
    Slider wingforearm_size;
    Slider wing_number;

    //渡す値
    public int wingupperarm_long_throwValue = 0;
    public int wingupperarm_size_throwValue = 0;
    public int wingforearm_long_throwValue = 0;
    public int wingforearm_size_throwValue = 0;
    public int wing_numberSlider_throwValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        wingupperarm_long = GameObject.Find("wingupperarm_longSlider").GetComponent<Slider>();
        wingupperarm_size = GameObject.Find("wingupperarm_sizeSlider").GetComponent<Slider>();
        wingforearm_long = GameObject.Find("wingforearm_longSlider").GetComponent<Slider>();
        wingforearm_size = GameObject.Find("wingforearm_sizeSlider").GetComponent<Slider>();
        wing_number = GameObject.Find("footnail_sizeSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        wingupperarm_long_throwValue = (int)wingupperarm_long.value;
        wingupperarm_size_throwValue = (int)wingupperarm_size.value;
        wingforearm_long_throwValue = (int)wingforearm_long.value;
        wingforearm_size_throwValue = (int)wingforearm_size.value;
        wing_numberSlider_throwValue = (int)wing_number.value;
    }
}
