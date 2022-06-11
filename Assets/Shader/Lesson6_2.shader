Shader "Unlit/Lesson6_2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Main Color", COLOR) = (1,1,1,1) 
        _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1)
        _Size("Size", Float) = 0.1
        _Falloff("Falloff", Float) = 5
        _FalloffPlanet("Falloff Planet", Float) = 5
        _Transparency("Transparency", Float) = 15
        _TransparencyPlanet("Transparency Planet", Float) = 1
        _AtmosphereRadius("AtmosphereRadius", Range(0,100))=0
        _Degrees("Degrees", Range(0,1))=0
        _Strength("Strength", Range(0,1))=0
        _Step("Step", Range(0,1))=0
        _Pow("Pow", Range(0,10))=0
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
                float2 texcoord : TEXCOORD2;
                float4 pos : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 normal : TEXCOORD0;
                //float3 dotProduct : TEXCOORD1;
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
               v.pos.xyz*=(1+_AtmosphereRadius);
               o.pos=UnityObjectToClipPos(v.vertex);
               o.normal=TRANSFORM_TEX(v.texcoord, _MainTex);
               o.dotProduct.x=abs(dotProduct);
 
                //o.pos = UnityObjectToClipPos (v.vertex);
                //o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                //o.dotProduct = mul(unity_ObjectToWorld, v.vertex).xyz;
                //o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //i.normal = normalize(i.normal);
                   // float3 viewdir = normalize(_WorldSpaceCameraPos-i.dotProduct);
 
                   // float4 atmo = _AtmoColor;
                   // atmo.a = pow(1.0-saturate(dot(viewdir, i.normal)), _FalloffPlanet);
                   // atmo.a *= _TransparencyPlanet*_Color;
 
                   // float4 color = tex2D(_MainTex, i.texcoord)*_Color;
                    //color.rgb = lerp(color.rgb, atmo.rgb, atmo.a);
 
                   // return color*dot(_WorldSpaceLightPos0, i.normal);

                   fixed alpha=_Strength*pow(abs(sin(i.dotProduct.x*radians(_Degrees)+_Step)),_Pow);
                   fixed4 color=_AtmoColor;
                   color.w=alpha;
                   return color;
            }
            ENDCG
        }
    }
}
