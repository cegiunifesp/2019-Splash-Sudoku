using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashMark : MonoBehaviour {


    private Animator Animator { get { return this.GetComponent<Animator>(); } }
    private UnityEngine.UI.Image Image { get { return this.GetComponent<UnityEngine.UI.Image>(); } }

    private void Start()
    {
        transform.localScale = new Vector3(0, 0, 1);
    }

    public void Clean()
    {
        Animator.SetBool("Show", false);
    }

    public void EndClean()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void Splash(Color c)
    {
        Image.color = c;
        transform.Rotate(new Vector3(0, 0, Random.Range(-100, 100)));
        Animator.SetBool("Show", true);
    }
}
