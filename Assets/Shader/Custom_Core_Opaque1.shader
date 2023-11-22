Shader "Custom/Core/Opaque1" {
	Properties {
		[NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
		_ShadowColor ("Shadow Color", Vector) = (0,0,0,0)
		_ShadeColor ("Shade Color", Vector) = (0.5,0.5,0.5,0.5)
		_ShadeMin ("Shade Min", Range(-1, 1)) = 0
		_ShadeMax ("Shade Max", Range(-1, 1)) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}