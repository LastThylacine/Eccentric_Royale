using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class ServerTimer : NetworkBehaviour
{
    private const int _startDelay = 10;

    public void StartTick()
    {
        StartCoroutine(StartTickCoroutine());
    }

    private IEnumerator StartTickCoroutine()
    {
        WaitForSeconds oneSecond = new WaitForSeconds(1);

        for (int i = _startDelay; i > 0; i--)
        {
            StartOneTick(i);
            yield return oneSecond;
        }

        FinishStartTick();
    }

    [ClientRpc]
    public void StartOneTick(int i)
    {
        MatchmakingManager.Instance.LocalSceneDependency.StartTimer.StartTick(i);
    }

    [ClientRpc]
    public void FinishStartTick()
    {
        MatchmakingManager.Instance.LocalSceneDependency.StartTimer.Destroy();
    }
}
