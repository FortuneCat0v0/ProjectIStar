Shader "ProjectIStar/Sprite Lit Cutout"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.1
        _Metallic ("Metallic", Range(0,1)) = 0
        _Smoothness ("Smoothness", Range(0,1)) = 0

        [MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0
        [HideInInspector] _RendererColor ("Renderer Color", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)

        // Used by SpriteRenderer when the platform stores alpha separately.
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
            "IgnoreProjector" = "True"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        ZWrite On

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows addshadow alphatest:_Cutoff vertex:vert nofog keepalpha noinstancing
        #pragma target 3.0
        #pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

        #include "UnitySprites.cginc"

        half _Metallic;
        half _Smoothness;

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color;
        };

        // SpriteRenderer supplies its current frame UVs through TEXCOORD0.
        // Pass them explicitly for dynamically generated sprite meshes.
        void vert(inout appdata_full vertex, out Input output)
        {
            UNITY_INITIALIZE_OUTPUT(Input, output);
            vertex.vertex = UnityFlipSprite(vertex.vertex, _Flip);
            output.uv_MainTex = vertex.texcoord.xy;
            output.color = vertex.color * _Color * _RendererColor;

            #ifdef PIXELSNAP_ON
            vertex.vertex = UnityPixelSnap(vertex.vertex);
            #endif
        }

        void surf(Input input, inout SurfaceOutputStandard output)
        {
            fixed4 color = SampleSpriteTexture(input.uv_MainTex) * input.color;

            output.Albedo = color.rgb;
            output.Alpha = color.a;
            output.Metallic = _Metallic;
            output.Smoothness = _Smoothness;
            output.Occlusion = 1.0;
        }
        ENDCG
    }

    FallBack "Transparent/Cutout/VertexLit"
}
