using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneDataToShader : MonoBehaviour
{
    /// <summary>
    /// シェーダーに座標を渡す
    /// </summary>
    /// 

    [SerializeField] private Renderer _renderer;

    //ドラゴンボーンのスクリプト
    MakeDragonBone mdb;

    //Shader側でていぎずみの座標を受け取る変数
    private string propName = "_MPosition";

    //マテリアル
    private Material mat;

    //ドラゴンボーンの座標配列
    Vector3 m_position;

    float[] sin = {
        Mathf.Sin(0),
        Mathf.Sin(30),
        Mathf.Sin(60),
        Mathf.Sin(90),
        Mathf.Sin(60),
        Mathf.Sin(30)
    };



    // Start is called before the first frame update
    void Start()
    {

        //マテリアルを取得
        mat = _renderer.material;
                
        m_position = new Vector3(-1.0f, 0, 0);

        
        //マテリアルに座標を渡す
        mat.SetVector(propName, m_position);
        for (int i = 0; i < 6; i++)
        {
            mat.SetFloat("_Sin[" + i + "]", sin[i]);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        //mat.SetVector(propName, m_position);
    }
}
