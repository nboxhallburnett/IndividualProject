﻿using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour {

    public Material EffectMaterial;

    void OnRenderImage (RenderTexture src, RenderTexture dst) {
        Graphics.Blit(src, dst, EffectMaterial);
    }
}
