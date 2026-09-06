using System;

public interface IScoreService
{
    void PushScore(string nickName, int score, Action onFinished = null);
    void GetScores(int count, Action<NetworkedScoreEntry[]> onFinished);
    void ClearScores(Action onFinished = null);
}
