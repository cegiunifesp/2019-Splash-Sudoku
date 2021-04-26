using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System;
using UnityEngine;

[RequireComponent(typeof(GameSparksUnity))]
public class GameSparksConnection : SingletonBehaviour<GameSparksConnection>
{
    public bool LoggedIn { get; set; }

    private void Awake()
    {
        LoggedIn = false;
    }

    public void Login(string userName, string password, Action<AuthenticationResponse> onResponse = null)
    {
        new AuthenticationRequest().SetUserName(userName).SetPassword(password).Send(response =>
            {
                LoggedIn = !response.HasErrors;
                onResponse?.Invoke(response);
            });
    }
    public void Register(string userName,string displayName, string password, Action<RegistrationResponse> onResponse = null)
    {
        new RegistrationRequest().SetUserName(userName).SetDisplayName(displayName).SetPassword(password).Send(response =>
        {
            LoggedIn = !response.HasErrors;
            onResponse?.Invoke(response);
        });
    }
    public void SendEvent(string eventKey, Action<LogEventRequest> setAttributes, Action<LogEventResponse> onResponse = null)
    {
        var eventRequest = new LogEventRequest().SetEventKey(eventKey);
        setAttributes.Invoke(eventRequest);
        eventRequest.Send(onResponse);
    }
}
