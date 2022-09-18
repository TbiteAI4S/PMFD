using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBallsSample : MonoBehaviour
{
    public Material CurrentMaterial
    {
        get//マテリアルを取得
        {
            return mat;
        }
        set//マテリアルを設定
        {
            if (value == mat)
                return;
            mat = pSystem.GetComponent<ParticleSystemRenderer>().sharedMaterial = value;
        }
    }

    private Material mat;                       //マテリアル
    private ParticleSystem pSystem;             //パーティクル本体
    private ParticleSystem.Particle[] particles;//各パーティクル
    private List<Vector4> particlesPos;         //各パーティクルの座標
    private float speed = 0.0f;                 //スピード

    // Use this for initialization
    void Awake()
    {
        //パーティクルシステムを取得
        pSystem = GetComponent<ParticleSystem>();
        //
        particles = new ParticleSystem.Particle[pSystem.main.maxParticles];
        //particlesPosは10個に設定
        particlesPos = new List<Vector4>(10);
        //パーティクルのマテリアルを得る
        mat = pSystem.GetComponent<ParticleSystemRenderer>().sharedMaterial;
        //パーティクルのスピードを得る
        speed = pSystem.main.startSpeedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        //パーティクルの座標を初期化
        particlesPos.Clear();

        //現在，存在するパーティクルのリストを取得
        int aliveParticles = pSystem.GetParticles(particles);

        //今，存在するすべてのパーティクルの位置を取得
        for (int i = 0; i < aliveParticles; i++)
        {
            particlesPos.Add(particles[i].position);
        }

        // シェーダーの位置を更新
        mat.SetVectorArray("_ParticlesPos", particlesPos);

    }

    /*
    public bool TogglePlay()
    {
        if (pSystem.isPlaying)
            pSystem.Pause();
        else
            pSystem.Play();

        return pSystem.isPlaying;
    }

    public void TogglePlay(bool b)
    {
        if (b)
            pSystem.Play();
        else
            pSystem.Pause();
    }

    public bool ToggleNoise()
    {
        ParticleSystem.NoiseModule n = pSystem.noise;
        if (n.enabled)
            n.enabled = false;
        else
            n.enabled = true;
        return n.enabled;
    }

    public void ToggleNoise(bool b)
    {
        ParticleSystem.NoiseModule n = pSystem.noise;
        n.enabled = b;
    }

    public void ChangeSpeed(float newSpeed)
    {
        ParticleSystem.MainModule pmain = pSystem.main;
        pmain.startSpeedMultiplier = speed * newSpeed;
    }
    */
}
