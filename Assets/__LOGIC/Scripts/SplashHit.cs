using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashHit : MonoBehaviour {

    private Paintable _parent;


    void Start()
    {
        GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().Play();
        transform.Rotate(new Vector3(0,0,Random.Range(-100,100)));
    }

    public void SetParent(Paintable p)
    {
        _parent = p;

        GameManager gm = GameManager.Instance;
        Color? currentColor = gm.CurrentColor;
        if (currentColor.HasValue)
        {
            this.GetComponent<Image>().color = currentColor.Value;
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void CallParent()
    {
        _parent.Splash();
    }
}
