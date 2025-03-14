using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class AlphaHitTestFilter : MonoBehaviour, ICanvasRaycastFilter
{
    [Tooltip("Minimum alpha value (0 to 1) required to register a click.")]
    [Range(0, 1)]
    public float alphaThreshold = 0.1f;

    private Image image;
    private Sprite sprite;
    private Texture2D texture;
    private Rect spriteRect;

    void Awake()
    {
        image = GetComponent<Image>();
        sprite = image.sprite;
        if (sprite != null)
        {
            texture = sprite.texture;
            spriteRect = sprite.textureRect;
        }
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (texture == null)
            return true;

        RectTransform rt = image.rectTransform;
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPoint, eventCamera, out localPoint))
            return true;

        Rect rect = rt.rect;
        float normX = (localPoint.x - rect.x) / rect.width;
        float normY = (localPoint.y - rect.y) / rect.height;

        int texX = Mathf.FloorToInt(spriteRect.x + spriteRect.width * normX);
        int texY = Mathf.FloorToInt(spriteRect.y + spriteRect.height * normY);

        if (texX < 0 || texX >= texture.width || texY < 0 || texY >= texture.height)
            return false;

        Color pixelColor = texture.GetPixel(texX, texY);
        return pixelColor.a >= alphaThreshold;
    }
}
