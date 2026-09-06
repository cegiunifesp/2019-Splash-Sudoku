using UnityEngine;
using UnityEngine.UI;

public class SplashMark : MonoBehaviour
{
    private Animator m_Animator;
    private Image m_Image;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Image = GetComponent<Image>();
    }

    private void Start()
    {
        transform.localScale = new Vector3(0, 0, 1);
    }

    public void Clean()
    {
        if (m_Animator != null)
            m_Animator.SetBool("Show", false);
    }

    public void EndClean()
    {
        transform.eulerAngles = Vector3.zero;
    }

    public void Splash(Color c)
    {
        if (m_Image != null)
            m_Image.color = c;

        transform.Rotate(new Vector3(0, 0, Random.Range(-100f, 100f)));

        if (m_Animator != null)
            m_Animator.SetBool("Show", true);
    }
}
