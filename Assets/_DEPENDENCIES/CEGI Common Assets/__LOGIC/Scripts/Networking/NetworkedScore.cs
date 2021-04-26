using GameSparks.Api.Responses;
using GameSparks.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedScore : SingletonBehaviour<NetworkedScore>
{
    public string AccountLogin = "Joe";
    public string AccountPassword = "#secret#";
    public string GameName;

    [ContextMenu("Register Current Credentials")]
    private void RegisterCredentials()
    {
        GameSparksConnection.Instance.Register(AccountLogin, AccountLogin, AccountPassword, data => Debug.Log(data.JSONString));
    }

    private void Authenticated(Action action)
    {
        var gameSparksConnection = GameSparksConnection.Instance;
        if (!gameSparksConnection.LoggedIn)
        {
            gameSparksConnection.Login(AccountLogin, AccountPassword, response =>
            {
                if (gameSparksConnection.LoggedIn)
                    action();

            });
            return;
        }
        action();
    }

    public void PushScore(string nickName, int score, Action<LogEventResponse> onFinished = null)
    {
        Authenticated(() =>
        {
            GameSparksConnection.Instance.SendEvent("PushScore", scoreEvent =>
            {
                scoreEvent.SetEventAttribute("GameName", GameName).SetEventAttribute("Nickname", nickName)
                    .SetEventAttribute("Score", score);
            }, onFinished);
        });
    }


    public void GetScores(int count, Action<NetworkedScoreEntry[]> onFinished)
    {
        Authenticated(() =>
        {
            GameSparksConnection.Instance.SendEvent("GetScores",
                scoreEvent => { scoreEvent.SetEventAttribute("GameName", GameName).SetEventAttribute("Count", count); },
                response =>
                {
                    List<GSData> scoreDataList = response.ScriptData.GetGSDataList("score");
                    NetworkedScoreEntry[] scoreList = new NetworkedScoreEntry[scoreDataList.Count];
                    for (var i = 0; i < scoreDataList.Count; i++)
                    {
                        var score = scoreDataList[i].JSON;
                        scoreList[i] = JsonConvert.DeserializeObject<NetworkedScoreEntry>(score);
                    }

                    onFinished.Invoke(scoreList);
                });
        });
    }
  
}

    public struct NetworkedScoreEntry
    {
        public string Score;
        public string Name;
    }
