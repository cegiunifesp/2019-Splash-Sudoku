using UnityEngine;

public class GameSceneManager : MonoBehaviour, ISceneManager
{
    public void OnEnable()
    {
        GameManager.Instance?.StopGame();
    }

    public void Ready()
    {
        GameManager.Instance.StartGame();
    }
}