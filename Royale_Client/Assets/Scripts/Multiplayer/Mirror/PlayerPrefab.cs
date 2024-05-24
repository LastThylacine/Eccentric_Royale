using Mirror;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class PlayerPrefab : NetworkBehaviour
{
    public string[] Deck { get; private set; }
    public void SetDeck(string[] deck) => Deck = deck;
    public string sqlID { get; private set; }

    public void SetSqlID(string sqlID) => this.sqlID = sqlID;

    #region CLIENT
    public override void OnStartClient()
    {
        base.OnStartClient();
        string id = UserInfo.Instance.ID.ToString();
        OnJoin(id);
    }

    [TargetRpc]
    public void StartMatch(string[] playerDeck, string[] enemyDeck, int matchHeight, bool isEnemy)
    {
        StartCoroutine(DelayStartMatch(playerDeck, enemyDeck, matchHeight, isEnemy));
    }

    private IEnumerator DelayStartMatch(string[] playerDeck, string[] enemyDeck, int matchHeight, bool isEnemy)
    {
        MatchmakingManager matchmakingManager = MatchmakingManager.Instance;
        yield return new WaitUntil(() => matchmakingManager.LocalSceneDependency);

        matchmakingManager.LocalSceneDependency.CardManager.Init(playerDeck, enemyDeck);

        matchmakingManager.LocalSceneDependency.SetScene(matchHeight, isEnemy);

        matchmakingManager.DestroyUI();
    }
    #endregion

    [Command]
    public void OnJoin(string sqlID)
    {
#if UNITY_SERVER
        if (!string.IsNullOrEmpty(this.sqlID)) return;

        MatchmakingManager.Instance.OnJoin(this, sqlID);
#endif
    }

    [Command]
    public void CmdSpawn(string id, Vector3 spawnPoint)
    {
#if UNITY_SERVER
        bool isEnemy = LocalSceneDependency.IsEnemy(this);
        LocalSceneDependency.Spawner.Spawn(id, spawnPoint, isEnemy);
        FinishSpawn();
#endif
    }

    [TargetRpc]
    private void FinishSpawn()
    {
        MatchmakingManager.Instance.LocalSceneDependency.Spawner.DestroyHologram();
    }

    [TargetRpc]
    public void SuccessConnect(string[] cardIDs)
    {
        MatchmakingManager.Instance.ConnectFinish(cardIDs);
    }

    #region SERVER
    public LocalSceneDependency LocalSceneDependency { get; private set; }

    public void SetLocalSceneDependency(LocalSceneDependency localSceneDependency) => this.LocalSceneDependency = localSceneDependency;

    public override void OnStopServer()
    {
#if UNITY_SERVER
        MatchmakingManager.Instance.OnLeave(this);
        base.OnStopServer();
#endif
    }
    #endregion
}
