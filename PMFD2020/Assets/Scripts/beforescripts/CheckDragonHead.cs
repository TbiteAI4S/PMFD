using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDragonHead : MonoBehaviour
{
    //ドラゴンのデータ
    MakeDragonBone mdb;

    //headのデータ配列
    Vector3[] head;

    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //データ取得
        mdb = GameObject.Find("Doragon").GetComponent<MakeDragonBone>();

        head = mdb.dragonbone[0];
        //LineRendererコンポーネントをゲームオブジェクトにアタッチする
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        // 線を引く場所を指定する
        lineRenderer.positionCount = head.Length;
        lineRenderer.SetPositions(head);
    }

    // Update is called once per frame
    void Update()
    {
        head = mdb.dragonbone[0];
        lineRenderer.positionCount = head.Length;
        lineRenderer.SetPositions(head);

    }
}
