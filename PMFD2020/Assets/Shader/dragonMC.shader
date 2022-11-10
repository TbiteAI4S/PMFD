Shader "Custom/dragonMC"
{
    Properties
    {
        _SegmentNum("SegmentNum", int) = 32

        _Scale("Scale", float) = 1
        _Threashold("Threashold", float) = 0.5

        _DiffuseColor("Diffuse", Color) = (0,0,0,1)

        _EmissionIntensity("Emission Intensity", Range(0,1)) = 1
        _EmissionColor("Emission", Color) = (0,0,0,1)

        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass{
            CGPROGRAM
            #include "UnityCG.cginc"

            // メッシュから渡ってくる頂点データ
            struct appdata
            {
                float4 vertex	: POSITION;	// 頂点座標
            };

            // 頂点シェーダからジオメトリシェーダに渡すデータ
            struct v2g
            {
                float4 pos : SV_POSITION;	// 頂点座標
            };

            // 実体レンダリング時のジオメトリシェーダからフラグメントシェーダに渡すデータ
            struct g2f_light
            {
                float4 pos			: SV_POSITION;	// ローカル座標
                float3 normal		: NORMAL;		// 法線
                float4 worldPos		: TEXCOORD0;	// ワールド座標
                half3 sh : TEXCOORD3;				// SH
            };

            // 影のレンダリング時のジオメトリシェーダからフラグメントシェーダに渡すデータ
            struct g2f_shadow
            {
                float4 pos			: SV_POSITION;	// 座標
                float4 hpos			: TEXCOORD1;
            };
            
            int _SegmentNum;            // グリッドの一辺の分割数

            float _Scale;               // 表示スケール
            float _Threashold;          // メッシュ化するスカラー値のしきい値

            float4 _DiffuseColor;       // ディフューズカラー
            float3 _HalfSize;
            float4x4 _Matrix;

            float _EmissionIntensity;   // 発光の強さ
            half3 _EmissionColor;       // 発光色

            half _Glossiness;           // 光沢感
            half _Metallic;             // メタリック感


            //MarchingCubesDefinesクラスのComputeBuffer
            StructuredBuffer<float3> vertexOffset;
            StructuredBuffer<int> cubeEdgeFlags;
            StructuredBuffer<int2> edgeConnection;
            StructuredBuffer<float3> edgeDirection;
            StructuredBuffer<int> triangleConnectionTable;
           
            // 頂点シェーダ（実体と影 共通）
            v2g vert(appdata v)
            {
                v2g o = (v2g)0;
                o.pos = v.vertex;
                return o;
            }

            // 実体のジオメトリシェーダ
            [maxvertexcount(15)]	// シェーダから出力する頂点の最大数の定義
            void geom_light(point v2g input[1], inout TriangleStream<g2f_light> outStream)
            {
                g2f_light o = (g2f_light)0;

                int i, j;
                float cubeValue[8];	// グリッドの８つの角のスカラー値取得用の配列

                // 頂点配列
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

                // 法線配列
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

                //メッシュ作成時にグリッド空間に配置した頂点の座標
                float3 pos = input[0].pos.xyz;
                float3 defpos = pos;

                // グリッドの８つの角のスカラー値を取得
                for (i = 0; i < 8; i++) {
                    cubeValue[i] = Sample(
                        //posにオフセット座標を加える
                        pos.x + vertexOffset[i].x,
                        pos.y + vertexOffset[i].y,
                        pos.z + vertexOffset[i].z
                    );
                }

                pos *= _Scale;
                pos -= _HalfSize;

                int flagIndex = 0;

                // グリッドの８つの角の値が閾値を超えているかチェック
                for (i = 0; i < 8; i++) {
                    if (cubeValue[i] <= _Threashold) {
                        flagIndex |= (1 << i);
                    }
                }

                int edgeFlags = cubeEdgeFlags[flagIndex];

                // 空か完全に満たされている場合は何も描画しない
                if ((edgeFlags == 0) || (edgeFlags == 255)) {
                    return;
                }


                //ポリゴンの頂点座標計算
                float offset = 0.5;
                float3 vertex;
                for (i = 0; i < 12; i++) {
                    if ((edgeFlags & (1 << i)) != 0) {
                        // 角同士の閾値のオフセットを取得
                        offset = getOffset(cubeValue[edgeConnection[i].x], cubeValue[edgeConnection[i].y], _Threashold);

                        // オフセットを元に頂点の座標を補完
                        vertex = (vertexOffset[edgeConnection[i].x] + offset * edgeDirection[i]);

                        edgeVertices[i].x = pos.x + vertex.x * _Scale;
                        edgeVertices[i].y = pos.y + vertex.y * _Scale;
                        edgeVertices[i].z = pos.z + vertex.z * _Scale;

                        // 法線計算（Sampleし直すため、スケールを掛ける前の頂点座標が必要）
                        edgeNormals[i] = getNormal(defpos.x + vertex.x, defpos.y + vertex.y, defpos.z + vertex.z);
                    }
                }

                // 頂点を連結してポリゴンを作成
                int vindex = 0;
                int findex = 0;
                // 最大５つの三角形ができる
                for (i = 0; i < 5; i++) {
                    findex = flagIndex * 16 + 3 * i;
                    if (triangleConnectionTable[findex] < 0)
                        break;

                    // 三角形を作る
                    for (j = 0; j < 3; j++) {
                        vindex = triangleConnectionTable[findex + j];

                        // Transform行列を掛けてワールド座標に変換
                        float4 ppos = mul(_Matrix, float4(edgeVertices[vindex], 1));
                        o.pos = UnityObjectToClipPos(ppos);

                        float3 norm = UnityObjectToWorldNormal(normalize(edgeNormals[vindex]));
                        o.normal = normalize(mul(_Matrix, float4(norm, 0)));

                        outStream.Append(o);	// ストリップに頂点を追加
                    }
                    outStream.RestartStrip();	// 一旦区切って次のプリミティブストリップを開始
                }
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}
