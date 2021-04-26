using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color Color;

    private TutorialController tutController;
    private Animator m_Animator;

    public void Awake()
    {
        this.GetComponent<Image>().color = Color;
        tutController = FindObjectOfType<TutorialController>();
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        tutController.StartTutorial();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.ChangeCurrentColor(Color);
        tutController.pickedColor = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Animator.SetBool("Selected", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Animator.SetBool("Selected", false);
    }
}
