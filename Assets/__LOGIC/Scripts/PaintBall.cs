using UnityEngine;
using UnityEngine.UI;

public class PaintBall : MonoBehaviour
{
    private Paintable _parentPaintable;
    private Image _ballImage;

    private void Awake()
    {
        _ballImage = GetComponent<Image>();
    }

    public void SetParent(Paintable parent)
    {
        _parentPaintable = parent;

        GameManager gm = GameManager.Instance;
        if (gm != null && gm.CurrentColor.HasValue && _ballImage != null)
        {
            _ballImage.color = gm.CurrentColor.Value;
        }
    }

    public void HitCanvas()
    {
        if (_parentPaintable != null)
        {
            _parentPaintable.HitBall();
        }
        Destroy(gameObject);
    }
}
