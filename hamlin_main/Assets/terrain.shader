Shader "Custom/terrain" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float minHeight;
		float maxHeight;

		struct Input {
			float3 worldPos;
		};

		//called for every pixel where the mesh is visible
		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = float3(0,1,0); //turns entire mesh green
			
			
			// Albedo comes from a texture tinted by color
			// Metallic and smoothness come from slider variables
		}
		ENDCG
	}
	FallBack "Diffuse"
}
