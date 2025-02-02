Shader "Editions/Negative"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "UnitySprites.cginc"

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color;
        };

        void vert (inout appdata_full v, out Input o)
        {
            v.vertex.xy *= _Flip.xy;

            #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap (v.vertex);
            #endif

            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color * _RendererColor;
        }

        float3 RGBtoHSL(float3 c)
        {
            float minVal = min(c.r, min(c.g, c.b));
            float maxVal = max(c.r, max(c.g, c.b));
            float delta = maxVal - minVal;
        
            float h = 0.0;
            float s = 0.0;
            float l = (maxVal + minVal) * 0.5;
        
            if (delta > 0.0001) {
                s = (l < 0.5) ? (delta / (maxVal + minVal)) : (delta / (2.0 - maxVal - minVal));
        
                if (maxVal == c.r) {
                    h = (c.g - c.b) / delta + (c.g < c.b ? 6.0 : 0.0);
                } else if (maxVal == c.g) {
                    h = (c.b - c.r) / delta + 2.0;
                } else {
                    h = (c.r - c.g) / delta + 4.0;
                }
                h /= 6.0;
            }
            return float3(h, s, l);
        }

        float3 HSLtoRGB(float3 hsl)
        {
            float h = hsl.x;
            float s = hsl.y;
            float l = hsl.z;
        
            float r, g, b;
            if (s == 0.0) {
                r = g = b = l;
            } else {
                float q = (l < 0.5) ? (l * (1.0 + s)) : (l + s - l * s);
                float p = 2.0 * l - q;
                float3 t = float3(h + 1.0 / 3.0, h, h - 1.0 / 3.0);
        
                for (int i = 0; i < 3; i++) {
                    if (t[i] < 0.0) t[i] += 1.0;
                    if (t[i] > 1.0) t[i] -= 1.0;
        
                    if (t[i] < 1.0 / 6.0) {
                        t[i] = p + (q - p) * 6.0 * t[i];
                    } else if (t[i] < 0.5) {
                        t[i] = q;
                    } else if (t[i] < 2.0 / 3.0) {
                        t[i] = p + (q - p) * (2.0 / 3.0 - t[i]) * 6.0;
                    } else {
                        t[i] = p;
                    }
                }
                r = t.x;
                g = t.y;
                b = t.z;
            }
            return float3(r, g, b);
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
            
            float3 hsl = RGBtoHSL(c.rgb);

            hsl.z = 1.0 - hsl.z;
            hsl.x = -hsl.x + 0.2;

            c.rgb = HSLtoRGB(hsl);

            o.Albedo = c.rgb * c.a;
            o.Alpha = c.a;
        }
        ENDCG
    }

Fallback "Sprites/Diffuse"
}
