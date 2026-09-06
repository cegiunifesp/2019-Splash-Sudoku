using UnityEngine;
using UnityEngine.UI;

public class SplashMark : MonoBehaviour
{
    private Animator _animator;
    private Image _markImage;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _markImage = GetComponent<Image>();
    }

    private void Start()
    {
        transform.localScale = new Vector3(0, 0, 1);
    }

    public void Clear()
    {
        if (_animator != null)
            _animator.SetBool("Show", false);
    }

    public void ResetRotation()
    {
        transform.eulerAngles = Vector3.zero;
    }

    public void Splash(Color splashColor)
    {
        if (_markImage != null)
            _markImage.color = splashColor;

        transform.Rotate(new Vector3(0, 0, Random.Range(-100f, 100f)));

        if (_animator != null)
            _animator.SetBool("Show", true);
    }

    // Backward compatibility aliases
    public void Clean() => Clear();
    public void EndClean() => ResetRotation();
}
