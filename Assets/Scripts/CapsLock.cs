using UnityEngine;

public class CapsLock : MonoBehaviour
{
    [SerializeField] private SpriteRenderer visualRenderer;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;

    public void UpdateVisual(bool capital)
    {
        visualRenderer.sprite = capital ? onSprite : offSprite;
    }
}