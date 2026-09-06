using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int GridSize = 5;
    private const int GridCellCount = GridSize * GridSize;

    private static readonly int[][] DiagonalGroups = new int[][]
    {
        // Direita
        new[] { 15, 21 },
        new[] { 10, 16, 22 },
        new[] { 5, 11, 17, 23 },
        new[] { 0, 6, 12, 18, 24 },
        new[] { 1, 7, 13, 19 },
        new[] { 2, 8, 14 },
        new[] { 3, 9 },
        // Esquerda
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
    public GameObject Menu;
    public GameObject Game;
    public GameObject Score;
    public GameObject Credits;
    public GameObject OutBase;
    public GameObject AddScore;
    public GameObject Canvas;

    [Header("Buttons")]
    public GameObject QuitButton;
    public GameObject BackButton;
    public GameObject BackButton2;

    [Header("InGame")]
    public Text Relogio;
    public Animator Paleta;
    private float _time = 0;
    public Image Brush;
    public bool Paused = false;
    public ErrorMessage Erro;

    [Header("Grid")]
    public List<Paintable> Images;

    [Header("Colors")]
    public List<Color> Colors;

    [Header("AddScore")]
    public Text Nome;
    public Text ScoreText;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent StartGameEvent;
    public UnityEngine.Events.UnityEvent StopGameEvent;

    public Color? CurrentColor { get; set; } = null;

    private SceneMovement sceneMovement = null;
    private readonly HashSet<Color> _colorCheckBuffer = new HashSet<Color>();

    public void Awake()
    {
#if UNITY_STANDALONE && !UNITY_EDITOR
        Screen.SetResolution(360, 720, false);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        Screen.SetResolution(280, 560, false);
#endif

        Instance = this;
        Hide(Game);
        Hide(Score);
        Hide(Credits);
        
        if (Menu != null) Menu.SetActive(true);
        if (AddScore != null) AddScore.SetActive(false);
        if (BackButton != null) BackButton.SetActive(false);
        if (BackButton2 != null) BackButton2.SetActive(false);

        SetCanvasGroupInteractable(Menu, true);
        SetCanvasGroupInteractable(Game, false);
        SetCanvasGroupInteractable(Score, false);
        SetCanvasGroupInteractable(Credits, false);
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

    public void StartGame()
    {
        _time = 0;
        if (Relogio != null) Relogio.text = "0";
        ChangeCurrentColor(Color.white);
        Paused = false;
    }

    public void StopGame()
    {
        _time = 0;
        if (Relogio != null) Relogio.text = "0";
        if (Paleta != null) Paleta.enabled = true;
    }

    private void _resetGame()
    {
        ClearAll();
        if (Relogio != null) Relogio.text = "0";
        _time = 0;
        ChangeCurrentColor(Color.white);
    }

    public void CheckColors()
    {
        if (HasWhite())
            return;

        CheckResult result = CheckAll();
        if (result.IsSuccessful)
        {
            int elapsedSeconds = Mathf.FloorToInt(_time);
            if (ScoreText != null)
                ScoreText.text = $"{elapsedSeconds} segundos";
            if (AddScore != null)
                AddScore.SetActive(true);
            Paused = true;
        }
        else
        {
            foreach (int id in result.Errors)
            {
                if (id >= 0 && id < Images.Count && Images[id] != null)
                    Images[id].IndicaErro();
            }
            if (Erro != null)
                Erro.Show();
        }
    }

    public void AddNewScore()
    {
        string nome = Nome != null ? Nome.text : string.Empty;
        if (!string.IsNullOrWhiteSpace(nome))
        {
            NetworkedScore.Instance?.PushScore(nome, Mathf.FloorToInt(_time));
        }
        CloseAddScore();
    }

    public void CloseAddScore()
    {
        if (AddScore != null)
            AddScore.SetActive(false);
        GoTo(Menu);
    }

    private bool HasWhite()
    {
        for (int i = 0; i < Images.Count; i++)
        {
            var paintable = Images[i];
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
            if (i >= Images.Count || Images[i] == null) continue;
            Paintable image = Images[i];
            if (!_colorCheckBuffer.Add(image.color))
                errors.Add(i);
        }
    }

    private void CheckColumn(int column, HashSet<int> errors)
    {
        _colorCheckBuffer.Clear();
        for (int i = column; i < GridCellCount; i += GridSize)
        {
            if (i >= Images.Count || Images[i] == null) continue;
            Paintable image = Images[i];
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
            if (n >= Images.Count || Images[n] == null) continue;
            Paintable image = Images[n];
            if (!_colorCheckBuffer.Add(image.color))
                errors.Add(n);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMenu() => GoTo(Menu);
    public void GoToGame() => GoTo(Game);
    public void GoToScore() => GoTo(Score);
    public void GoToCredits() => GoTo(Credits);

    private void GoTo(GameObject scene)
    {
        if (scene == null) return;

        GameObject current = GetCurrentScene();
        if (current == scene)
            return;

        if (current == Game)
        {
            StopGameEvent?.Invoke();
            _resetGame();
        }
        else if (scene == Game)
        {
            StartGameEvent?.Invoke();
        }

        if (BackButton != null) BackButton.SetActive(false);
        if (BackButton2 != null) BackButton2.SetActive(false);
        if (QuitButton != null) QuitButton.SetActive(false);
        
        scene.SetActive(true);
        Fade(current, scene, Canvas);
    }

    private void Fade(GameObject sceneToHide, GameObject sceneToShow, GameObject allScenes)
    {
        if (sceneToHide == null || sceneToShow == null || allScenes == null)
            return;
        sceneMovement = new SceneMovement(sceneToHide, sceneToShow, allScenes);
    }

    private GameObject GetCurrentScene()
    {
        if (Menu != null && Menu.activeSelf)
            return Menu;
        if (Game != null && Game.activeSelf)
            return Game;
        if (Score != null && Score.activeSelf)
            return Score;
        return Credits;
    }

    private void Hide(GameObject target)
    {
        if (target != null)
            target.SetActive(false);
    }

    public void Update()
    {
        if (!Paused)
        {
            _time += Time.deltaTime;
            if (Relogio != null)
                Relogio.text = $"{Mathf.FloorToInt(_time)}";
        }

        if (sceneMovement != null)
        {
            float deltaTime = Time.deltaTime;
            sceneMovement.CurTime += deltaTime;
            Vector3 move = (deltaTime / sceneMovement.TotalTime) * sceneMovement.TotalMove;
            if (((sceneMovement.CurMove.x + move.x > sceneMovement.TotalMove.x) && (sceneMovement.TotalMove.x > 0))
                ||
                ((sceneMovement.CurMove.x + move.x < sceneMovement.TotalMove.x) && (sceneMovement.TotalMove.x < 0)))
            {
                move = sceneMovement.TotalMove - sceneMovement.CurMove;
                sceneMovement.CurMove = sceneMovement.TotalMove;
            }
            else
            {
                sceneMovement.CurMove += move;
            }

            if (sceneMovement.ScenesTransform != null)
                sceneMovement.ScenesTransform.position = sceneMovement.ScenesTransform.position - move;

            if (sceneMovement.CurTime >= sceneMovement.TotalTime)
            {
                SetCanvasGroupInteractable(sceneMovement.NextScene, true);

                if (sceneMovement.NextScene == Menu)
                {
                    if (QuitButton != null) QuitButton.SetActive(true);
                }
                else if (sceneMovement.NextScene == Score)
                {
                    if (BackButton2 != null) BackButton2.SetActive(true);
                }
                else
                {
                    if (BackButton != null) BackButton.SetActive(true);
                }

                Hide(sceneMovement.CurrentScene);
                sceneMovement.NextScene?.GetComponent<ISceneManager>()?.Ready();
                sceneMovement = null;
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

    public void ChangeCurrentColor(Color c)
    {
        if (Brush != null)
            Brush.color = c;
        CurrentColor = c;
    }

    public void ClearAll()
    {
        for (int i = 0; i < Images.Count; i++)
        {
            if (Images[i] != null)
                Images[i].Clear();
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

        public Vector3 TotalMove { get; }
        public Vector3 CurMove { get; set; }
        public float TotalTime { get; }
        public float CurTime { get; set; }

        public SceneMovement(GameObject currentScene, GameObject nextScene, GameObject allScenes)
        {
            CurrentScene = currentScene;
            NextScene = nextScene;
            AllScene = allScenes;
            TotalMove = (ShowTransform != null && HideTransform != null) 
                ? ShowTransform.position - HideTransform.position 
                : Vector3.zero;

            CurMove = Vector3.zero;
            TotalTime = 0.65f;
            CurTime = 0;
        }
    }
}
