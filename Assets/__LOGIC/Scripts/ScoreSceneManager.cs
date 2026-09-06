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
        LoadRanking();
    }

    public void LoadRanking()
    {
        if (Loading != null && Loading.activeSelf)
            return;

        if (Loading != null)
            Loading.SetActive(true);

        if (RankingList != null)
        {
            foreach (Transform child in RankingList.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (NetworkedScore.Instance != null)
        {
            NetworkedScore.Instance.GetScores(10, entries =>
            {
                if (entries != null)
                {
                    int i = 0;
                    foreach (NetworkedScoreEntry entry in entries)
                    {
                        AddScore(++i, entry.Name, entry.Score);
                    }
                }

                if (Loading != null)
                    Loading.SetActive(false);
            });
        }
        else
        {
            if (Loading != null)
                Loading.SetActive(false);
        }
    }

    private void AddScore(int pos, string nome, string tempo)
    {
        if (ScoreBase == null || RankingList == null)
            return;

        GameObject score = Instantiate(ScoreBase, RankingList.transform, false);
        var scoreObj = score.GetComponent<ScoreObject>();

        if (scoreObj == null)
        {
            Debug.LogError("O prefab ScoreBase não possui o componente ScoreObject anexado!");
            return;
        }

        scoreObj.Init(pos.ToString(), nome, tempo);
    }
}
