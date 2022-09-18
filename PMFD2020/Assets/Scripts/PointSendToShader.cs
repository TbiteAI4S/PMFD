using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PointSendToShader : MonoBehaviour
{
    /// <summary>
    /// シェーダーに座標を渡すサンプル
    /// </summary>
    /// 

    [SerializeField] private Renderer _renderer;

    //Shader側でていぎずみの座標を受け取る変数
    private string propName = "_MPosition";

    private Material mat;

    Vector3 m_position;

    Vector3 noise;

    int a = 0;

    bool b = false;

    // Start is called before the first frame update
    void Start()
    {
        mat = _renderer.material;

        m_position = new Vector3(-5.0f, 0, 0);
        mat.SetVector(propName, m_position);

        noise = new Vector3(0.01f, 0.01f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        a += 1;
        if(a/5 == 0)
        {
            b = true;
        }else
        {
            b = false;
        }

        if (b == true)
        {
            noise = -1.0f * noise;
            //Debug.Log("change");
        }
        
        //m_position = m_position + noise;
        //mat.SetVector(propName, m_position);
    }
}
