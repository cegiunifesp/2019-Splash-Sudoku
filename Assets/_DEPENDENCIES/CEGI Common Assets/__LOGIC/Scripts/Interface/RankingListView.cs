using UnityEngine;

public class RankingListView : MonoBehaviour
{
    public ScoreObject RankingEntryTemplate;

    private void Awake()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (NetworkedScore.Instance == null)
            return;

        NetworkedScore.Instance.GetScores(10, data =>
        {
            Clear();
            AddEntries(data);
        });
    }

    private void AddEntries(NetworkedScoreEntry[] scores)
    {
        if (scores == null || RankingEntryTemplate == null)
            return;

        for (var i = 0; i < scores.Length; i++)
        {
            var score = scores[i];
            ScoreObject entry = Instantiate(RankingEntryTemplate, transform);
            entry.Init((i + 1).ToString(), score.Name, score.Score);
        }
    }

    private void Clear()
    {
        ScoreObject[] entries = transform.GetComponentsInChildren<ScoreObject>();
        for (var i = 0; i < entries.Length; i++)
        {
            if (entries[i] != null && entries[i] != RankingEntryTemplate)
            {
                Destroy(entries[i].gameObject);
            }
        }
    }
}
