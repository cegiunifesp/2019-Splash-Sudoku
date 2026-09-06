using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NetworkedScoreEntry
{
    public string Score;
    public string Name;
}

public class NetworkedScore : SingletonBehaviour<NetworkedScore>, IScoreService
{
    private const string StorageKey = "SplashSudoku_RankingScores";
    private const int MaxStoredScores = 50;

    [Serializable]
    private class ScoreDataListWrapper
    {
        public List<NetworkedScoreEntry> entries = new List<NetworkedScoreEntry>();
    }

    private readonly List<NetworkedScoreEntry> _runtimeScores = new List<NetworkedScoreEntry>();
    private bool _isLoaded = false;
    public string GameName = "Splash Sudoku";

    private void Awake()
    {
        EnsureLoaded();
    }

    private void EnsureLoaded()
    {
        if (_isLoaded)
            return;

        _runtimeScores.Clear();

        if (PlayerPrefs.HasKey(StorageKey))
        {
            try
            {
                string json = PlayerPrefs.GetString(StorageKey);
                if (!string.IsNullOrEmpty(json))
                {
                    ScoreDataListWrapper wrapper = JsonUtility.FromJson<ScoreDataListWrapper>(json);
                    if (wrapper != null && wrapper.entries != null && wrapper.entries.Count > 0)
                    {
                        _runtimeScores.AddRange(wrapper.entries);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[NetworkedScore] Falha ao carregar ranking do PlayerPrefs: {ex.Message}");
            }
        }

        // Se estiver vazio, inicializa com registros padrão do jogo
        if (_runtimeScores.Count == 0)
        {
            _runtimeScores.Add(new NetworkedScoreEntry { Name = "Mestre Sudoku", Score = "35" });
            _runtimeScores.Add(new NetworkedScoreEntry { Name = "Tintas Pro", Score = "40" });
            _runtimeScores.Add(new NetworkedScoreEntry { Name = "Color Master", Score = "49" });
            _runtimeScores.Add(new NetworkedScoreEntry { Name = "Splash Kid", Score = "65" });
            _runtimeScores.Add(new NetworkedScoreEntry { Name = "Novato", Score = "85" });
            SaveToStorage();
        }

        SortAndTrim(_runtimeScores);
        _isLoaded = true;
    }

    private void SaveToStorage()
    {
        try
        {
            ScoreDataListWrapper wrapper = new ScoreDataListWrapper { entries = _runtimeScores };
            string json = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(StorageKey, json);
            PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[NetworkedScore] Erro ao salvar ranking: {ex.Message}");
        }
    }

    [ContextMenu("Clear Stored Scores")]
    public void ClearLocalScores()
    {
        ClearScores();
    }

    public void ClearScores(Action onFinished = null)
    {
        _runtimeScores.Clear();
        PlayerPrefs.DeleteKey(StorageKey);
        PlayerPrefs.Save();
        _isLoaded = false;
        EnsureLoaded();
        onFinished?.Invoke();
    }

    public void PushScore(string nickName, int score, Action onFinished = null)
    {
        EnsureLoaded();

        string cleanName = string.IsNullOrWhiteSpace(nickName) ? "Player" : nickName.Trim();
        _runtimeScores.Add(new NetworkedScoreEntry
        {
            Name = cleanName,
            Score = Mathf.Max(1, score).ToString()
        });

        SortAndTrim(_runtimeScores);
        SaveToStorage();
        onFinished?.Invoke();
    }

    public void GetScores(int count, Action<NetworkedScoreEntry[]> onFinished)
    {
        EnsureLoaded();
        SortAndTrim(_runtimeScores);

        int safeCount = Mathf.Clamp(count, 0, _runtimeScores.Count);
        NetworkedScoreEntry[] result = _runtimeScores.GetRange(0, safeCount).ToArray();
        onFinished?.Invoke(result);
    }

    private void SortAndTrim(List<NetworkedScoreEntry> scores)
    {
        // No Sudoku, menor tempo em segundos é a melhor posição (ordenação ascendente)
        scores.Sort((a, b) => ParseScore(a.Score).CompareTo(ParseScore(b.Score)));

        if (scores.Count > MaxStoredScores)
        {
            scores.RemoveRange(MaxStoredScores, scores.Count - MaxStoredScores);
        }
    }

    private int ParseScore(string scoreValue)
    {
        if (int.TryParse(scoreValue, out int parsed))
            return parsed;
        return int.MaxValue;
    }
}
