using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D),typeof(SpriteRenderer))]
public class SpriteButton : MonoBehaviour
{
    public UnityAction<SpriteButton> OnClicked ;
    public UnityAction<SpriteButton> OnMouseHover;
    public UnityAction<SpriteButton> OnMouseLeave;

    public Color HoverColorMultiplier =  new Color(.8f,.8f,.8f);
    public Color PressedColorMultiplier = Color.gray;

    private SpriteRenderer _sprite;
    private Color _originalColor;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _originalColor = _sprite.color;
    }

    private void OnEnable()
    {
        if (_sprite != null)
            _sprite.color = _originalColor;
    }

    private void OnMouseDown()
    {
        _sprite.color = _originalColor * PressedColorMultiplier;
        OnClicked?.Invoke(this);
    }

    private void OnMouseUp()
    {
        _sprite.color = _originalColor;
    }

    private void OnMouseEnter()
    {
        _sprite.color = _originalColor * HoverColorMultiplier;
        OnMouseHover?.Invoke(this);
    }
    private void OnMouseExit()
    {
        _sprite.color = _originalColor;
        OnMouseLeave?.Invoke(this);
    }
}
