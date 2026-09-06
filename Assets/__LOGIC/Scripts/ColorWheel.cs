using UnityEngine;

public class ColorWheel : MonoBehaviour
{
    private float _raio;

    private void Start()
    {
        float ang = 0f;
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            _raio = Vector2.Distance(child.position, transform.position);

            child.position = transform.position + new Vector3(Mathf.Cos(ang) * _raio, Mathf.Sin(ang) * _raio, -0.01f);
            child.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * ang - 90f);

            ang += Mathf.PI / 4f;
        }
    }
}
