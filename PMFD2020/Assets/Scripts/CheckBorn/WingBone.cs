using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingBone : MonoBehaviour
{
    MakeDragonBone mbd;

    LineRenderer lineRenderer;

    int bornNum = 4;

    // Start is called before the first frame update
    void Start()
    {
        mbd = GameObject.Find("Doragon").GetComponent<MakeDragonBone>();

        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

        // 点の数を指定する
        //lineRenderer.positionCount = mbd.dragonbone[bornNum].Length;
        // 線を引く場所を指定する
        //lineRenderer.SetPositions(mbd.dragonbone[bornNum]);
    }
}
