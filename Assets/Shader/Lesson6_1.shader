Shader "Unlit/Lesson6_1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Main Color", COLOR) = (1,1,1,1) // 
        _Height("Height", Range(-20,20)) = 0.5 // 
        _Scale("Scale", Range(0,20)) = 1 // 
        _PointSet("PointSet", Range(0,1))=0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color; // цвет, которым будет окрашиваться изображение
            float _Height; // 
            float _Scale; // 
            float _PointSet;

            v2f vert (appdata v)
            {
                v2f o;

                float4 vertex=v.vertex;
                vertex.y-=_Height*pow(v.uv.x-_PointSet,2);

                //v.vertex.xyz += v.normal * _Height * v.texcoord.x * v.texcoord.x;

                o.vertex = UnityObjectToClipPos(vertex*_Scale);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
               // UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
               // fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
               // UNITY_APPLY_FOG(i.fogCoord, col);

               fixed4 col = tex2D(_MainTex, i.uv);
                col = col * _Color;
                return col;

            }
            ENDCG
        }
    }
}
