using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour {

    private bool _show;
    private float _duration = 1;
    private float _cur_time;

    public Image fundo;
    public Text texto;

	// Use this for initialization
	void Start () {

        _cur_time = 0;
		
	}
	
	// Update is called once per frame
	void Update () {


        if ((_show) && (_cur_time < _duration))
        {
            _cur_time += Time.deltaTime;

            _paintChildren(_cur_time / _duration);


        }
        else if ((_show) && (_cur_time >= _duration))
        {
            _cur_time += Time.deltaTime;

            if (_cur_time > 4 * _duration)
            {
                _show = false;
                _cur_time = _duration;
            }
        }
        else if ((!_show) && (_cur_time > 0))
        {
            _cur_time -= Time.deltaTime;
            _paintChildren(_cur_time / _duration);
        }


    }

    void _paintChildren(float alpha)
    {
        fundo.color = new Color(fundo.color.r, fundo.color.g, fundo.color.b,alpha);
        texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, alpha);

    }

    public void Show()
    {
        _show = true;
    }
}
