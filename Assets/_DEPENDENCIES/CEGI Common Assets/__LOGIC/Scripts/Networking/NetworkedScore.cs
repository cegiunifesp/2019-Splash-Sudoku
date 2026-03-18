using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NetworkedScoreEntry
{
    public string Score;
    public string Name;
}

public class NetworkedScore : SingletonBehaviour<NetworkedScore>
{
    private const int MaxStoredScores = 100;
    private readonly List<NetworkedScoreEntry> _runtimeScores = new List<NetworkedScoreEntry>();
    public string GameName;

    [ContextMenu("Clear Runtime Scores")]
    private void ClearLocalScores()
    {
        _runtimeScores.Clear();
    }

    public void PushScore(string nickName, int score, Action onFinished = null)
    {
        var cleanName = string.IsNullOrWhiteSpace(nickName) ? "Player" : nickName.Trim();
        _runtimeScores.Add(new NetworkedScoreEntry
        {
            Name = cleanName,
            Score = score.ToString()
        });

        SortAndTrim(_runtimeScores);
        onFinished?.Invoke();
    }

    public void GetScores(int count, Action<NetworkedScoreEntry[]> onFinished)
    {
        SortAndTrim(_runtimeScores);

        var safeCount = Mathf.Clamp(count, 0, _runtimeScores.Count);
        var result = _runtimeScores.GetRange(0, safeCount).ToArray();
        onFinished?.Invoke(result);
    }

    private void SortAndTrim(List<NetworkedScoreEntry> scores)
    {
        scores.Sort((a, b) => ParseScore(b.Score).CompareTo(ParseScore(a.Score)));
        if (scores.Count > MaxStoredScores)
        {
            scores.RemoveRange(MaxStoredScores, scores.Count - MaxStoredScores);
        }
    }

    private int ParseScore(string scoreValue)
    {
        int parsed;
        return int.TryParse(scoreValue, out parsed) ? parsed : 0;
    }
}
