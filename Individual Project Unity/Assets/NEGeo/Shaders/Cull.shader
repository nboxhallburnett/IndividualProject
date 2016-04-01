Shader "NEGeo/Cull" {
	SubShader
	{
		Tags{ "Queue" = "Geometry-1" }

		Lighting Off

		Pass

	{
		ZWrite On
		Cull Front
	}
	}
}
