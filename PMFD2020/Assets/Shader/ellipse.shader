Shader "Custom/ellipse"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        #define UNITY_PASS_DEFERRED
        #include "HLSLSupport.cginc"
        #include "UnityShaderVariables.cginc"
        #include "UnityCG.cginc"

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
            half3 sh : TEXCOORD3;				// SH
        };



        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
