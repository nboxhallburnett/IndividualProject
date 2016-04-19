Shader "NEGeo/Subtractive" {
	SubShader{
		Tags{ "Queue" = "Overlay" }

		// Don't draw in the RGBA channels; just the depth buffer
		ColorMask 0
		ZWrite On

		Pass { }
	}
}