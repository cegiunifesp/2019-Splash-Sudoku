using UnityEngine;
using UnityEngine.UI;

public class SplashHit : MonoBehaviour
{
    private Paintable _parentPaintable;
    private Image _splashImage;

    private void Awake()
    {
        _splashImage = GetComponent<Image>();
    }

    private void Start()
    {
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null)
        {
            AudioSource audioSource = audioObj.GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.Play();
        }

        transform.Rotate(new Vector3(0, 0, Random.Range(-100f, 100f)));
    }

    public void SetParent(Paintable parent)
    {
        _parentPaintable = parent;

        GameManager gm = GameManager.Instance;
        if (gm != null && gm.CurrentColor.HasValue && _splashImage != null)
        {
            _splashImage.color = gm.CurrentColor.Value;
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void CallParent()
    {
        if (_parentPaintable != null)
        {
            _parentPaintable.Splash();
        }
    }
}
