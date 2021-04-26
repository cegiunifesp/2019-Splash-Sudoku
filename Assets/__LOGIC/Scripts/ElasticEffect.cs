using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticEffect : MonoBehaviour
{
    RectTransform rectTransform;

    public Vector2 minSize, maxSize;
    public float speed = 0.05f;
    private Vector2 target;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        target = minSize;
    }

    private void Update()
    {
        rectTransform.localScale = Vector3.MoveTowards(rectTransform.localScale, target, speed * Time.deltaTime);

        if (rectTransform.localScale.x <= minSize.x)
            target = maxSize;

        if (rectTransform.localScale.x >= maxSize.x)
            target = minSize;
    }
}
