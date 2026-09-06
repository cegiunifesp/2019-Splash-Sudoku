using UnityEngine;

public class ElasticEffect : MonoBehaviour
{
    private RectTransform _rectTransform;

    public Vector2 minSize;
    public Vector2 maxSize;
    public float speed = 0.05f;

    private Vector2 _target;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _target = minSize;
    }

    private void Update()
    {
        if (_rectTransform == null)
            return;

        _rectTransform.localScale = Vector3.MoveTowards(_rectTransform.localScale, _target, speed * Time.deltaTime);

        if (_rectTransform.localScale.x <= minSize.x)
            _target = maxSize;
        else if (_rectTransform.localScale.x >= maxSize.x)
            _target = minSize;
    }
}
