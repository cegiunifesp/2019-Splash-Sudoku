using UnityEngine;
using UnityEngine.UI;

public class SplashHit : MonoBehaviour
{
    private Paintable _parent;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
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

    public void SetParent(Paintable p)
    {
        _parent = p;

        GameManager gm = GameManager.Instance;
        if (gm != null && gm.CurrentColor.HasValue && _image != null)
        {
            _image.color = gm.CurrentColor.Value;
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void CallParent()
    {
        if (_parent != null)
        {
            _parent.Splash();
        }
    }
}
