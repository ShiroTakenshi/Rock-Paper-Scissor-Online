using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using System;

public class BotDifficultyManager : MonoBehaviour
{

    [SerializeField] Bot bot;
    [SerializeField] int selectedDifficulty;
    [SerializeField] BotStats[] botDifficulties;
    [Header("Remote Config Parameters:")]
    [SerializeField] bool enableRemotConfig = false;
    [SerializeField] private string DifficultyKey = "Difficulty";
    struct userAttributes { };
    struct AppAttributes { };


    IEnumerator Start()
    {
        yield return new WaitUntil(() => bot.IsReady);
        var newStats = botDifficulties[selectedDifficulty];
        bot.SetStats(newStats, true);

        if (enableRemotConfig == false)
            yield break;
        yield return new WaitUntil(() => UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn);

        RemoteConfigService.Instance.FetchCompleted += OnRemoteComplete;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new AppAttributes());
        //
    }

    private void OnRemoteComplete(ConfigResponse obj)
    {
        if (RemoteConfigService.Instance.appConfig.HasKey(DifficultyKey) == false)
        {
            Debug.Log("remoteConfig doesn't have key " + DifficultyKey);
            return;
        }
        switch (obj.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("default");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("cached");
                break;
            case ConfigOrigin.Remote:
                selectedDifficulty = RemoteConfigService.Instance.appConfig.GetInt(DifficultyKey);
                selectedDifficulty = Mathf.Clamp(selectedDifficulty, 0, botDifficulties.Length - 1);
                var newStats = botDifficulties[selectedDifficulty];
                bot.SetStats(newStats, true);
                break;


        }

    }
}
