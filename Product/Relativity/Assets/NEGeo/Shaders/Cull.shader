Shader "NEGeo/Cull" {
	SubShader
	{
		Tags{ "Queue" = "Overlay" }

		Lighting Off

		Pass

	{
		ZWrite On
		Cull Front
	}
	}
}
