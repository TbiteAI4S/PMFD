using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArmBorn : MonoBehaviour
{
    MakeDragonBone mbd;

    LineRenderer lineRenderer;

    int bornNum = 5;

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

        Vector3[] dbone = mbd.dragonbone[bornNum].Concat(mbd.dragonbone[bornNum + 1]).ToArray();

        // 点の数を指定する
        lineRenderer.positionCount = mbd.dragonbone[bornNum].Length + mbd.dragonbone[bornNum + 1].Length;
        // 線を引く場所を指定する
        lineRenderer.SetPositions(dbone);
    }
}
