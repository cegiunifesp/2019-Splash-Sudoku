using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorMessage : MonoBehaviour
{
    private bool _show;
    private float _duration = 1f;
    private float _cur_time;

    public Image fundo;
    public TextMeshProUGUI texto;

    private void Start()
    {
        _cur_time = 0f;
        SetAlpha(0f);
    }

    private void Update()
    {
        if (_show && _cur_time < _duration)
        {
            _cur_time += Time.deltaTime;
            SetAlpha(Mathf.Clamp01(_cur_time / _duration));
        }
        else if (_show && _cur_time >= _duration)
        {
            _cur_time += Time.deltaTime;
            if (_cur_time > 4f * _duration)
            {
                _show = false;
                _cur_time = _duration;
            }
        }
        else if (!_show && _cur_time > 0f)
        {
            _cur_time -= Time.deltaTime;
            SetAlpha(Mathf.Clamp01(_cur_time / _duration));
        }
    }

    private void SetAlpha(float alpha)
    {
        if (fundo != null)
        {
            Color c = fundo.color;
            fundo.color = new Color(c.r, c.g, c.b, alpha);
        }

        if (texto != null)
        {
            Color c = texto.color;
            texto.color = new Color(c.r, c.g, c.b, alpha);
        }
    }

    public void Show()
    {
        _show = true;
    }
}
