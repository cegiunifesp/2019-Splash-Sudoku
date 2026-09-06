using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using TMPro;

public class ErrorMessage : MonoBehaviour
{
    private bool _isVisible;
    private float _duration = 1f;
    private float _currentTime;

    [FormerlySerializedAs("fundo")]
    public Image backgroundImage;

    [FormerlySerializedAs("texto")]
    public TextMeshProUGUI messageText;

    // Backward compatibility properties
    public Image fundo => backgroundImage;
    public TextMeshProUGUI texto => messageText;

    private void Start()
    {
        _currentTime = 0f;
        SetAlpha(0f);
    }

    private void Update()
    {
        if (_isVisible && _currentTime < _duration)
        {
            _currentTime += Time.deltaTime;
            SetAlpha(Mathf.Clamp01(_currentTime / _duration));
        }
        else if (_isVisible && _currentTime >= _duration)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > 4f * _duration)
            {
                _isVisible = false;
                _currentTime = _duration;
            }
        }
        else if (!_isVisible && _currentTime > 0f)
        {
            _currentTime -= Time.deltaTime;
            SetAlpha(Mathf.Clamp01(_currentTime / _duration));
        }
    }

    private void SetAlpha(float alpha)
    {
        if (backgroundImage != null)
        {
            Color c = backgroundImage.color;
            backgroundImage.color = new Color(c.r, c.g, c.b, alpha);
        }

        if (messageText != null)
        {
            Color c = messageText.color;
            messageText.color = new Color(c.r, c.g, c.b, alpha);
        }
    }

    public void Show()
    {
        _isVisible = true;
    }
}
