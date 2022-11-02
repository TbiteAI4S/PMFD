using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingBorneData : MonoBehaviour
{
    /* ベース */
    public Vector3[] wing_nomalposition =
    {
        //翼の上腕
        new Vector3(-1.2f,-0.2f,0.0f),
        new Vector3(-0.2f,2.9f,0.0f),
        new Vector3(-1.4f,5.2f,0.0f),

    };

    public float[] wing_nomaltension =
    {   /* 始点, 終点 */

        //翼の上腕
        1.0f,1.0f,
        1.0f,1.0f,
    };

    public float[] wing_nomaldirection =
    {   /* 始点, 終点 */

        //翼の上腕
        0.0f,0.0f,
        0.0f,0.0f,
    };

    /* 最小値 */
    public Vector3[] wing_minposition =
    {
        //翼の上腕
        new Vector3(-1.2f,-0.2f,0.0f),
        new Vector3(-0.2f,2.9f,0.0f),
        new Vector3(-1.4f,5.2f,0.0f),

    };

    public float[] wing_minltension =
    {   /* 始点, 終点 */

        //翼の上腕
        1.0f,1.0f,
        1.0f,1.0f,
    };

    public float[] wing_mindirection =
    {   /* 始点, 終点 */

        //翼の上腕
        0.0f,0.0f,
        0.0f,0.0f,
    };

    /* 最大値 */
    public Vector3[] wing_maxposition =
    {
        //翼の上腕
        new Vector3(-1.2f,-0.2f,0.0f),
        new Vector3(-0.2f,2.9f,0.0f),
        new Vector3(-1.4f,5.2f,0.0f),

    };

    public float[] wing_maxtension =
    {   /* 始点, 終点 */

        //翼の上腕
        1.0f,1.0f,
        1.0f,1.0f,
    };

    public float[] wing_maxdirection =
    {   /* 始点, 終点 */

        //翼の上腕
        0.0f,0.0f,
        0.0f,0.0f,
    };
}
