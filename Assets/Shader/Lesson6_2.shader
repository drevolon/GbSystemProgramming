Shader "Unlit/Lesson6_2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Main Color", COLOR) = (1,1,1,1) 
        _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1)
       
        _AtmosphereRadius("AtmosphereRadius", Range(0,100))=0
        _Degrees("Degrees", Range(0,500))=179
        _Strength("Strength", Range(0,1))=0
        _Step("Step", Range(-1,1))=0
        _Pow("Pow", Range(0,50))=0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal: NORMAL;
                float2 texcoord : TEXCOORD2;
               
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                
                float2 texcoord : TEXCOORD2;
                float4 dotProduct : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
                        
            float4 _AtmoColor;
            float _FalloffPlanet;
            float _TransparencyPlanet;

            float4 _AtmosphereRadius;
            float _Strength;
            float _Degrees;
            float _Step;
            int _Pow;

            v2f vert (appdata v)
            {
               v2f o;
               float4 cameraLocalPos=mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0));
               float4 viewDir=v.vertex - cameraLocalPos;
               float dotProduct=dot(v.normal, normalize(viewDir));
               v.vertex.xyz*=(1+_AtmosphereRadius);
               o.vertex=UnityObjectToClipPos(v.vertex);
               o.uv=TRANSFORM_TEX(v.texcoord, _MainTex);
               o.dotProduct.x=abs(dotProduct);
 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed alpha=_Strength*pow(abs(sin(i.dotProduct.x*radians(_Degrees)+_Step)),_Pow);
                fixed4 color=_AtmoColor;
                color.w=alpha;
                return color;
            }
            ENDCG
        }
    }
}
