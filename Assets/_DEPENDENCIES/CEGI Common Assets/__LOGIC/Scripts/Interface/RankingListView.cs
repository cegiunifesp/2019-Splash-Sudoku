using UnityEngine;

public class RankingListView : MonoBehaviour
{
    public RankingEntry RankingEntryTemplate;

    private void Awake()
    {
        Refresh();
    }

    public void Refresh()
    {
        NetworkedScore.Instance.GetScores(10, data =>
        {
            Clear();
            AddEntries(data);
        });
    }
    private void AddEntries(NetworkedScoreEntry[] scores)
    {
        for (var i = 0; i < scores.Length; i++)
        {
            var score = scores[i];
            Instantiate(RankingEntryTemplate, transform).Init((i + 1).ToString(), score.Name, score.Score);
        }
    }

    private void Clear()
    {
        RankingEntry[] entries = transform.GetComponentsInChildren<RankingEntry>();
        for (var i = 0; i < entries.Length; i++)
        {
            Destroy(entries[i].gameObject);
        }
    }
}
