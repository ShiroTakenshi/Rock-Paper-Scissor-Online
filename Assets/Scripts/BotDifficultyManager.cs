using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;

public class BotDifficultyManager : MonoBehaviour
{
    [SerializeField] Bot bot;
    [SerializeField] int selectedDifficulty;
    [SerializeField] BotStats[] botDifficulties;
    [Header("Remote Config Parameters:")]
    [SerializeField] bool enableRemotConfig = false;


    IEnumerator Start()
    {
        yield return new WaitUntil(() => bot.IsReady);

        var newStats = botDifficulties[selectedDifficulty];
        bot.SetStats(newStats, true);

        if (enableRemotConfig == false)
            yield break;
    }
}
