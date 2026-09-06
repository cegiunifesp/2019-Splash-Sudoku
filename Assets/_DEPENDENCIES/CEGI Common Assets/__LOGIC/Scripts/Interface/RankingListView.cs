using UnityEngine;

public class RankingListView : MonoBehaviour
{
    public ScoreObject RankingEntryTemplate;

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
            ScoreObject entry = Instantiate(RankingEntryTemplate, transform, false);
            entry.name = $"Ranking_Item_{i + 1}";
            string formattedTempo = score.Score.EndsWith("s") ? score.Score : $"{score.Score}s";
            entry.Init((i + 1).ToString(), score.Name, formattedTempo);
        }
    }

    private void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.name.Contains("TitleLine") || child.name.Contains("Header") || child.GetSiblingIndex() == 0)
                continue;

            if (RankingEntryTemplate != null && child.gameObject == RankingEntryTemplate.gameObject)
            {
                child.gameObject.SetActive(false);
                continue;
            }

            Destroy(child.gameObject);
        }
    }
}
