Shader "Custom/Lesson7"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainMask ("MainMask (RGB)", 2D) = "black" {}
        _MainTex ("MainTex (RGB)", 2D) = "white" {}
        _MainTex1 ("MainTex1 (RGB)", 2D) = "white" {}
        _MainTex2 ("MainTex2 (RGB)", 2D) = "white" {}
        //_Glossiness ("Smoothness", Range(0,1)) = 0.5
        //_Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows
        #pragma surface surf Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainMask, _MainTex, _MainTex1, _MainTex2;

        struct Input
        {
            float2 uv_MainMask;
            float2 uv_MainTex;
        };

        //half _Glossiness;
        //half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
       // UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        //UNITY_INSTANCING_BUFFER_END(Props)

        
        //void surf (Input IN, inout SurfaceOutputStandard o)
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed3 mask= tex2D (_MainMask, IN.uv_MainMask);

            fixed3  col = tex2D (_MainTex, IN.uv_MainTex) * mask.b;
            col += tex2D (_MainTex1, IN.uv_MainTex) * mask.r;
            col += tex2D (_MainTex2, IN.uv_MainTex) * mask.g;

            o.Albedo = col;
            o.Emission=float3(0.5,0.0,0.0);
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            //o.Alpha = col.a;
            

        }
        ENDCG
    }
    FallBack "Diffuse"
}
