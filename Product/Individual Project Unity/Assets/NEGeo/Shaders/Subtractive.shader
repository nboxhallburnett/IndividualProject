Shader "NEGeo/Subtractive" {
	SubShader{
		// Render just before the Overlay layer, to make sure it is rendered on top of other objects
		Tags{ "Queue" = "Overlay-1" }

		// Don't draw in the RGBA channels
		ColorMask 0
		// Only write to the Z-Buffer
		ZWrite On

		// No need to do any additional processing in the pass
		Pass { }
	}
}