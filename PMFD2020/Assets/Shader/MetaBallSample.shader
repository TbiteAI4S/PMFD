// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MetaBall/MetaBallSample"
{
    Properties
    {
        /*---Parameter---*/
        // Diffuse
        _Color("Tint", Color) = (0.2, 0.3, 0.4, 1)
        _DiffusePower("Diffuse Power", Float) = 3
        _DiffuseLevels("DiffuseLevels", int) = 3
        // Specular
        _SpecColor("Specular", Color) = (0.4, 0.5, 0.6, 1)
        _SpecPower("Specular Power", Float) = 3
        _SpecAtten("Specular Attenuation", Range(0,1)) = 1
        _SpecThreshold("Specular Threshold", Range(0,1)) = 0.4
        // Edge
        _EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
        _EdgeThreshold("Edge Threshold", Range(0,1)) = 0.2
        // Radial Basis Function
        _K("RadialBasis Constant", Float) = 7
        _Threshold ("Isosurface Threshold", Range(0,1)) = 0.5
        _Epsilon("Normal Epsilon", Range(0,1)) = 0.1
    }
    SubShader
    {
        /*---ShaderSetting---*/
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        /*
        * Blend Off: ブレンドを無効にします (これがデフォルト)
        * Blend SrcFactor DstFactor : ブレンディングを設定し使用可能にします。生成されたカラーは SrcFactor と乗算します。画面にすでにあるカラーは DstFactor と乗算し、2 つは合成されます。
        * Blend SrcFactor DstFactor、SrcFactorA DstFactorA : 上記と同様ですが、アルファチャンネルをブレンディングするのに、異なる要素を使用します。
        * BlendOp Op: ブレンドしたカラーを合わせる代わりに、異なる操作を行います。
        * BlendOp OpColor, OpAlpha: 上記と同様ですが、カラー (RGB) とアルファ (A) チャンネルに対し異なるブレンド操作を使用します。
        */
        Blend SrcAlpha OneMinusSrcAlpha
        /*カリング 
        * Back( 法線が裏側を向いているポリゴンを描画しない) 
        * Front(法線が表側を向いているポリゴンを描画しない)
        * Off(カリングを無効にしてすべての面を描画)
        */
        Cull Off
        ZWrite Off 			// Don't write on the ZBuffer.
        Lighting Off		// Don't let unity do lighting calculations.

        Pass
        {
            /*Passの名前*/
            Name "MetaBallSamples"


            CGPROGRAM         

            /*バーテックスシェーダー*/
            #pragma vertex vert
            /*フラグメントシェーダー*/
            #pragma fragment frag

            #include "UnityCG.cginc"

            /*頂点データへの入力定義*/
            struct appdata
            {
                //頂点の座標
                float4 vertex : POSITION;
            };

            /*フラグメントシェーダーへ渡すデータ*/
            struct v2f
            {
                //座標変換された後の頂点座標
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD0;
            };
            

            /*
            * メタボール
            * 参考：https://butadiene.hatenablog.com/entry/2019/02/23/075535#%E5%BD%B1%E3%82%92%E3%81%A4%E3%81%91%E3%82%8B
            * 参考：https://baba-s.hatenablog.com/entry/2018/04/10/094100
            */

            uniform float3 _ParticlesPos[10];   //メタボールの座標

            float4 _Color;						// Diffuse Color.
            float  _DiffusePower;				// Diffuse Power.
            int _DiffuseLevels;

            float4 _SpecColor;					// Specular Color.
            float _SpecPower;					// Specular Power.
            fixed _SpecAtten;					// Specular Scale.
            fixed _SpecThreshold;				// Specular threshold.

            float4 _EdgeColor;					// Edge Color.
            fixed _EdgeThreshold;				// Edge threshold.

            float _K;                            // Radial basis function constant.
            fixed _Threshold;                   //メタボールの半径
            fixed _Epsilon;                     //正規分布

            
            ///*
            //* 溶けるように滑らかに接続
            //* d1:距離 , d2:距離 , k:係数
            //*/
            //float SmoothConnection(float d1,float d2,float k)
            //{
            //    return -log(exp(-k * d1) + exp(-k * d2)) / k;
            //}

            ///*
            //* メタボースのサイズを取得
            //* p:ボールの座標 , s:球の半径
            //*/
            //float Ball(float3 p, float s)
            //{
            //    return length(p) - s;
            //}   

            ///*
            //* ボールの位置と半径の取得
            //* i:ボールの番号
            //*/
            //float4 MetaballValue(float i)
            //{
            //    //ボールの位置
            //    float3 ballpos = _ParticlesPos[i].xyz;
            //    //ボールの半径
            //    float scale = _Threshold;

            //    return  float4(ballpos,scale);
            //}
            //
            ///*
            //* MetaballValueを反映した距離関数
            //* p:座標, i:ボールの番号
            //*/
            //float MetaballOne(float3 p, float i)
            //{
            //    //MetaballValueを取得
            //    float4 value = MetaballValue(i);
            //    //メタボールの場所を計算
            //    float3 ballpos = p - value.xyz;
            //    //メタボールのサイズ
            //    float scale = value.w;

            //    return  Ball(ballpos,scale);
            //}

            ///*
            //* メタボール(群)の距離関数
            //* p:座標
            //*/
            //float Metaball(float3 p)
            //{
            //    //ボールとの距離
            //    float d1;
            //    //1ループ前に計算したボールとの距離
            //    float d2 = MetaballOne(p,0);
            //    //6つのボールに対して計算
            //    for (int i = 1; i < 6; ++i) 
            //    {
            //        d1 = MetaballOne(p,i);
            //        //d1に滑らかに接続する値を格納
            //        d1 = SmoothConnection(d1,d2,20);
            //        //d2を更新
            //        d2 = d1;
            //    }
            //    return d1;
            //}

            /*計算*/
            float scalarField(float3 pos) {
            float density = 0;
            for (int i = 0; i < 10; i++) {
                float dis = distance(pos, _ParticlesPos[i].xyz);
                density += exp(-_K * (dis));
            }
            return density;
            }
            
            float scalarField(float x, float y, float z) {
                float3 pos = float3(x, y, z);
                return scalarField(pos);
            }

            /*法線を算出*/
            float3 calcNormal(float3 p) {
                float3 N;
                N.x = scalarField(p.x + _Epsilon, p.y, p.z) - scalarField(p.x - _Epsilon, p.y, p.z);
                N.y = scalarField(p.x, p.y + _Epsilon, p.z) - scalarField(p.x, p.y - _Epsilon, p.z);
                N.z = scalarField(p.x, p.y, p.z + _Epsilon) - scalarField(p.x, p.y, p.z - _Epsilon);
                return -N;
            }

            //陰影付け
            float3 blinnPhongToon(float3 V, float3 N, float3 L) {
                float3 H = normalize(V + L);
                float NdotH = saturate(dot(N, H)); // for specular.
                float NdotV = saturate(dot(N, V)); // for fresnel.
                float LdotN = saturate(dot(L, N)); // for diffuse.

                float3 diffuseTerm = _Color * floor(LdotN * _DiffuseLevels) / _DiffuseLevels;
                float spec = pow(NdotH, _SpecPower);
                float3 specularTerm = spec * _SpecAtten * _SpecColor * (spec > _SpecThreshold ? 1 : 0);
                float3 edge = NdotV > _EdgeThreshold ? 1 : _EdgeColor;
                return edge * (_Color + diffuseTerm + (specularTerm));
            }

            /*vert関数へ頂点情報を送る*/
            v2f vert(appdata v)
            {
                v2f o;
                //視点方向のベクトルを求める(ワールド座標系)
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                //モデルスペースからクリップスペースへの変換
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                //フラグメントの色の初期化
                float4 col = _Color;
                col.a = 0;

                // レイキャストの準備
                float3 viewDir = normalize(i.worldPosition - _WorldSpaceCameraPos.xyz);//カメラからフラグメントへのワールドスペース方向
                float3 start = i.worldPosition - 2.0 * viewDir; //viewDirに沿ったフラグメントの2ユニット手前からサンプリングをはじめる
                float3 p; //サンプリングされた点

                // レイのサンプリングを開始 レイに沿って 0.2 ポイントずつ最大 3 単位の長さでサンプリング
                // 等値面との交点を見つけたら、交点を正確に特定できるようになるまで、さらに小さなステップでサンプリング
                for (float i = 0; i < 3.0; i += 0.2) {
                    p = start + i * viewDir;

                    if (scalarField(p) > _Threshold) {
                        // now, sample each 0.05 from (p - 0.2, p + 0.2)
                        float3 start2 = p - 0.2 * viewDir;
                        for (float i = 0.05; i < 0.4; i += 0.05) {
                            p = start2 + i * viewDir;

                            if (scalarField(p) > _Threshold) {
                                // finally, sample each 0.01 from (p - 0.05, p + 0.05)
                                float3 start3 = p - 0.05 * viewDir;
                                for (float i = 0.01; i < 0.1; i += 0.01) {
                                    p = start3 + i * viewDir;
                                    if (scalarField(p) > _Threshold) {
                                        break;
                                    }
                                }
                                break;
                            }
                        }

                        // ... calculate its normal.
                        float3 N = normalize(calcNormal(p));
                        // ... calculate the lightDir.
                        float3 L = normalize(UnityWorldSpaceLightDir(p.xyz)); // the same as normalize(_WorldSpaceLightPos0 - p.xyz);
                        // ... illuminate with blinnPhong
                        col.xyz = blinnPhongToon(-viewDir, N, L);
                        // ... make it visible.
                        col.a = 1;
                        break;
                    }
                }

                return col;
            }

            ENDCG
        }        
    }
    FallBack "Diffuse"
}
