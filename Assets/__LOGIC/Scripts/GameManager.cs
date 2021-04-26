using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    public void Awake()
    {

#if UNITY_STANDALONE
        Screen.SetResolution(360, 720, false);
#endif

#if UNITY_WEBGL
        Screen.SetResolution(280, 560, false);
#endif

#if UNITY_EDITOR
        Screen.SetResolution(720, 1460, false);
#endif

        Instance = this;
        Hide(Game);
        Hide(Score);
        Hide(Credits);
        Menu.SetActive(true);
        AddScore.SetActive(false);
        BackButton.SetActive(false);
        BackButton2.SetActive(false);
        Menu.GetComponent<CanvasGroup>().interactable = true;
        Game.GetComponent<CanvasGroup>().interactable = false;
        Score.GetComponent<CanvasGroup>().interactable = false;
        Credits.GetComponent<CanvasGroup>().interactable = false;
    }

    public void StartGame()
    {
        _time = 0;
        Relogio.text = "0";
        ChangeCurrentColor(Color.white);
        Paused = false;


    }

    public void StopGame()
    {
        _time = 0;
        Relogio.text = "0";
        Paleta.enabled = true;
    }

    private void _resetGame()
    {
        ClearAll();
        Relogio.text = "0";
        _time = 0;
        ChangeCurrentColor(Color.white);
    }

    public void CheckColors()
    {
        if (HasWhite())
            return;
        CheckResult result = CheckAll();
        //Debug.Log(result.IsSuccessful);
        //Debug.Log(result.Errors.Count);
        if (result.IsSuccessful)
        {
            //TODO: Cabou
            //Debug.Log("cabou");
            _time = float.Parse(Relogio.text);
            ScoreText.text = Relogio.text + " segundos";
            AddScore.SetActive(true);
            Paused = true;
        }
        else
        {
            foreach(int id in result.Errors)
            {
                Images[id].IndicaErro();
                Erro.Show();
            }
        }
        //TODO: result.Errors -> Ids que estão errados
    }

    public void AddNewScore()
    {
        string nome = Nome.text;
        if (!string.IsNullOrWhiteSpace(nome))
            NetworkedScore.Instance.PushScore(nome, Mathf.FloorToInt(_time));
        CloseAddScore();
    }

    public void CloseAddScore()
    {
        AddScore.SetActive(false);
        GoTo(Menu);
    }

    private bool HasWhite()
    {
        foreach (Paintable i in Images)
            if (i.color == Color.white)
                return true;
        return false;
    }

    private CheckResult CheckAll()
    {
        HashSet<int> errors = new HashSet<int>();
        for (int i = 0; i < 5; i++)
        {
            CheckRow(i, ref errors);
            CheckColumn(i, ref errors);
        }
        //Diagonais
        List<int[]> groups = new List<int[]>
        {
            //Direita
            new[] { 15, 21 },
            new[] { 10, 16, 22 },
            new[] { 5, 11, 17, 23 },
            new[] { 0, 6, 12, 18, 24 },
            new[] { 1, 7, 13, 19 },
            new[] { 2, 8, 14 },
            new[] { 3, 9 },
            //Esquerda
            new[] { 1, 5 },
            new[] { 2, 6, 10 },
            new[] { 3, 7, 11, 15 },
            new[] { 4, 8, 12, 16, 20 },
            new[] { 9, 13, 17, 21 },
            new[] { 14, 18, 22 },
            new[] { 19, 23 }
        };
        foreach (int[] group in groups)
            CheckGroup(group, ref errors);
        return new CheckResult(errors);
    }

    private void CheckRow(int row, ref HashSet<int> errors)
    {
        List<Color> colors = new List<Color>();
        int start = row * 5, end = row * 5 + 5;
        for (int i = start; i < end; i++)
        {
            Paintable image = Images[i];
            if (colors.Contains(image.color))
                errors.Add(i);
            colors.Add(image.color);
        }
    }

    private void CheckColumn(int column, ref HashSet<int> errors)
    {
        List<Color> colors = new List<Color>();
        for (int i = column; i < 25; i += 5)
        {
            Paintable image = Images[i];
            if (colors.Contains(image.color))
                errors.Add(i);
            colors.Add(image.color);
        }
    }

    private void CheckGroup(int[] arr, ref HashSet<int> errors)
    {
        List<Color> colors = new List<Color>();
        foreach (int n in arr)
        {
            Paintable image = Images[n];
            if (colors.Contains(image.color))
                errors.Add(n);
            colors.Add(image.color);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMenu()
        => GoTo(Menu);

    public void GoToGame()
        => GoTo(Game);

    public void GoToScore()
        => GoTo(Score);

    public void GoToCredits()
        => GoTo(Credits);

    private void GoTo(GameObject scene)
    {
        GameObject current = GetCurrentScene();
        if (current == scene)
            return;
        if (current == Game)
        {
            StopGameEvent.Invoke();
            _resetGame();
        }
        else if(scene == Game){
            StartGameEvent.Invoke();
        }

        //current.GetComponent<CanvasGroup>().interactable = false;
        BackButton.SetActive(false);
        BackButton2.SetActive(false);
        QuitButton.SetActive(false);
        scene.SetActive(true);
        Fade(current, scene, Canvas);
    }

    SceneMovement sceneMovement = null;
    private void Fade(GameObject sceneToHide, GameObject sceneToShow, GameObject allScenes)
    {
        sceneMovement = new SceneMovement(sceneToHide, sceneToShow, allScenes);
    }

    private GameObject GetCurrentScene()
    {
        if (Menu.activeSelf)
            return Menu;
        if (Game.activeSelf)
            return Game;
        if (Score.activeSelf)
            return Score;
        return Credits;
    }

    private void Hide(GameObject gameObject)
    {
        //gameObject.transform.position = OutBase.transform.position;
        gameObject.SetActive(false);
    }



    public void Update()
    {
        if (!Paused)
        {
            _time += Time.deltaTime;
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
                sceneMovement.CurMove += move;
            sceneMovement.ScenesTransform.position = sceneMovement.ScenesTransform.position - move;

            if (sceneMovement.CurTime >= sceneMovement.TotalTime)
            {
                sceneMovement.NextScene.GetComponent<CanvasGroup>().interactable = true;
                if (sceneMovement.NextScene == Menu)
                    QuitButton.SetActive(true);
                else
                    if(sceneMovement.NextScene == Score)
                        BackButton2.SetActive(true);
                    else
                        BackButton.SetActive(true);
                Hide(sceneMovement.CurrentScene);
                sceneMovement.NextScene.GetComponent<ISceneManager>()?.Ready();
                sceneMovement = null;
            }
        }
    }

    public class CheckResult
    {
        public bool IsSuccessful
        {
            get
            {
                return Errors.Count == 0;
            }
        }
        public HashSet<int> Errors { get; }

        public CheckResult(HashSet<int> errors)
        {
            Errors = errors;
        }
    }

    public void ChangeCurrentColor(Color c)
    {
        Brush.color = c;
        CurrentColor = c;
    }

    public void ClearAll()
    {
        foreach (Paintable p in Images)
            p.Clear();
    }

    public class SceneMovement
    {
        public GameObject CurrentScene { get; }
        public GameObject NextScene { get; }
        public GameObject AllScene { get;  }

        public Transform ShowTransform { get { return NextScene.transform; } }
        public Transform HideTransform { get { return CurrentScene.transform; } }
        public Transform ScenesTransform { get { return AllScene.transform; } }

        public Vector3 TotalMove { get; }
        public Vector3 CurMove { get; set; }
        public float TotalTime { get; }
        public float CurTime { get; set; }

        public SceneMovement(GameObject currentScene, GameObject nextScene, GameObject allScenes)
        {
            CurrentScene = currentScene;
            NextScene = nextScene;
            AllScene = allScenes;
            TotalMove = ShowTransform.position - HideTransform.position;

            CurMove = Vector3.zero;
            TotalTime = 0.65f;
            CurTime = 0;
        }
    }
}
