
using UnityEngine;

public class Keycap : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private SpriteRenderer visualRenderer;
    [SerializeField] private Transform visual;
    [SerializeField] private MonitorController monitorController;
    [SerializeField] private BuddyController buddyController;

    [SerializeField] private float offset = 0.02f;
    [SerializeField] private float darker = 0.8f;

    private bool pressed;

    private Vector3 defaultPosition;
    private Color ogColor;

    private void Awake()
    {
        defaultPosition = visual.localPosition;
        ogColor = visualRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pressed || !buddyController.IsGrounded)
            return;

        
        
        
        PressKey();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!buddyController.IsGrounded)
        {
            pressed = false;
            visual.localPosition = defaultPosition;
            visualRenderer.color = ogColor;
            return;
        }
    
        if (pressed)
            return;

        
        
        PressKey();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        pressed = false;
        visual.localPosition = defaultPosition;
        visualRenderer.color = ogColor;
    }

    private void PressKey()
    {
        pressed = true;
        visual.localPosition = defaultPosition + Vector3.down * offset;
        visualRenderer.color = ogColor * darker;
        monitorController.KeyPressed(key);
    }
}
