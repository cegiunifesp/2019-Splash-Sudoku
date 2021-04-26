using UnityEngine;

public class ScoreSceneManager : MonoBehaviour, ISceneManager
{
    public GameObject RankingList;
    public GameObject ScoreBase;
    public GameObject Loading;

    public void Awake()
    {
        LoadRanking();
    }

    public void Ready()
    {
    }

    public void LoadRanking()
    {
        if (Loading.activeSelf)
            return;
        Loading.SetActive(true);
        NetworkedScore.Instance.GetScores(10, entries =>
        {
            int i = 0;
            foreach (NetworkedScoreEntry entry in entries)
                AddScore(++i, entry.Name, entry.Score);
        });
        Loading.SetActive(false);
    }

    private void AddScore(int pos, string nome, string tempo)
    {
        GameObject score = Instantiate(ScoreBase, RankingList.transform, false);
        var scoreObj = score.GetComponent<ScoreObject>();
        scoreObj.Posicao.text = pos.ToString();
        scoreObj.Nome.text = nome;
        scoreObj.Score.text = tempo;
        score.SetActive(true);
    }
}
