using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
[ExecuteAlways]
public class ResponsiveUIController : MonoBehaviour
{
    [Header("Design Reference")]
    [SerializeField] private Vector2 designResolution = new Vector2(1920f, 3413.333f); // 9:16 portrait design ratio

    private CanvasScaler _canvasScaler;
    private int _lastScreenWidth = -1;
    private int _lastScreenHeight = -1;

    public float TargetAspectRatio => designResolution.x / designResolution.y;

    private void Awake()
    {
        _canvasScaler = GetComponent<CanvasScaler>();
        ApplyResponsiveScaling();
    }

    private void OnEnable()
    {
        ApplyResponsiveScaling();
    }

    private void Update()
    {
        if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
        {
            ApplyResponsiveScaling();
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplyResponsiveScaling();
    }

    public void ApplyResponsiveScaling()
    {
        if (_canvasScaler == null)
            _canvasScaler = GetComponent<CanvasScaler>();

        if (_canvasScaler == null)
            return;

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        if (screenWidth <= 0 || screenHeight <= 0)
            return;

        _lastScreenWidth = screenWidth;
        _lastScreenHeight = screenHeight;

        _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _canvasScaler.referenceResolution = designResolution;
        _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        float currentAspectRatio = (float)screenWidth / screenHeight;
        float targetAspect = TargetAspectRatio;

        // If screen is wider than design aspect ratio (e.g. 16:9 landscape, 4:3 tablet):
        // Match Height (1.0f) to ensure top and bottom are never cut off.
        // If screen is taller/narrower than design aspect ratio (e.g. 9:19.5, 9:20 phone):
        // Match Width (0.0f) to ensure left and right are never cut off.
        if (currentAspectRatio >= targetAspect)
        {
            _canvasScaler.matchWidthOrHeight = 1f;
        }
        else
        {
            _canvasScaler.matchWidthOrHeight = 0f;
        }
    }
}
