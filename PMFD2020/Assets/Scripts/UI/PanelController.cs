using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    //panel
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;



    void Start()
    {
        //初めはすべてのパネルが見えないようにする
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
    }

    /*---ボタンを押したら目的のパネルを表示し他のパネルは消す*/
    public void BodyView()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);
    }
    public void TailView()
    {
        panel1.SetActive(false);
        panel2.SetActive(true);
        panel3.SetActive(false);
    }
    public void HeadView()
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(true);
    }
}
