using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintBall : MonoBehaviour
{
    private Paintable _parent;

    // Use this for initialization
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


    public void HitCanvas()
    {
        _parent.HitBall();
        Destroy(gameObject);
    }
}
