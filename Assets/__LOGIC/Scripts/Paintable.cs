using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Paintable : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject paintBall;
    public GameObject splash;
    public GameObject cor;
    public Sprite mancha;
    public Color color = Color.white;

    private SplashMark m_Mark;
    private Animator m_Animator;
    private Image m_Image;
    private TutorialController m_Tutorial;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_Animator = GetComponent<Animator>();
        if (cor != null)
        {
            m_Mark = cor.GetComponent<SplashMark>();
        }
        m_Tutorial = FindObjectOfType<TutorialController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || gm.Paused)
            return;

        if (gm.CurrentColor.HasValue)
        {
            if (gm.CurrentColor.Value == Color.white)
            {
                Clear();
            }
            else
            {
                transform.SetAsLastSibling();
                if (paintBall != null)
                {
                    GameObject ballObj = Instantiate(paintBall, transform);
                    PaintBall ball = ballObj.GetComponent<PaintBall>();
                    if (ball != null)
                        ball.SetParent(this);
                }

                if (m_Animator != null)
                    m_Animator.SetBool("Shake", false);

                if (m_Tutorial != null)
                    m_Tutorial.painted = true;
            }
        }
    }

    public void Blink(bool onOff)
    {
    }

    public void HitBall()
    {
        transform.SetAsLastSibling();
        if (splash != null)
        {
            GameObject splashObj = Instantiate(splash, transform);
            SplashHit hit = splashObj.GetComponent<SplashHit>();
            if (hit != null)
                hit.SetParent(this);
        }
    }

    public void Splash()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null) return;

        Color? currentColor = gm.CurrentColor;
        if (currentColor.HasValue)
        {
            if (m_Mark != null)
                m_Mark.Splash(currentColor.Value);

            color = currentColor.Value;
            gm.CheckColors();
        }
    }

    public void Clear()
    {
        if (m_Mark != null)
            m_Mark.Clean();

        color = Color.white;

        if (m_Animator != null)
            m_Animator.SetBool("Shake", false);
    }

    public void IndicaErro()
    {
        if (m_Animator != null)
            m_Animator.SetBool("Shake", true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager gm = GameManager.Instance;
        if (gm != null && gm.CurrentColor.HasValue && m_Image != null)
        {
            m_Image.color = Color.Lerp(Color.white, gm.CurrentColor.Value, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_Image != null)
            m_Image.color = Color.white;
    }
}
