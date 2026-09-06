using UnityEngine;
using UnityEngine.UI;

public class PaintBall : MonoBehaviour
{
    private Paintable _parent;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetParent(Paintable p)
    {
        _parent = p;

        GameManager gm = GameManager.Instance;
        if (gm != null && gm.CurrentColor.HasValue && _image != null)
        {
            _image.color = gm.CurrentColor.Value;
        }
    }

    public void HitCanvas()
    {
        if (_parent != null)
        {
            _parent.HitBall();
        }
        Destroy(gameObject);
    }
}
