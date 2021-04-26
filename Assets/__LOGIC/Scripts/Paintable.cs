using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Paintable : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject paintBall;
    public GameObject splash;
    public GameObject cor;
    private SplashMark Mark { get { return cor.GetComponent<SplashMark>(); } }
    private Animator Animator { get { return this.GetComponent<Animator>(); } }
    public Sprite mancha;
    public Color color = Color.white;

    private Image m_Image;

    private TutorialController tutorial;

    private void Awake()
    {
        tutorial = FindObjectOfType<TutorialController>();
        m_Image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager gm = GameManager.Instance;

        if (gm.Paused)
            return;

        if (gm.CurrentColor.HasValue)
        {


            if (gm.CurrentColor == Color.white)
                Clear();
            else
            {
                transform.SetAsLastSibling();
                Instantiate(paintBall, transform).GetComponent<PaintBall>().SetParent(this);
                Animator.SetBool("Shake", false);
                tutorial.painted = true;
            }
        }
    }

    public void Blink(bool onOff)
    {

    }

    public void HitBall()
    {
        transform.SetAsLastSibling();
        Instantiate(splash, transform).GetComponent<SplashHit>().SetParent(this);
    }

    public void Splash()
    {
        GameManager gm = GameManager.Instance;
        Color? currentColor = gm.CurrentColor;
        if (currentColor.HasValue)
        {
            Mark.Splash(currentColor.Value);
            color = currentColor.Value;
            gm.CheckColors();
        }
    }

    public void Clear()
    {
        Mark.Clean();
        color = Color.white;
        Animator.SetBool("Shake", false);
    }


    public void IndicaErro()
    {
        Animator.SetBool("Shake", true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager gm = GameManager.Instance;

        if (gm.CurrentColor.HasValue)
        {
            m_Image.color = Color.Lerp(Color.white, (Color)gm.CurrentColor, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Image.color = Color.white;
    }
}
