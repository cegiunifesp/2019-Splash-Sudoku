using UnityEngine;
using UnityEngine.Serialization;

public class RankingListView : MonoBehaviour
{
    [FormerlySerializedAs("RankingEntryTemplate")]
    public ScoreObject rankingEntryTemplate;

    public ScoreObject RankingEntryTemplate
    {
        get => rankingEntryTemplate;
        set => rankingEntryTemplate = value;
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
        if (scores == null || rankingEntryTemplate == null)
            return;

        for (var i = 0; i < scores.Length; i++)
        {
            var score = scores[i];
            ScoreObject entry = Instantiate(rankingEntryTemplate, transform, false);
            entry.name = $"Ranking_Item_{i + 1}";
            string formattedTime = score.Score.EndsWith("s") ? score.Score : $"{score.Score}s";
            entry.Init((i + 1).ToString(), score.Name, formattedTime);
        }
    }

    private void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.name.Contains("TitleLine") || child.name.Contains("Header") || child.GetSiblingIndex() == 0)
                continue;

            if (rankingEntryTemplate != null && child.gameObject == rankingEntryTemplate.gameObject)
            {
                child.gameObject.SetActive(false);
                continue;
            }

            Destroy(child.gameObject);
        }
    }
}
