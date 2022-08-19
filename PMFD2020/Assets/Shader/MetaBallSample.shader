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
        * Blend Off: �u�����h�𖳌��ɂ��܂� (���ꂪ�f�t�H���g)
        * Blend SrcFactor DstFactor : �u�����f�B���O��ݒ肵�g�p�\�ɂ��܂��B�������ꂽ�J���[�� SrcFactor �Ə�Z���܂��B��ʂɂ��łɂ���J���[�� DstFactor �Ə�Z���A2 �͍�������܂��B
        * Blend SrcFactor DstFactor�ASrcFactorA DstFactorA : ��L�Ɠ��l�ł����A�A���t�@�`�����l�����u�����f�B���O����̂ɁA�قȂ�v�f���g�p���܂��B
        * BlendOp Op: �u�����h�����J���[�����킹�����ɁA�قȂ鑀����s���܂��B
        * BlendOp OpColor, OpAlpha: ��L�Ɠ��l�ł����A�J���[ (RGB) �ƃA���t�@ (A) �`�����l���ɑ΂��قȂ�u�����h������g�p���܂��B
        */
        Blend SrcAlpha OneMinusSrcAlpha
        /*�J�����O 
        * Back( �@���������������Ă���|���S����`�悵�Ȃ�) 
        * Front(�@�����\���������Ă���|���S����`�悵�Ȃ�)
        * Off(�J�����O�𖳌��ɂ��Ă��ׂĂ̖ʂ�`��)
        */
        Cull Off
        ZWrite Off 			// Don't write on the ZBuffer.
        Lighting Off		// Don't let unity do lighting calculations.

        Pass
        {
            /*Pass�̖��O*/
            Name "MetaBallSamples"


            CGPROGRAM         

            /*�o�[�e�b�N�X�V�F�[�_�[*/
            #pragma vertex vert
            /*�t���O�����g�V�F�[�_�[*/
            #pragma fragment frag

            #include "UnityCG.cginc"

            /*���_�f�[�^�ւ̓��͒�`*/
            struct appdata
            {
                //���_�̍��W
                float4 vertex : POSITION;
            };

            /*�t���O�����g�V�F�[�_�[�֓n���f�[�^*/
            struct v2f
            {
                //���W�ϊ����ꂽ��̒��_���W
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD0;
            };
            

            /*
            * ���^�{�[��
            * �Q�l�Fhttps://butadiene.hatenablog.com/entry/2019/02/23/075535#%E5%BD%B1%E3%82%92%E3%81%A4%E3%81%91%E3%82%8B
            * �Q�l�Fhttps://baba-s.hatenablog.com/entry/2018/04/10/094100
            */

            uniform float3 _ParticlesPos[10];   //���^�{�[���̍��W

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
            fixed _Threshold;                   //���^�{�[���̔��a
            fixed _Epsilon;                     //���K���z

            
            ///*
            //* �n����悤�Ɋ��炩�ɐڑ�
            //* d1:���� , d2:���� , k:�W��
            //*/
            //float SmoothConnection(float d1,float d2,float k)
            //{
            //    return -log(exp(-k * d1) + exp(-k * d2)) / k;
            //}

            ///*
            //* ���^�{�[�X�̃T�C�Y���擾
            //* p:�{�[���̍��W , s:���̔��a
            //*/
            //float Ball(float3 p, float s)
            //{
            //    return length(p) - s;
            //}   

            ///*
            //* �{�[���̈ʒu�Ɣ��a�̎擾
            //* i:�{�[���̔ԍ�
            //*/
            //float4 MetaballValue(float i)
            //{
            //    //�{�[���̈ʒu
            //    float3 ballpos = _ParticlesPos[i].xyz;
            //    //�{�[���̔��a
            //    float scale = _Threshold;

            //    return  float4(ballpos,scale);
            //}
            //
            ///*
            //* MetaballValue�𔽉f���������֐�
            //* p:���W, i:�{�[���̔ԍ�
            //*/
            //float MetaballOne(float3 p, float i)
            //{
            //    //MetaballValue���擾
            //    float4 value = MetaballValue(i);
            //    //���^�{�[���̏ꏊ���v�Z
            //    float3 ballpos = p - value.xyz;
            //    //���^�{�[���̃T�C�Y
            //    float scale = value.w;

            //    return  Ball(ballpos,scale);
            //}

            ///*
            //* ���^�{�[��(�Q)�̋����֐�
            //* p:���W
            //*/
            //float Metaball(float3 p)
            //{
            //    //�{�[���Ƃ̋���
            //    float d1;
            //    //1���[�v�O�Ɍv�Z�����{�[���Ƃ̋���
            //    float d2 = MetaballOne(p,0);
            //    //6�̃{�[���ɑ΂��Čv�Z
            //    for (int i = 1; i < 6; ++i) 
            //    {
            //        d1 = MetaballOne(p,i);
            //        //d1�Ɋ��炩�ɐڑ�����l���i�[
            //        d1 = SmoothConnection(d1,d2,20);
            //        //d2���X�V
            //        d2 = d1;
            //    }
            //    return d1;
            //}

            /*�v�Z*/
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

            /*�@�����Z�o*/
            float3 calcNormal(float3 p) {
                float3 N;
                N.x = scalarField(p.x + _Epsilon, p.y, p.z) - scalarField(p.x - _Epsilon, p.y, p.z);
                N.y = scalarField(p.x, p.y + _Epsilon, p.z) - scalarField(p.x, p.y - _Epsilon, p.z);
                N.z = scalarField(p.x, p.y, p.z + _Epsilon) - scalarField(p.x, p.y, p.z - _Epsilon);
                return -N;
            }

            //�A�e�t��
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

            /*vert�֐��֒��_���𑗂�*/
            v2f vert(appdata v)
            {
                v2f o;
                //���_�����̃x�N�g�������߂�(���[���h���W�n)
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                //���f���X�y�[�X����N���b�v�X�y�[�X�ւ̕ϊ�
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                //�t���O�����g�̐F�̏�����
                float4 col = _Color;
                col.a = 0;

                // ���C�L���X�g�̏���
                float3 viewDir = normalize(i.worldPosition - _WorldSpaceCameraPos.xyz);//�J��������t���O�����g�ւ̃��[���h�X�y�[�X����
                float3 start = i.worldPosition - 2.0 * viewDir; //viewDir�ɉ������t���O�����g��2���j�b�g��O����T���v�����O���͂��߂�
                float3 p; //�T���v�����O���ꂽ�_

                // ���C�̃T���v�����O���J�n ���C�ɉ����� 0.2 �|�C���g���ő� 3 �P�ʂ̒����ŃT���v�����O
                // ���l�ʂƂ̌�_����������A��_�𐳊m�ɓ���ł���悤�ɂȂ�܂ŁA����ɏ����ȃX�e�b�v�ŃT���v�����O
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
