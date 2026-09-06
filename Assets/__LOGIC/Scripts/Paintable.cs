using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Paintable : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [FormerlySerializedAs("paintBall")]
    public GameObject paintBallPrefab;

    [FormerlySerializedAs("splash")]
    public GameObject splashPrefab;

    [FormerlySerializedAs("cor")]
    public GameObject splashMarkObject;

    [FormerlySerializedAs("mancha")]
    public Sprite splashSprite;

    public Color color = Color.white;

    // Backward compatibility aliases
    public GameObject paintBall => paintBallPrefab;
    public GameObject splash => splashPrefab;
    public GameObject cor => splashMarkObject;
    public Sprite mancha => splashSprite;

    private SplashMark _splashMark;
    private Animator _animator;
    private Image _cellImage;
    private TutorialController _tutorialController;

    private void Awake()
    {
        _cellImage = GetComponent<Image>();
        _animator = GetComponent<Animator>();
        if (splashMarkObject != null)
        {
            _splashMark = splashMarkObject.GetComponent<SplashMark>();
        }
        _tutorialController = FindObjectOfType<TutorialController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || gm.IsPaused)
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
                if (paintBallPrefab != null)
                {
                    GameObject ballObj = Instantiate(paintBallPrefab, transform);
                    PaintBall ball = ballObj.GetComponent<PaintBall>();
                    if (ball != null)
                        ball.SetParent(this);
                }

                if (_animator != null)
                    _animator.SetBool("Shake", false);

                if (_tutorialController != null)
                    _tutorialController.painted = true;
            }
        }
    }

    public void HitBall()
    {
        transform.SetAsLastSibling();
        if (splashPrefab != null)
        {
            GameObject splashObj = Instantiate(splashPrefab, transform);
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
            if (_splashMark != null)
                _splashMark.Splash(currentColor.Value);

            color = currentColor.Value;
            gm.CheckColors();
        }
    }

    public void Clear()
    {
        if (_splashMark != null)
            _splashMark.Clear();

        color = Color.white;

        if (_animator != null)
            _animator.SetBool("Shake", false);
    }

    public void ShowErrorFeedback()
    {
        if (_animator != null)
            _animator.SetBool("Shake", true);
    }

    // Backward compatibility alias
    public void IndicaErro() => ShowErrorFeedback();

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager gm = GameManager.Instance;
        if (gm != null && gm.CurrentColor.HasValue && _cellImage != null)
        {
            _cellImage.color = Color.Lerp(Color.white, gm.CurrentColor.Value, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_cellImage != null)
            _cellImage.color = Color.white;
    }
}
