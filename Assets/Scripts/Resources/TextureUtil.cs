using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/ObelardO/Unity-Utils/blob/master/TextureAlphaPremultiply/TextureUtil.cs
public static class TextureUtil
{
    public static Texture2D PremultiplyAlpha(this Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Premultiply(pixels[i]);
        texture.SetPixels(pixels);
        return texture;
    }

    private static Color Premultiply(Color color)
    {
        return new Color(color.r * color.a, color.g * color.a, color.b * color.a, color.a);
    }

}
