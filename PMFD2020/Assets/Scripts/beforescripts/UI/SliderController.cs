using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    /*---スライダー---*/
    Slider thisSlider;

    /*---テキスト---*/
    //制御点
    public Text sliderTexts;

    //スライダーの値をテキストに反映する
    private void ChangeSliderText()
    {
        //スライダーの値
        float sliderValue = thisSlider.value;
    }

    // Start is called before the first frame update
    void Start()
    {
        thisSlider = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
