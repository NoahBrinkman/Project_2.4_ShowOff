﻿﻿Shader "Custom/Portal"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off
   
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            
            struct appdata
            {
                float4 vertex : POSITION;
            };

            sampler2D _MainTex;
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target{
                float2 screenSpaceUV = i.screenPos.xy / i.screenPos.w;
                return tex2D(_MainTex, screenSpaceUV);
            }
            
            ENDCG
        }
    }
    Fallback "Standard" // for shadows
}