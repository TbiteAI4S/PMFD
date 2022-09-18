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

        //���^�{�[���̐�
        _MetaballCount("metaballCount",int) = 6
        
        

        //���̍���
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

            //���^�{�[���̐�
            int _MetaballCount;

            //�ꏊ
            float4 _MPosition;
            float4 _MPositions[6];//���I�z��͎g���Ȃ��C�������w�肵�Đ錾

            
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
            * �n����悤�Ɋ��炩�ɐڑ�
            * d1:���� , d2:���� , k:�W��
            */
            float smoothMin(float d1,float d2,float k)
            {
                return -log(exp(-k * d1) + exp(-k * d2)) / k;
            }

            // Base distance function
            /*
            * ���^�{�[�X�̃T�C�Y���擾
            * p:�{�[���̍��W , s:���̔��a
            */
            float ball(float3 p,float s)
            {
                return length(p) - s;
            }


            // Making ball status
            /*
            * �{�[���̈ʒu�Ɣ��a�̎擾
            * i:�{�[���̔ԍ�
            */
            float4 metaballvalue(float i)
            {
                //���Ԃ𗘗p�����W��
                float kt = 3 * _Time.y * (0.1 + 0.01 * i);
                //�W���ƃm�C�Y�𗘗p���ă{�[���̍��W������
                //float3 ballpos = 0.3 * float3(noise(float2(i,i) + kt),noise(float2(i + 10,i * 20) + kt),noise(float2(i * 20,i + 20) + kt));
                //float3 ballpos = 0.1 * float3(0.0f + i, 0.0f + i, 0.0f + i + kt);

                float3 ballpos = 0.1 * float3(_MPosition[0] + i, _MPosition[1] + 0.5 * i, _MPosition[2] + 0.03 * i);

                //�W���ƃm�C�Y�𗘗p���ă{�[���̃T�C�Y������
                float scale = 0.05 + 0.02 * hash(float2(i,i));
                //float scale = 0.01 + 0.01 * hash(float2(i,i));

                return  float4(ballpos,scale);
            }
            // Making ball distance function
            /*
            * MetaballValue�𔽉f���������֐�
            * p:���W, i:�{�[���̔ԍ�
            */
            float metaballone(float3 p, float i)
            {
                //�{�[���̈ʒu�C�T�C�Y������
                float4 value = metaballvalue(i);
                //�{�[���̏ꏊ���v�Z
                float3 ballpos = p - value.xyz;
                //�{�[���̃T�C�Y���擾
                float scale = value.w;

                return  ball(ballpos,scale);
            }

            //Making metaballs distance function
            /*
            * ���^�{�[��(�Q)�̋����֐�
            * p:���W
            */
            float metaball(float3 p)
            {
                //����1
                float d1;
                //����2��1���[�v�O�̋���
                //���߂�p��0�̋���
                float d2 = metaballone(p,0);
                //�c��5�̃{�[���̋����֐����Z�o
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
            * ���C�}�[�`���O
            * ro:���C, rd:����
            */
            float raymarch(float3 ro,float3 rd)
            {
                //�O���̔��a
                float previousradius = 0.0;
                //�ő勗��
                float maxdistance = 3;
                //���C�̃X�^�[�g�n�_�����̂̒��ɂ��邩�O�ɂ��邩
                float outside = dist(ro) < 0 ? -1 : +1;
                //t = 1�ɂ������f�̔��a
                float pixelradius = 0.02;
                //���C�̐i�ރX�s�[�h
                float omega = 1.2;
                //
                float t = 0.0001;
                //1���[�v�O�ɐi�񂾋���
                float step = 0;
                //pixelt�̍ŏ��l
                float minpixelt = 999999999;
                //pixelt ����ԏ���������t�̒l
                float mint = 0;

                float hit = 0.01;

                for (int i = 0; i < 60; ++i) 
                {

                    float radius = outside * dist(ro + rd * t);
                    bool fail = omega > 1 && step > (abs(radius) + abs(previousradius));
                    //step�̒l�����i�����Ƃ��Ă��鋗��radius�ƂP���[�v�O�ɐi�����Ƃ�������previousradius(omega���������Ă��Ȃ�����step�ƈقȂ�l�ł��邱�Ƃɒ���)
                    //�̘a���傫����ΐi�݂����Ȃ���omega��1.0�ɂ��ĕ��ʂɐi��
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
                    //�ł��I�u�W�F�N�g�ɋ߂������_���Փ˓_�̌��Ƃ���
                    if (!fail && pixelt < minpixelt) 
                    {
                        minpixelt = pixelt;
                        mint = t;
                    }
                    //�ő勗���ɉ����āu�X�N���[���ɑ΂��郌�C�̐i�ޔ��a�̑傫���i���Ȃ킿pixelt�j�v��
                    //�s�N�Z���̑傫����菬�����̂Ȃ�΃��[�v���I����Ă��悢
                    if (!fail && pixelt<pixelradius || t>maxdistance)
                    break;
                    t += step;
                }

                //�X�N���[���ɑ΂��郌�C�̐i�ޔ��a�̑傫���ipixelt�j�v�̍ŏ��l���s�N�Z���̑傫�����傫���A
                //����pixelt ����ԏ���������t�̒lmint�����̒l�i�����ł�hit�j���傫���Ƃ��͓������Ă��Ȃ�����
                if ((t > maxdistance || minpixelt > pixelradius) && (mint > hit)) 
                {
                    return -1;
                }
                else//�X�N���[���ɑ΂��郌�C�̐i�ޔ��a�̑傫���ipixelt�j�v����ԏ�������������t�i���Ȃ킿pixelt ����ԏ���������t�̒lmint�j�������Ƃ���    
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
            /*�A�e�t��*/
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
            /*�t�H���̋��ʔ���*/
            float4 lighting(float3 pos)
            {
                //�ʒu
                float3 mpos = pos;
                //�@��
                float3 normal = getnormal(mpos);

                //�I�u�W�F�N�g�X�y�[�X�̍��W�����[���h�X�y�[�X�ɕϊ�
                pos = mul(unity_ObjectToWorld,float4(pos,1)).xyz;
                normal = normalize(mul(unity_ObjectToWorld,float4(normal,0)).xyz);

                //��������
                float3 viewdir = normalize(pos - _WorldSpaceCameraPos);
                //��������
                half3 lightdir = normalize(UnityWorldSpaceLightDir(pos));

                //�A�e�t��
                float sha = softray(mpos,lightdir,3.3);
                //�F
                float4 Color = material(mpos);

                //�@���ƌ��������̓���
                float NdotL = max(0,dot(normal,lightdir));
                //���˕���
                float3 R = -normalize(reflect(lightdir,normal));
                //�X�y�L�����[
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
                //���f���X�y�[�X����N���b�v�X�y�[�X�ւ̕ϊ�
                o.vertex = UnityObjectToClipPos(v.vertex);
                //���f���X�y�[�X����N���b�v�X�y�[�X�ւ̕ϊ�����xyz��n��
                o.pos = mul(unity_ObjectToWorld,v.vertex).xyz;
                //uv��n��
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
                //���_�̈ʒu
                float3 ro = mul(unity_WorldToObject,float4(_WorldSpaceCameraPos,1)).xyz;
                //�i�ޕ���
                float3 rd = normalize(mul(unity_WorldToObject,float4(i.pos,1)).xyz - mul(unity_WorldToObject,float4(_WorldSpaceCameraPos,1)).xyz);
                //���C�}�[�`���O
                float t = raymarch(ro,rd);
                fixed4 col = 0;

                if (t == -1) //���C���Փ˂��Ȃ��ꍇ
                {
                    //clip�̈����ɓn�����l��0�ȉ��ƂȂ����ꍇ�A�`�悵�Ȃ�
                    clip(-1);
                }
                else
                {
                    //���W = ���_�̈ʒu * �i�ޕ��� * ���C�}�[�`���O�̌���
                    float3 pos = ro + rd * t;
                    col = lighting(pos);
                }
                pout o;
                //�s�N�Z���ɐF��t����
                o.pixel = col;
                //�I�u�W�F�N�g��Ԃ���J�����̃N���b�v��Ԃ֕ϊ�
                float4 curp = UnityObjectToClipPos(float4(ro + rd * t,1));
                o.depth = (curp.z) / (curp.w); //Drawing depth

                return o;

            }

        ENDCG

        }
    }
}
