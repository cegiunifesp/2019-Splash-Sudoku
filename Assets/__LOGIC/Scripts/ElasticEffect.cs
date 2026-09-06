using UnityEngine;

public class ElasticEffect : MonoBehaviour
{
    private RectTransform _rectTransform;

    public Vector2 minSize;
    public Vector2 maxSize;
    public float speed = 0.05f;

    private Vector2 _targetScale;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _targetScale = minSize;
    }

    private void Update()
    {
        if (_rectTransform == null)
            return;

        _rectTransform.localScale = Vector3.MoveTowards(_rectTransform.localScale, _targetScale, speed * Time.deltaTime);

        if (_rectTransform.localScale.x <= minSize.x)
            _targetScale = maxSize;
        else if (_rectTransform.localScale.x >= maxSize.x)
            _targetScale = minSize;
    }
}
