using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color Color;

    private TutorialController tutController;
    private Animator m_Animator;
    private Image m_Image;

    public void Awake()
    {
        m_Image = GetComponent<Image>();
        if (m_Image != null)
            m_Image.color = Color;

        tutController = FindObjectOfType<TutorialController>();
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (tutController != null)
            tutController.StartTutorial();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ChangeCurrentColor(Color);

        if (tutController != null)
            tutController.pickedColor = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_Animator != null)
            m_Animator.SetBool("Selected", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_Animator != null)
            m_Animator.SetBool("Selected", false);
    }
}
