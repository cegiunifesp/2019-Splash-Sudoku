using UnityEngine;

public class ColorWheel : MonoBehaviour
{
    private float _radius;

    private void Start()
    {
        float currentAngle = 0f;
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            _radius = Vector2.Distance(child.position, transform.position);

            child.position = transform.position + new Vector3(Mathf.Cos(currentAngle) * _radius, Mathf.Sin(currentAngle) * _radius, -0.01f);
            child.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * currentAngle - 90f);

            currentAngle += Mathf.PI / 4f;
        }
    }
}
