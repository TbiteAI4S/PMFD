using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LegRightBorn : MonoBehaviour
{
    MakeDragonBone mbd;

    LineRenderer lineRenderer;

    int bornNum = 2;

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

        Vector3[] dbone = mbd.right_arm_leg_dragonbone[bornNum].Concat(mbd.right_arm_leg_dragonbone[bornNum + 1]).ToArray();
        //Vector3[] bone = dbone.Concat(mbd.right_arm_leg_dragonbone[bornNum + 2]).ToArray();

        // 点の数を指定する
        lineRenderer.positionCount = mbd.right_arm_leg_dragonbone[bornNum].Length + 
            mbd.right_arm_leg_dragonbone[bornNum + 1].Length/* + mbd.right_arm_leg_dragonbone[bornNum + 2].Length*/;
        // 線を引く場所を指定する
        //lineRenderer.SetPositions(bone);
        lineRenderer.SetPositions(dbone);
    }
}
