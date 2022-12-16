Shader "Custom/S_Painting   "
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AnimationSpeed ("Animation Speed", Range(0, 1)) = 0.5
    }

    SubShader
    {
        // This subshader is always used
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _AnimationSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the main texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Generate a random color
                float3 randomColor = frac(sin(_Time.y * _AnimationSpeed) * 43758.5453);
                fixed4 randomCol = fixed4(randomColor, 1);

                // Combine the original color with the random color
                col = lerp(col, randomCol, 0.5);

                return col;
            }
            ENDCG
        }
    }
}