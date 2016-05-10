using UnityEngine;
using System.Collections;
using ResetCore.Asset;

public class SpriteHelper {

    public static Sprite GetSprite(string spriteName, Rect rect = default(Rect), Vector2 pivot = default(Vector2))
    {
        Texture2D texture = ResourcesLoaderHelper.Instance.LoadResource<Texture2D>(spriteName);
        
        Rect finRect = rect;
        Vector2 finPivot = pivot;

        if (rect == default(Rect))
        {
            finRect = new Rect(0, 0, texture.width, texture.height);
        }
        if (pivot == default(Vector2))
        {
            finPivot = new Vector2(0.5f, 0.5f);
        }

        Sprite sprite = Sprite.Create(texture, finRect, finPivot);
        return sprite;
    }
}
