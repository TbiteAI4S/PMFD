Shader "MetaBall/MetaBallSample2"
{
    Properties
    {
        // Diffuse
        _Color("Tint", Color) = (0.2, 0.3, 0.4, 1)
        _Color2("Tint", Color) = (0.2, 0.3, 0.4, 1)
        _Color3("Tint", Color) = (0.2, 0.3, 0.4, 1)
        _Color4("Tint", Color) = (0.2, 0.3, 0.4, 1)
        _Color5("Tint", Color) = (0.2, 0.3, 0.4, 1)
        _Color6("Tint", Color) = (0.2, 0.3, 0.4, 1)

        //メタボールの数
        _MetaballCount("metaballCount",int) = 6
        
        

        //床の高さ
        _ypos("floor height",float) = -0.25
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
        LOD 100
        Cull Front
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _ypos;
            
            //Diffuse Color.
            float4 _Color;						
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;
            float4 _Color5;
            float4 _Color6;    

            //メタボールの数
            int _MetaballCount;

            //場所
            float4 _MPosition;
            float4 _MPositions[6];//動的配列は使えない，長さを指定して宣言

            
            //Making noise
            float hash(float2 p)
            {
                p = 50.0 * frac(p * 0.3183099 + float2(0.71,0.113));
                return -1.0 + 2.0 * frac(p.x * p.y * (p.x + p.y));
            }

            float noise(in float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(lerp(hash(i + float2(0.0,0.0)),
                                 hash(i + float2(1.0,0.0)), u.x),
                            lerp(hash(i + float2(0.0,1.0)),
                                 hash(i + float2(1.0,1.0)), u.x), u.y);
            }
            ///////////////////////////////////////////////////////////////////////
            /*
            * 溶けるように滑らかに接続
            * d1:距離 , d2:距離 , k:係数
            */
            float smoothMin(float d1,float d2,float k)
            {
                return -log(exp(-k * d1) + exp(-k * d2)) / k;
            }

            // Base distance function
            /*
            * メタボースのサイズを取得
            * p:ボールの座標 , s:球の半径
            */
            float ball(float3 p,float s)
            {
                return length(p) - s;
            }


            // Making ball status
            /*
            * ボールの位置と半径の取得
            * i:ボールの番号
            */
            float4 metaballvalue(float i)
            {
                //時間を利用した係数
                float kt = 3 * _Time.y * (0.1 + 0.01 * i);
                //係数とノイズを利用してボールの座標を決定
                //float3 ballpos = 0.3 * float3(noise(float2(i,i) + kt),noise(float2(i + 10,i * 20) + kt),noise(float2(i * 20,i + 20) + kt));
                //float3 ballpos = 0.1 * float3(0.0f + i, 0.0f + i, 0.0f + i + kt);

                float3 ballpos = 0.1 * float3(_MPosition[0] + i, _MPosition[1] + 0.5 * i, _MPosition[2] + 0.03 * i);

                //係数とノイズを利用してボールのサイズを決定
                float scale = 0.05 + 0.02 * hash(float2(i,i));
                //float scale = 0.01 + 0.01 * hash(float2(i,i));

                return  float4(ballpos,scale);
            }
            // Making ball distance function
            /*
            * MetaballValueを反映した距離関数
            * p:座標, i:ボールの番号
            */
            float metaballone(float3 p, float i)
            {
                //ボールの位置，サイズを決定
                float4 value = metaballvalue(i);
                //ボールの場所を計算
                float3 ballpos = p - value.xyz;
                //ボールのサイズを取得
                float scale = value.w;

                return  ball(ballpos,scale);
            }

            //Making metaballs distance function
            /*
            * メタボール(群)の距離関数
            * p:座標
            */
            float metaball(float3 p)
            {
                //距離1
                float d1;
                //距離2は1ループ前の距離
                //初めはpと0の距離
                float d2 = metaballone(p,0);
                //残り5つのボールの距離関数を算出
                for (int i = 1; i < 6; ++i) 
                {

                    d1 = metaballone(p,i);
                    d1 = smoothMin(d1,d2,20);
                    d2 = d1;
                }
                return d1;
            }

            // Making distance function
            float dist(float3 p)
            {
                //float y = p.y;
                float d1 = metaball(p);
                //float d2 = y - (_ypos); //For floor
                //d1 = smoothMin(d1,d2,20);
                return d1;
            }


            //enhanced sphere tracing  http://erleuchtet.org/~cupe/permanent/enhanced_sphere_tracing.pdf
            /*
            * レイマーチング
            * ro:レイ, rd:方向
            */
            float raymarch(float3 ro,float3 rd)
            {
                //前方の半径
                float previousradius = 0.0;
                //最大距離
                float maxdistance = 3;
                //レイのスタート地点が物体の中にあるか外にあるか
                float outside = dist(ro) < 0 ? -1 : +1;
                //t = 1における画素の半径
                float pixelradius = 0.02;
                //レイの進むスピード
                float omega = 1.2;
                //
                float t = 0.0001;
                //1ループ前に進んだ距離
                float step = 0;
                //pixeltの最小値
                float minpixelt = 999999999;
                //pixelt が一番小さい時のtの値
                float mint = 0;

                float hit = 0.01;

                for (int i = 0; i < 60; ++i) 
                {

                    float radius = outside * dist(ro + rd * t);
                    bool fail = omega > 1 && step > (abs(radius) + abs(previousradius));
                    //stepの値が今進もうとしている距離radiusと１ループ前に進もうとした距離previousradius(omegaがかかっていないためstepと異なる値であることに注意)
                    //の和より大きければ進みすぎなためomegaを1.0にして普通に進む
                    if (fail) 
                    {
                        step -= step * omega;
                        omega = 1.0;
                    }
                    else 
                    {
                        step = omega * radius;
                    }

                    previousradius = radius;
                    float pixelt = radius / t;
                    //最もオブジェクトに近かった点を衝突点の候補とする
                    if (!fail && pixelt < minpixelt) 
                    {
                        minpixelt = pixelt;
                        mint = t;
                    }
                    //最大距離に加えて「スクリーンに対するレイの進む半径の大きさ（すなわちpixelt）」が
                    //ピクセルの大きさより小さいのならばループを終わってもよい
                    if (!fail && pixelt<pixelradius || t>maxdistance)
                    break;
                    t += step;
                }

                //スクリーンに対するレイの進む半径の大きさ（pixelt）」の最小値がピクセルの大きさより大きく、
                //かつpixelt が一番小さい時のtの値mintが一定の値（ここではhit）より大きいときは当たっていない判定
                if ((t > maxdistance || minpixelt > pixelradius) && (mint > hit)) 
                {
                    return -1;
                }
                else//スクリーンに対するレイの進む半径の大きさ（pixelt）」が一番小さかった時のt（すなわちpixelt が一番小さい時のtの値mint）を距離とする    
                {
                    return mint;
                }

            }

            // The MIT License
            // Copyright ? 2013 Inigo Quilez
            // Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
            // https://www.shadertoy.com/view/Xds3zN

            //Tetrahedron technique  http://iquilezles.org/www/articles/normalsSDF/normalsSDF.htm
            float3 getnormal(float3 p)
            {
                static const float2 e = float2(0.5773,-0.5773) * 0.0001;
                float3 nor = normalize(e.xyy * dist(p + e.xyy) + e.yyx * dist(p + e.yyx) + e.yxy * dist(p + e.yxy) + e.xxx * dist(p + e.xxx));
                nor = normalize(float3(nor));
                return nor;
            }
            ////////////////////////////////////////////////////////////////////////////

            // Making shadow
            /*陰影付け*/
            float softray(float3 ro, float3 rd , float hn)
            {
                float t = 0.000001;
                float jt = 0.0;
                float res = 1;
                for (int i = 0; i < 20; ++i) 
                {
                    jt = dist(ro + rd * t);
                    res = min(res,jt * hn / t);
                    t = t + clamp(0.02,2,jt);
                }
                return saturate(res);
            }

            // The MIT License
            // Copyright ? 2013 Inigo Quilez
            // Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
            // https://www.shadertoy.com/view/ld2GRz

            float4 material(float3 pos)
            {
                float4 ballcol[6] = { _Color, _Color2, _Color3, _Color4, _Color5, _Color6};
                float3 mate = float3(0,0,0);
                float w = 0.01;
                // Making ball color
                for (int i = 0; i < 6; ++i) 
                {
                    float x = clamp((length(metaballvalue(i).xyz - pos) - metaballvalue(i).w) * 10,0,1);
                    float p = 1.0 - x * x * (3.0 - 2.0 * x);
                    mate += p * float3(ballcol[i].xyz);
                    w += p;
                }
                // Making floor color
                /*
                float x = clamp((pos.y - _ypos) * 10,0,1);
                float p = 1.0 - x * x * (3.0 - 2.0 * x);
                mate += p * float3(0.2,0.2,0.2);
                w += p;
                mate /= w;
                */
                return float4(mate,1);
            }
            ////////////////////////////////////////////////////

            //Phong reflection model ,Directional light
            /*フォンの鏡面反射*/
            float4 lighting(float3 pos)
            {
                //位置
                float3 mpos = pos;
                //法線
                float3 normal = getnormal(mpos);

                //オブジェクトスペースの座標をワールドスペースに変換
                pos = mul(unity_ObjectToWorld,float4(pos,1)).xyz;
                normal = normalize(mul(unity_ObjectToWorld,float4(normal,0)).xyz);

                //視線方向
                float3 viewdir = normalize(pos - _WorldSpaceCameraPos);
                //光源方向
                half3 lightdir = normalize(UnityWorldSpaceLightDir(pos));

                //陰影付け
                float sha = softray(mpos,lightdir,3.3);
                //色
                float4 Color = material(mpos);

                //法線と光源方向の内積
                float NdotL = max(0,dot(normal,lightdir));
                //反射方向
                float3 R = -normalize(reflect(lightdir,normal));
                //スペキュラー
                float3 spec = pow(max(dot(R,-viewdir),0),10);

                float4 col = sha * (Color * NdotL + float4(spec,0));
                return col;
            }



            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 pos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };



            v2f vert(appdata v)
            {
                v2f o;
                //モデルスペースからクリップスペースへの変換
                o.vertex = UnityObjectToClipPos(v.vertex);
                //モデルスペースからクリップスペースへの変換したxyzを渡す
                o.pos = mul(unity_ObjectToWorld,v.vertex).xyz;
                //uvを渡す
                o.uv = v.uv;

                return o;
            }

            struct pout
            {
                float4 pixel: SV_Target;
                float depth : SV_Depth;

            };

            pout frag(v2f i)
            {
                //視点の位置
                float3 ro = mul(unity_WorldToObject,float4(_WorldSpaceCameraPos,1)).xyz;
                //進む方向
                float3 rd = normalize(mul(unity_WorldToObject,float4(i.pos,1)).xyz - mul(unity_WorldToObject,float4(_WorldSpaceCameraPos,1)).xyz);
                //レイマーチング
                float t = raymarch(ro,rd);
                fixed4 col = 0;

                if (t == -1) //レイが衝突しない場合
                {
                    //clipの引数に渡した値が0以下となった場合、描画しない
                    clip(-1);
                }
                else
                {
                    //座標 = 視点の位置 * 進む方向 * レイマーチングの結果
                    float3 pos = ro + rd * t;
                    col = lighting(pos);
                }
                pout o;
                //ピクセルに色を付ける
                o.pixel = col;
                //オブジェクト空間からカメラのクリップ空間へ変換
                float4 curp = UnityObjectToClipPos(float4(ro + rd * t,1));
                o.depth = (curp.z) / (curp.w); //Drawing depth

                return o;

            }

        ENDCG

        }
    }
}
