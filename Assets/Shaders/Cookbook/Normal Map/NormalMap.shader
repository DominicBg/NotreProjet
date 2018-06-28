
Shader "MaximeTest/Cookbook/NormalMap" {
	Properties {
		_MainTint ("Diffuse Tint", Color) = (1,1,1,1)
		_NormalTex ("Normal Map", 2D) = "bump" {}
		_NormalMapIntensity("Normal intensity", Range(0,1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float2 uv_NormalTex;
		};

		sampler2D _NormalTex;
		fixed4 _MainTint;
		float _NormalMapIntensity;

		void surf (Input IN, inout SurfaceOutput o) {

			float3 n = UnpackNormal (tex2D(_NormalTex, IN.uv_NormalTex));
			n.x *= _NormalMapIntensity;
			n.y *= _NormalMapIntensity;

			o.Albedo = _MainTint;
			o.Normal = normalize(n);

		}

		ENDCG
	}
	FallBack "Diffuse"
}

