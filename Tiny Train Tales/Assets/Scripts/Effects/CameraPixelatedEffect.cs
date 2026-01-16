using UnityEngine;

[ExecuteInEditMode]
public class CameraPixelateEffect : MonoBehaviour
{
    public Material pixelMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pixelMaterial != null)
            Graphics.Blit(src, dest, pixelMaterial);
        else
            Graphics.Blit(src, dest);
    }
}
