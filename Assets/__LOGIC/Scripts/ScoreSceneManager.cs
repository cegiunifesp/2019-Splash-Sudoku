using UnityEngine;

public class ScoreSceneManager : MonoBehaviour, ISceneManager
{
    [Header("UI References")]
    public GameObject RankingList;
    public GameObject ScoreBase;
    public GameObject Loading;

    public void Awake()
    {
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
                    int pos = 0;
                    foreach (NetworkedScoreEntry entry in entries)
                    {
                        pos++;
                        AddScore(container, template, pos, entry.Name, entry.Score);
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

    private Transform ResolveContainer()
    {
        if (RankingList == null)
            return null;

        // Se RankingList for o Content do ScrollView e tiver o filho Ranking_LIST, usa o filho
        Transform subList = RankingList.transform.Find("Ranking_LIST");
        if (subList != null)
            return subList;

        return RankingList.transform;
    }

    private GameObject ResolveTemplate(Transform container)
    {
        if (ScoreBase != null && ScoreBase.GetComponent<ScoreObject>() != null)
            return ScoreBase;

        if (container != null)
        {
            ScoreObject[] existing = container.GetComponentsInChildren<ScoreObject>(true);
            if (existing.Length > 0)
            {
                ScoreBase = existing[0].gameObject;
                return ScoreBase;
            }
        }

        ScoreObject found = FindObjectOfType<ScoreObject>();
        if (found != null)
        {
            ScoreBase = found.gameObject;
            return ScoreBase;
        }

        return ScoreBase;
    }

    private void ClearDynamicEntries(Transform container, GameObject template)
    {
        if (container == null) return;

        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Transform child = container.GetChild(i);

            // Preserva o cabeçalho da tabela (TitleLine_PANEL)
            if (child.name.Contains("TitleLine") || child.name.Contains("Header") || child.GetSiblingIndex() == 0)
            {
                child.gameObject.SetActive(true);
                continue;
            }

            // O template original deve ficar apenas oculto para clonagem
            if (template != null && child.gameObject == template)
            {
                child.gameObject.SetActive(false);
                continue;
            }

            Destroy(child.gameObject);
        }
    }

    private void AddScore(Transform container, GameObject template, int pos, string nome, string tempo)
    {
        if (container == null || template == null)
            return;

        GameObject row = Instantiate(template, container, false);
        row.name = $"Score_Entry_{pos}";
        row.SetActive(true);

        var scoreObj = row.GetComponent<ScoreObject>();
        if (scoreObj != null)
        {
            string formattedTempo = tempo.EndsWith("s") ? tempo : $"{tempo}s";
            scoreObj.Init(pos.ToString(), nome, formattedTempo);
        }
    }
}
