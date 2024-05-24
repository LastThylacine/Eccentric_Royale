using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerPrefab _playerPrefab;

    #region SERVER
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (Utils.IsSceneActive(singleton.onlineScene))
        {
            var player = Instantiate(_playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
        }
    }
    #endregion

    #region CLIENT
    public override void OnStartClient()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs")
            .Where(x => x != singleton.playerPrefab)
            .ToArray();

        foreach (var prefab in prefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        if (sceneOperation == SceneOperation.LoadAdditive) StartCoroutine(LoadAdditive(newSceneName));
        if (sceneOperation == SceneOperation.UnloadAdditive) StartCoroutine(UnloadAdditive(newSceneName));
    }

    private IEnumerator LoadAdditive(string sceneName)
    {
        if(mode == NetworkManagerMode.ClientOnly)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        NetworkClient.isLoadingScene = false;
        OnClientSceneChanged();
    }

    private IEnumerator UnloadAdditive(string sceneName)
    {
        if (mode == NetworkManagerMode.ClientOnly)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            yield return Resources.UnloadUnusedAssets();
        }

        NetworkClient.isLoadingScene = false;
        OnClientSceneChanged();
    }
    #endregion
}
