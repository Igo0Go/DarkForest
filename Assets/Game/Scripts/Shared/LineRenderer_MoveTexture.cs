using UnityEngine;

public class LineRenderer_MoveTexture : MonoBehaviour
{
    [SerializeField, Range(1, 5)]
    private float textureOffsetSpeed = 1;
    [SerializeField]
    private LineRenderer centerRenderer = null;
    [SerializeField]
    private LineRenderer outlineRenderer = null;

    private float textureOffset = 0;

    void Update()
    {
        MoveTexture();
    }

    private void MoveTexture()
    {
        textureOffset -= Time.deltaTime * textureOffsetSpeed;
        if (textureOffset < -10)
        {
            textureOffset += 10;
        }
        for (int i = 0; i < outlineRenderer.sharedMaterials.Length; i++)
        {
            outlineRenderer.sharedMaterials[i].SetTextureOffset("_MainTex", new Vector2(textureOffset, 0));
        }
    }
}
