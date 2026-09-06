using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [FormerlySerializedAs("Color")]
    public Color pickerColor = Color.white;

    // Backward compatibility property
    public Color Color
    {
        get => pickerColor;
        set => pickerColor = value;
    }

    private TutorialController _tutorialController;
    private Animator _animator;
    private Image _pickerImage;

    public void Awake()
    {
        _pickerImage = GetComponent<Image>();
        if (_pickerImage != null)
            _pickerImage.color = pickerColor;

        _tutorialController = FindObjectOfType<TutorialController>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (_tutorialController != null)
            _tutorialController.StartTutorial();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ChangeCurrentColor(pickerColor);

        if (_tutorialController != null)
            _tutorialController.pickedColor = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_animator != null)
            _animator.SetBool("Selected", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_animator != null)
            _animator.SetBool("Selected", false);
    }
}
