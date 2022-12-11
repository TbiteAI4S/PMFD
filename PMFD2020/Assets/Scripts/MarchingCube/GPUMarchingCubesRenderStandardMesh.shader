Shader "Custom/GPUMarchingCubesRenderStandardMesh"
{
    Properties
    {
        //������
        _SegmentNum("SegmentNum", int) = 32
        //�傫��
        _Scale("Scale", float) = 1
        //臒l
        _Threashold("Threashold", float) = 0.5
        //�F
        _DiffuseColor("Diffuse", Color) = (0,0,0,1)
        //����
        _EmissionIntensity("Emission Intensity", Range(0,1)) = 1
        _EmissionColor("Emission", Color) = (0,0,0,1)
        //����
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags{ "RenderType" = "Opaque" }

        CGINCLUDE
        #define UNITY_PASS_DEFERRED
        #include "HLSLSupport.cginc"
        #include "UnityShaderVariables.cginc"
        #include "UnityCG.cginc"
        #include "Lighting.cginc"
        #include "UnityPBSLighting.cginc"
        //�I���W�i����include
        #include "Primitives.cginc"
        #include "Utils.cginc"

        // ���b�V������n���Ă��钸�_�f�[�^
        struct appdata
        {
            float4 vertex	: POSITION;	// ���_���W
        };

        // ���_�V�F�[�_����W�I���g���V�F�[�_�ɓn���f�[�^
        struct v2g
        {
            float4 pos : SV_POSITION;	// ���_���W
        };

        // ���̃����_�����O���̃W�I���g���V�F�[�_����t���O�����g�V�F�[�_�ɓn���f�[�^
        struct g2f_light
        {
            float4 pos			: SV_POSITION;	// ���[�J�����W
            float3 normal		: NORMAL;		// �@��
            float4 worldPos		: TEXCOORD0;	// ���[���h���W
            half3 sh            : TEXCOORD3;	// SH
        };

        // �e�̃����_�����O���̃W�I���g���V�F�[�_����t���O�����g�V�F�[�_�ɓn���f�[�^
        struct g2f_shadow
        {
            float4 pos			: SV_POSITION;	// ���W
            float4 hpos			: TEXCOORD1;
        };

        //������
        int _SegmentNum;

        //�傫��
        float _Scale;
        //臒l
        float _Threashold;

        //�J���[
        float4 _DiffuseColor;

        float3 _HalfSize;
        float4x4 _Matrix;

        //����
        float _EmissionIntensity;
        half3 _EmissionColor;

        //����
        half _Glossiness;
        half _Metallic;

        //�萔
        StructuredBuffer<float3> vertexOffset;
        StructuredBuffer<int> cubeEdgeFlags;
        StructuredBuffer<int2> edgeConnection;
        StructuredBuffer<float3> edgeDirection;
        StructuredBuffer<int> triangleConnectionTable;

        // �T���v�����O�֐�
        float Sample(float x, float y, float z) {

            // ���W���O���b�h��Ԃ���͂ݏo���Ă����Ȃ����H
            if ((x <= 1) || (y <= 1) || (z <= 1) || (x >= (_SegmentNum - 1)) || (y >= (_SegmentNum - 1)) || (z >= (_SegmentNum - 1)))
                return 0;

            //�L���[�u�̃T�C�Y = (������)^3
            float3 size = float3(_SegmentNum, _SegmentNum, _SegmentNum);

            float3 pos = float3(x, y, z) / size;

            //float3 spPos;
            float result = 0;

            // �R�̋��̋����֐�
            for (int i = 0; i < 3; i++) {
                float sp = -sphere(pos - float3(0.5, 0.25 + 0.25 * i, 0.5), 0.005 + (sin(_Time.y * 8.0 + i * 23.365) * 0.5 + 0.5) * 0.125) + 0.5;
                result = smoothMax(result, sp, 14);
            }
                return result;
        }

        // �I�t�Z�b�g�v�Z�i2�l�̊Ԃ�臒l(desired)�ɋ߂��_���v�Z����j
        float getOffset(float val1, float val2, float desired) {
            float delta = val2 - val1;
            if (delta == 0.0) {
                return 0.5;
            }
            return (desired - val1) / delta;
        }

        // �@���v�Z
        float3 getNormal(float fX, float fY, float fZ)
        {
            float3 normal;
            float offset = 1.0;	// �ׂ̃O���b�h

            normal.x = Sample(fX - offset, fY, fZ) - Sample(fX + offset, fY, fZ);
            normal.y = Sample(fX, fY - offset, fZ) - Sample(fX, fY + offset, fZ);
            normal.z = Sample(fX, fY, fZ - offset) - Sample(fX, fY, fZ + offset);

            return normal;
        }

        // ���_�V�F�[�_
        v2g vert(appdata v)
        {
            v2g o = (v2g)0;
            o.pos = v.vertex;
            return o;
        }

        // ���̂̃W�I���g���V�F�[�_
        [maxvertexcount(15)]	// �V�F�[�_����o�͂��钸�_�̍ő吔�̒�`
        void geom_light(point v2g input[1], inout TriangleStream<g2f_light> outStream)
        {
            g2f_light o = (g2f_light)0;

            int i, j;
            float cubeValue[8];	// �O���b�h�̂W�̊p�̃X�J���[�l�擾�p�̔z��

            // ���_�z��
            float3 edgeVertices[12] = {
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0) };

            // �@���z��
            float3 edgeNormals[12] = {
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0) };

            float3 pos = input[0].pos.xyz;
            float3 defpos = pos;

            // �O���b�h�̂W�̊p�̃X�J���[�l���擾
            for (i = 0; i < 8; i++) {
                cubeValue[i] = Sample(
                    pos.x + vertexOffset[i].x,
                    pos.y + vertexOffset[i].y,
                    pos.z + vertexOffset[i].z
                );
            }

            pos *= _Scale;
            pos -= _HalfSize;

            int flagIndex = 0;

            // �O���b�h�̂W�̊p�̒l��臒l�𒴂��Ă��邩�`�F�b�N
            for (i = 0; i < 8; i++) {
                if (cubeValue[i] <= _Threashold) {
                    flagIndex |= (1 << i);
                }
            }

            int edgeFlags = cubeEdgeFlags[flagIndex];

            // �󂩊��S�ɖ�������Ă���ꍇ�͉����`�悵�Ȃ�
            if ((edgeFlags == 0) || (edgeFlags == 255)) {
                return;
            }

            float offset = 0.5;
            float3 vertex;
            for (i = 0; i < 12; i++) {
                if ((edgeFlags & (1 << i)) != 0) {
                    // �p���m��臒l�̃I�t�Z�b�g���擾
                    offset = getOffset(cubeValue[edgeConnection[i].x], cubeValue[edgeConnection[i].y], _Threashold);

                    // �I�t�Z�b�g�����ɒ��_�̍��W��⊮
                    vertex = (vertexOffset[edgeConnection[i].x] + offset * edgeDirection[i]);

                    edgeVertices[i].x = pos.x + vertex.x * _Scale;
                    edgeVertices[i].y = pos.y + vertex.y * _Scale;
                    edgeVertices[i].z = pos.z + vertex.z * _Scale;

                    // �@���v�Z�iSample���������߁A�X�P�[�����|����O�̒��_���W���K�v�j
                    edgeNormals[i] = getNormal(defpos.x + vertex.x, defpos.y + vertex.y, defpos.z + vertex.z);
                }
            }

            // ���_��A�����ă|���S�����쐬
            int vindex = 0;
            int findex = 0;
            // �ő�T�̎O�p�`���ł���
            for (i = 0; i < 5; i++) {
                findex = flagIndex * 16 + 3 * i;
                if (triangleConnectionTable[findex] < 0)
                    break;

                // �O�p�`�����
                for (j = 0; j < 3; j++) {
                    vindex = triangleConnectionTable[findex + j];

                    // Transform�s����|���ă��[���h���W�ɕϊ�
                    float4 ppos = mul(_Matrix, float4(edgeVertices[vindex], 1));
                    o.pos = UnityObjectToClipPos(ppos);

                    float3 norm = UnityObjectToWorldNormal(normalize(edgeNormals[vindex]));
                    o.normal = normalize(mul(_Matrix, float4(norm,0)));

                    outStream.Append(o);	// �X�g���b�v�ɒ��_��ǉ�
                }
                outStream.RestartStrip();	// ��U��؂��Ď��̃v���~�e�B�u�X�g���b�v���J�n
            }
        }

        // ���̂̃t���O�����g�V�F�[�_
        void frag_light(g2f_light IN,
            out half4 outDiffuse		: SV_Target0,
            out half4 outSpecSmoothness : SV_Target1,
            out half4 outNormal : SV_Target2,
            out half4 outEmission : SV_Target3)
        {
            //�@���擾
            fixed3 normal = IN.normal;

            //���[���h���W
            float3 worldPos = IN.worldPos;

            //��������
            fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
            /*
#ifdef UNITY_COMPILER_HLSL
                SurfaceOutputStandard o = (SurfaceOutputStandard)0;
#else
                SurfaceOutputStandard o;
#endif
                o.Albedo = _DiffuseColor.rgb;
                o.Emission = _EmissionColor * _EmissionIntensity;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = 1.0;
                o.Occlusion = 1.0;
                o.Normal = normal;

                // Setup lighting environment
                UnityGI gi;
                UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
                //gi.indirect.diffuse = 0;
                //gi.indirect.specular = 0;
                //gi.light.color = 0;
                //gi.light.dir = half3(0, 1, 0);
                //gi.light.ndotl = LambertTerm(o.Normal, gi.light.dir);

                // Call GI (lightmaps/SH/reflections) lighting function
                UnityGIInput giInput;
                UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
                giInput.light = gi.light;
                giInput.worldPos = worldPos;
                giInput.worldViewDir = worldViewDir;
                giInput.atten = 1.0;

                giInput.ambient = IN.sh;

                giInput.probeHDR[0] = unity_SpecCube0_HDR;
                giInput.probeHDR[1] = unity_SpecCube1_HDR;

        #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
        #endif

        #if UNITY_SPECCUBE_BOX_PROJECTION
                giInput.boxMax[0] = unity_SpecCube0_BoxMax;
                giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
                giInput.boxMax[1] = unity_SpecCube1_BoxMax;
                giInput.boxMin[1] = unity_SpecCube1_BoxMin;
                giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
        #endif

                LightingStandard_GI(o, giInput, gi);

                // call lighting function to output g-buffer
                outEmission = LightingStandard_Deferred(o, worldViewDir, gi, outDiffuse, outSpecSmoothness, outNormal);
                outDiffuse.a = 1.0;

        #ifndef UNITY_HDR_ON
                outEmission.rgb = exp2(-outEmission.rgb);
        #endif
        */
            outDiffuse = _DiffuseColor;
            outSpecSmoothness = half4 (0.0f,0.0f,0.0f, _Glossiness);
            outNormal = half4(normal * 0.5f + 0.5f, 1.0f);
            outEmission = 0;
        }

        // �e�̃W�I���g���V�F�[�_
        [maxvertexcount(15)]
        void geom_shadow(point v2g input[1], inout TriangleStream<g2f_shadow> outStream)
        {
            g2f_shadow o = (g2f_shadow)0;

            int i, j;
            float cubeValue[8];
            float3 edgeVertices[12] = {
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0) };
            float3 edgeNormals[12] = {
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0),
                float3(0, 0, 0) };

            float3 pos = input[0].pos.xyz;
            float3 defpos = pos;

            for (i = 0; i < 8; i++) {
                cubeValue[i] = Sample(
                    pos.x + vertexOffset[i].x,
                    pos.y + vertexOffset[i].y,
                    pos.z + vertexOffset[i].z
                );
            }

            pos *= _Scale;
            pos -= _HalfSize;

            int flagIndex = 0;

            for (i = 0; i < 8; i++) {
                if (cubeValue[i] <= _Threashold) {
                    flagIndex |= (1 << i);
                }
            }

            int edgeFlags = cubeEdgeFlags[flagIndex];

            if ((edgeFlags == 0) || (edgeFlags == 255)) {
                return;
            }

            float offset = 0.5;
            float3 vertex;
            for (i = 0; i < 12; i++) {
                if ((edgeFlags & (1 << i)) != 0) {
                    offset = getOffset(cubeValue[edgeConnection[i].x], cubeValue[edgeConnection[i].y], _Threashold);

                    vertex = (vertexOffset[edgeConnection[i].x] + offset * edgeDirection[i]);

                    edgeVertices[i].x = pos.x + vertex.x * _Scale;
                    edgeVertices[i].y = pos.y + vertex.y * _Scale;
                    edgeVertices[i].z = pos.z + vertex.z * _Scale;

                    edgeNormals[i] = getNormal(defpos.x + vertex.x, defpos.y + vertex.y, defpos.z + vertex.z);
                }
            }

            int vindex = 0;
            int findex = 0;
            for (i = 0; i < 5; i++) {
                findex = flagIndex * 16 + 3 * i;
                if (triangleConnectionTable[findex] < 0)
                    break;

                for (j = 0; j < 3; j++) {
                    vindex = triangleConnectionTable[findex + j];

                    float4 ppos = mul(_Matrix, float4(edgeVertices[vindex], 1));

                    float3 norm;
                    norm = UnityObjectToWorldNormal(normalize(edgeNormals[vindex]));

                    float4 lpos1 = mul(unity_WorldToObject, ppos);
                    o.pos = UnityClipSpaceShadowCasterPos(lpos1, normalize(mul(_Matrix, float4(norm, 0))));
                    o.pos = UnityApplyLinearShadowBias(o.pos);
                    o.hpos = o.pos;

                    outStream.Append(o);
                }
                outStream.RestartStrip();
            }
        }

        // �e�̃t���O�����g�V�F�[�_
        fixed4 frag_shadow(g2f_shadow i) : SV_Target
        {
            return i.hpos.z / i.hpos.w;
        }
        ENDCG

        // ���̂̃����_�����O
        Pass {
            Tags{ "LightMode" = "Deferred" }

            CGPROGRAM
            #pragma target 5.0
            #pragma vertex vert
            #pragma geometry geom_light
            #pragma fragment frag_light
            #pragma exclude_renderers nomrt
            #pragma multi_compile_prepassfinal noshadow
            ENDCG
        }

        // �e�̃����_�����O
        Pass {
            Tags{ "LightMode" = "ShadowCaster" }
            ZWrite On ZTest LEqual
            CGPROGRAM
            #pragma target 5.0
            #pragma vertex vert
            #pragma geometry geom_shadow
            #pragma fragment frag_shadow
            #pragma multi_compile_shadowcaster
            ENDCG
        }
    }
    FallBack "Diffuse"
}
