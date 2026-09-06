using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private const int GridSize = 5;
    private const int GridCellCount = GridSize * GridSize;

    private static readonly int[][] DiagonalGroups = new int[][]
    {
        // Right diagonals
        new[] { 15, 21 },
        new[] { 10, 16, 22 },
        new[] { 5, 11, 17, 23 },
        new[] { 0, 6, 12, 18, 24 },
        new[] { 1, 7, 13, 19 },
        new[] { 2, 8, 14 },
        new[] { 3, 9 },
        // Left diagonals
        new[] { 1, 5 },
        new[] { 2, 6, 10 },
        new[] { 3, 7, 11, 15 },
        new[] { 4, 8, 12, 16, 20 },
        new[] { 9, 13, 17, 21 },
        new[] { 14, 18, 22 },
        new[] { 19, 23 }
    };

    public static GameManager Instance { get; private set; }

    [Header("Scenes")]
    [FormerlySerializedAs("Menu")]
    public GameObject menuScene;

    [FormerlySerializedAs("Game")]
    public GameObject gameScene;

    [FormerlySerializedAs("Score")]
    public GameObject scoreScene;

    [FormerlySerializedAs("Credits")]
    public GameObject creditsScene;

    [FormerlySerializedAs("OutBase")]
    public GameObject outBase;

    [FormerlySerializedAs("AddScore")]
    public GameObject addScorePanel;

    [FormerlySerializedAs("Canvas")]
    public GameObject canvasRoot;

    // Backward compatibility scene references
    public GameObject Menu => menuScene;
    public GameObject Game => gameScene;
    public GameObject Score => scoreScene;
    public GameObject Credits => creditsScene;
    public GameObject AddScore => addScorePanel;
    public GameObject Canvas => canvasRoot;

    [Header("Buttons")]
    [FormerlySerializedAs("QuitButton")]
    public GameObject quitButton;

    [FormerlySerializedAs("BackButton")]
    public GameObject backButton;

    [FormerlySerializedAs("BackButton2")]
    public GameObject backButton2;

    public GameObject QuitButton => quitButton;
    public GameObject BackButton => backButton;
    public GameObject BackButton2 => backButton2;

    [Header("InGame UI")]
    [FormerlySerializedAs("Relogio")]
    public TextMeshProUGUI timerText;

    public Text timerLegacyText;

    [FormerlySerializedAs("Paleta")]
    public Animator paletteAnimator;

    [FormerlySerializedAs("Brush")]
    public Image brushImage;

    [FormerlySerializedAs("Paused")]
    public bool isPaused = false;

    [FormerlySerializedAs("Erro")]
    public ErrorMessage errorMessage;

    public TextMeshProUGUI Relogio => timerText;
    public Animator Paleta => paletteAnimator;
    public Image Brush => brushImage;
    public bool Paused { get => isPaused; set => isPaused = value; }
    public bool IsPaused => isPaused;
    public ErrorMessage Erro => errorMessage;

    [Header("Grid")]
    [FormerlySerializedAs("Images")]
    public List<Paintable> gridCells;

    public List<Paintable> Images => gridCells;

    [Header("Colors")]
    [FormerlySerializedAs("Colors")]
    public List<Color> colorPalette;

    public List<Color> Colors => colorPalette;

    [Header("AddScore")]
    [FormerlySerializedAs("Nome")]
    public Text nameLegacyText;

    public TextMeshProUGUI nameTMP;

    [FormerlySerializedAs("ScoreText")]
    public TextMeshProUGUI scoreText;

    public Text Nome => nameLegacyText;
    public TextMeshProUGUI ScoreText => scoreText;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent StartGameEvent;
    public UnityEngine.Events.UnityEvent StopGameEvent;

    public Color? CurrentColor { get; set; } = null;

    private float _elapsedTime = 0f;
    private bool _isTimerRunning = false;
    private SceneMovement _sceneMovement = null;
    private readonly HashSet<Color> _colorCheckBuffer = new HashSet<Color>();

    public void Awake()
    {
        Instance = this;
        Hide(gameScene);
        Hide(scoreScene);
        Hide(creditsScene);
        
        if (menuScene != null) menuScene.SetActive(true);
        if (addScorePanel != null) addScorePanel.SetActive(false);
        if (backButton != null) backButton.SetActive(false);
        if (backButton2 != null) backButton2.SetActive(false);

        SetCanvasGroupInteractable(menuScene, true);
        SetCanvasGroupInteractable(gameScene, false);
        SetCanvasGroupInteractable(scoreScene, false);
        SetCanvasGroupInteractable(creditsScene, false);
    }

    private void SetCanvasGroupInteractable(GameObject target, bool interactable)
    {
        if (target != null)
        {
            var cg = target.GetComponent<CanvasGroup>();
            if (cg != null)
                cg.interactable = interactable;
        }
    }

    private void UpdateTimerDisplay(string value)
    {
        if (timerText != null)
            timerText.text = value;
        if (timerLegacyText != null)
            timerLegacyText.text = value;
    }

    public void StartGame()
    {
        _elapsedTime = 0f;
        UpdateTimerDisplay("0");
        ChangeCurrentColor(Color.white);
        isPaused = false;

        TutorialController tut = FindObjectOfType<TutorialController>();
        if (tut != null && tut.IsTutorialActive)
        {
            _isTimerRunning = false;
        }
        else
        {
            _isTimerRunning = true;
        }
    }

    public void StartTimer()
    {
        _elapsedTime = 0f;
        UpdateTimerDisplay("0");
        _isTimerRunning = true;
    }

    public void StopTimer()
    {
        _isTimerRunning = false;
    }

    public void StopGame()
    {
        _elapsedTime = 0f;
        _isTimerRunning = false;
        UpdateTimerDisplay("0");
        if (paletteAnimator != null) paletteAnimator.enabled = true;
    }

    public void ResetGame()
    {
        ClearAll();
        UpdateTimerDisplay("0");
        _elapsedTime = 0f;
        _isTimerRunning = false;
        ChangeCurrentColor(Color.white);
    }

    public void CheckColors()
    {
        if (HasWhite())
            return;

        CheckResult result = CheckAll();
        if (result.IsSuccessful)
        {
            _isTimerRunning = false;
            int elapsedSeconds = Mathf.FloorToInt(_elapsedTime);
            if (scoreText != null)
                scoreText.text = $"{elapsedSeconds} segundos";
            if (addScorePanel != null)
                addScorePanel.SetActive(true);
            isPaused = true;
        }
        else
        {
            foreach (int id in result.Errors)
            {
                if (id >= 0 && id < gridCells.Count && gridCells[id] != null)
                    gridCells[id].ShowErrorFeedback();
            }
            if (errorMessage != null)
                errorMessage.Show();
        }
    }

    public void AddNewScore()
    {
        string playerName = string.Empty;
        if (nameTMP != null && !string.IsNullOrWhiteSpace(nameTMP.text))
            playerName = nameTMP.text;
        else if (nameLegacyText != null && !string.IsNullOrWhiteSpace(nameLegacyText.text))
            playerName = nameLegacyText.text;

        if (!string.IsNullOrWhiteSpace(playerName))
        {
            NetworkedScore.Instance?.PushScore(playerName, Mathf.FloorToInt(_elapsedTime));
        }
        CloseAddScore();
    }

    public void CloseAddScore()
    {
        if (addScorePanel != null)
            addScorePanel.SetActive(false);
        GoTo(menuScene);
    }

    private bool HasWhite()
    {
        if (gridCells == null) return false;

        for (int i = 0; i < gridCells.Count; i++)
        {
            var paintable = gridCells[i];
            if (paintable != null && paintable.color == Color.white)
                return true;
        }
        return false;
    }

    private CheckResult CheckAll()
    {
        HashSet<int> errors = new HashSet<int>();
        for (int i = 0; i < GridSize; i++)
        {
            CheckRow(i, errors);
            CheckColumn(i, errors);
        }

        for (int i = 0; i < DiagonalGroups.Length; i++)
        {
            CheckGroup(DiagonalGroups[i], errors);
        }

        return new CheckResult(errors);
    }

    private void CheckRow(int row, HashSet<int> errors)
    {
        _colorCheckBuffer.Clear();
        int start = row * GridSize;
        int end = start + GridSize;
        for (int i = start; i < end; i++)
        {
            if (gridCells == null || i >= gridCells.Count || gridCells[i] == null) continue;
            Paintable image = gridCells[i];
            if (!_colorCheckBuffer.Add(image.color))
                errors.Add(i);
        }
    }

    private void CheckColumn(int column, HashSet<int> errors)
    {
        _colorCheckBuffer.Clear();
        for (int i = column; i < GridCellCount; i += GridSize)
        {
            if (gridCells == null || i >= gridCells.Count || gridCells[i] == null) continue;
            Paintable image = gridCells[i];
            if (!_colorCheckBuffer.Add(image.color))
                errors.Add(i);
        }
    }

    private void CheckGroup(int[] arr, HashSet<int> errors)
    {
        _colorCheckBuffer.Clear();
        for (int k = 0; k < arr.Length; k++)
        {
            int n = arr[k];
            if (gridCells == null || n >= gridCells.Count || gridCells[n] == null) continue;
            Paintable image = gridCells[n];
            if (!_colorCheckBuffer.Add(image.color))
                errors.Add(n);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMenu() => GoTo(menuScene);
    public void GoToGame() => GoTo(gameScene);
    public void GoToScore() => GoTo(scoreScene);
    public void GoToCredits() => GoTo(creditsScene);

    public void GoTo(GameObject scene)
    {
        if (scene == null) return;

        GameObject current = GetCurrentScene();
        if (current == scene)
            return;

        if (current == gameScene)
        {
            StopGameEvent?.Invoke();
            ResetGame();
        }
        else if (scene == gameScene)
        {
            StartGameEvent?.Invoke();
        }

        if (backButton != null) backButton.SetActive(false);
        if (backButton2 != null) backButton2.SetActive(false);
        if (quitButton != null) quitButton.SetActive(false);
        
        scene.SetActive(true);
        Fade(current, scene, canvasRoot);
    }

    private void Fade(GameObject sceneToHide, GameObject sceneToShow, GameObject allScenes)
    {
        if (sceneToHide == null || sceneToShow == null || allScenes == null)
            return;
        _sceneMovement = new SceneMovement(sceneToHide, sceneToShow, allScenes);
    }

    private GameObject GetCurrentScene()
    {
        if (menuScene != null && menuScene.activeSelf)
            return menuScene;
        if (gameScene != null && gameScene.activeSelf)
            return gameScene;
        if (scoreScene != null && scoreScene.activeSelf)
            return scoreScene;
        return creditsScene;
    }

    private void Hide(GameObject target)
    {
        if (target != null)
            target.SetActive(false);
    }

    public void Update()
    {
        if (!isPaused && _isTimerRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerDisplay($"{Mathf.FloorToInt(_elapsedTime)}");
        }

        if (_sceneMovement != null)
        {
            _sceneMovement.CurTime += Time.deltaTime;
            float t = Mathf.Clamp01(_sceneMovement.CurTime / _sceneMovement.TotalTime);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            if (_sceneMovement.ScenesTransform != null)
            {
                _sceneMovement.ScenesTransform.position = Vector3.Lerp(
                    _sceneMovement.StartPosition,
                    _sceneMovement.TargetPosition,
                    smoothT
                );
            }

            if (_sceneMovement.CurTime >= _sceneMovement.TotalTime)
            {
                if (_sceneMovement.ScenesTransform != null)
                    _sceneMovement.ScenesTransform.position = _sceneMovement.TargetPosition;

                SetCanvasGroupInteractable(_sceneMovement.NextScene, true);

                if (_sceneMovement.NextScene == menuScene)
                {
                    if (quitButton != null) quitButton.SetActive(true);
                }
                else if (_sceneMovement.NextScene == scoreScene)
                {
                    if (backButton2 != null) backButton2.SetActive(true);
                }
                else
                {
                    if (backButton != null) backButton.SetActive(true);
                }

                Hide(_sceneMovement.CurrentScene);
                _sceneMovement.NextScene?.GetComponent<ISceneManager>()?.Ready();
                _sceneMovement = null;
            }
        }
    }

    public class CheckResult
    {
        public bool IsSuccessful => Errors.Count == 0;
        public HashSet<int> Errors { get; }

        public CheckResult(HashSet<int> errors)
        {
            Errors = errors;
        }
    }

    public void ChangeCurrentColor(Color newColor)
    {
        if (brushImage != null)
            brushImage.color = newColor;
        CurrentColor = newColor;
    }

    public void ClearAll()
    {
        if (gridCells == null) return;

        for (int i = 0; i < gridCells.Count; i++)
        {
            if (gridCells[i] != null)
                gridCells[i].Clear();
        }
    }

    public class SceneMovement
    {
        public GameObject CurrentScene { get; }
        public GameObject NextScene { get; }
        public GameObject AllScene { get; }

        public Transform ShowTransform => NextScene != null ? NextScene.transform : null;
        public Transform HideTransform => CurrentScene != null ? CurrentScene.transform : null;
        public Transform ScenesTransform => AllScene != null ? AllScene.transform : null;

        public Vector3 StartPosition { get; }
        public Vector3 TargetPosition { get; }
        public float TotalTime { get; }
        public float CurTime { get; set; }

        public SceneMovement(GameObject currentScene, GameObject nextScene, GameObject allScenes)
        {
            CurrentScene = currentScene;
            NextScene = nextScene;
            AllScene = allScenes;

            if (ScenesTransform != null && ShowTransform != null && HideTransform != null)
            {
                Vector3 displacement = ShowTransform.position - HideTransform.position;
                StartPosition = ScenesTransform.position;
                TargetPosition = StartPosition - displacement;
            }
            else
            {
                StartPosition = Vector3.zero;
                TargetPosition = Vector3.zero;
            }

            TotalTime = 0.65f;
            CurTime = 0f;
        }
    }
}
