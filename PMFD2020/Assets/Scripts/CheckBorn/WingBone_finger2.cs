using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingBone_finger2 : MonoBehaviour
{
    MakeDragonBone mbd;

    LineRenderer lineRenderer;

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

        //下腕と上腕の結合
        Vector3[] wing = new Vector3[mbd.wing_dragonbone[4].Length + mbd.wing_dragonbone[5].Length];
        Array.Copy(mbd.wing_dragonbone[4], wing, mbd.wing_dragonbone[4].Length);
        Array.Copy(mbd.wing_dragonbone[5], 0, wing, mbd.wing_dragonbone[4].Length, mbd.wing_dragonbone[5].Length);

        // 点の数を指定する
        lineRenderer.positionCount = wing.Length;
        // 線を引く場所を指定する
        lineRenderer.SetPositions(wing);
    }
}
