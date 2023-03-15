using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteData
{
    public string name;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float pivotX;
    public float pivotY;
    public byte[] data;

    // Convert a Sprite to an instance of SpriteData
    public static SpriteData FromSprite(Sprite sprite)
    {
        SpriteData result = new SpriteData();
        result.name = sprite.name;
        result.xMin = sprite.rect.xMin;
        result.xMax = sprite.rect.xMax;
        result.yMin = sprite.rect.yMin;
        result.yMax = sprite.rect.yMax;
        result.pivotX = sprite.pivot.x;
        result.pivotY = sprite.pivot.y;

        // Ensure that the texture is decompressed before serialising
        result.data = DeCompress(sprite.texture).EncodeToPNG();

        return result;
    }

    // Convert SpriteData to a Sprite
    public static Sprite ToSprite(SpriteData data)
    {
        Rect rect = new Rect();
        rect.xMin = data.xMin;
        rect.xMax = data.xMax;
        rect.yMin = data.yMin;
        rect.yMax = data.yMax;

        Vector2 pivot = new Vector2();
        pivot.x = data.pivotX;
        pivot.y = data.pivotY;

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(data.data);

        Sprite result = Sprite.Create(texture, rect, pivot);
        result.name = data.name;

        return result;
    }

    public static Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
