using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmParameterUI : MonoBehaviour
{
    //スライダー
    Slider upperarm_long;
    Slider upperarm_size;
    Slider forearm_long;
    Slider forearm_size;
    Slider nail_size;

    //渡す値
    public int upperarm_long_throwValue = 0;
    public int upperarm_size_throwValue = 0;
    public int forearm_long_throwValue = 0;
    public int forearm_size_throwValue = 0;
    public int nail_size_throwValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        upperarm_long = GameObject.Find("upperarm_longSlider").GetComponent<Slider>();
        upperarm_size = GameObject.Find("upperarm_sizeSlider").GetComponent<Slider>();
        forearm_long = GameObject.Find("forearm_longSlider").GetComponent<Slider>();
        forearm_size = GameObject.Find("forearm_sizeSlider").GetComponent<Slider>();
        nail_size = GameObject.Find("nail_sizeSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        upperarm_long_throwValue = (int)upperarm_long.value;
        upperarm_size_throwValue = (int)upperarm_size.value;
        forearm_long_throwValue = (int)forearm_long.value;
        forearm_size_throwValue = (int)forearm_size.value;
        nail_size_throwValue = (int)nail_size.value;
    }
}
