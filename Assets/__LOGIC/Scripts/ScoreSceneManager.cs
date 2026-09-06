using UnityEngine;
using UnityEngine.Serialization;

public class ScoreSceneManager : MonoBehaviour, ISceneManager
{
    [Header("UI References")]
    [FormerlySerializedAs("RankingList")]
    public GameObject rankingList;

    [FormerlySerializedAs("ScoreBase")]
    public GameObject scoreBaseTemplate;

    [FormerlySerializedAs("Loading")]
    public GameObject loadingIndicator;

    // Backward compatibility properties
    public GameObject RankingList
    {
        get => rankingList;
        set => rankingList = value;
    }

    public GameObject ScoreBase
    {
        get => scoreBaseTemplate;
        set => scoreBaseTemplate = value;
    }

    public GameObject Loading
    {
        get => loadingIndicator;
        set => loadingIndicator = value;
    }

    public void Awake()
    {
    }

    public void Ready()
    {
        LoadRanking();
    }

    public void LoadRanking()
    {
        if (loadingIndicator != null && loadingIndicator.activeSelf)
            return;

        if (loadingIndicator != null)
            loadingIndicator.SetActive(true);

        Transform container = ResolveContainer();
        GameObject template = ResolveTemplate(container);

        if (template != null)
            template.SetActive(false);

        ClearDynamicEntries(container, template);

        if (NetworkedScore.Instance != null)
        {
            NetworkedScore.Instance.GetScores(10, entries =>
            {
                if (entries != null && container != null && template != null)
                {
                    int positionIndex = 0;
                    foreach (NetworkedScoreEntry entry in entries)
                    {
                        positionIndex++;
                        AddScore(container, template, positionIndex, entry.Name, entry.Score);
                    }
                }

                if (loadingIndicator != null)
                    loadingIndicator.SetActive(false);
            });
        }
        else
        {
            if (loadingIndicator != null)
                loadingIndicator.SetActive(false);
        }
    }

    private Transform ResolveContainer()
    {
        if (rankingList == null)
            return null;

        Transform subList = rankingList.transform.Find("Ranking_LIST");
        if (subList != null)
            return subList;

        return rankingList.transform;
    }

    private GameObject ResolveTemplate(Transform container)
    {
        if (scoreBaseTemplate != null && scoreBaseTemplate.GetComponent<ScoreObject>() != null)
            return scoreBaseTemplate;

        if (container != null)
        {
            ScoreObject[] existing = container.GetComponentsInChildren<ScoreObject>(true);
            if (existing.Length > 0)
            {
                scoreBaseTemplate = existing[0].gameObject;
                return scoreBaseTemplate;
            }
        }

        ScoreObject found = FindObjectOfType<ScoreObject>();
        if (found != null)
        {
            scoreBaseTemplate = found.gameObject;
            return scoreBaseTemplate;
        }

        return scoreBaseTemplate;
    }

    private void ClearDynamicEntries(Transform container, GameObject template)
    {
        if (container == null) return;

        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Transform child = container.GetChild(i);

            // Preserve header rows
            if (child.name.Contains("TitleLine") || child.name.Contains("Header") || child.GetSiblingIndex() == 0)
            {
                child.gameObject.SetActive(true);
                continue;
            }

            // The template itself stays inactive for instantiation
            if (template != null && child.gameObject == template)
            {
                child.gameObject.SetActive(false);
                continue;
            }

            Destroy(child.gameObject);
        }
    }

    private void AddScore(Transform container, GameObject template, int position, string playerName, string scoreTime)
    {
        if (container == null || template == null)
            return;

        GameObject row = Instantiate(template, container, false);
        row.name = $"Score_Entry_{position}";
        row.SetActive(true);

        var scoreObj = row.GetComponent<ScoreObject>();
        if (scoreObj != null)
        {
            string formattedScoreTime = scoreTime.EndsWith("s") ? scoreTime : $"{scoreTime}s";
            scoreObj.Init(position.ToString(), playerName, formattedScoreTime);
        }
    }
}
