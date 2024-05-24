using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchmakingManager : MonoBehaviour
{
    #region SINGLETON
    public static MatchmakingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
    #endregion

    #region CLIENT
    [SerializeField] private MatchmakingUI _ui;

    [Client]
    public void ConnectFinish(string[] cardIDs)
    {
        _ui.SetImages(cardIDs);
    }

    [Client]
    public void DestroyUI()
    {
        _ui.Destroy();
    }

    public LocalSceneDependency LocalSceneDependency { get; private set; }
    public void AddNewSceneClient(LocalSceneDependency localSceneDependency)
    {
        this.LocalSceneDependency = localSceneDependency;
    }
    #endregion

    #region SERVER
#if UNITY_SERVER
    public class StringArray
    {
        public string[] arr;
    }

    private const string SERVER_PASSWORD = "Fkaf%jJ_JtjoR124";
    private const string KEY = "key";
    private const string USERID = "userID";

    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private Transform _navMeshPlanePrefab;
    [Scene, SerializeField] private string _additiveGameScene;

    private List<PlayerPrefab> _players = new List<PlayerPrefab>();
    private Queue<LocalSceneDependency> _localSceneDependencies = new Queue<LocalSceneDependency>();
    [Server]
    public void OnJoin(PlayerPrefab player, string sqlID)
    {
        string uri = URLLibrary.MAIN + URLLibrary.GETDECK;
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {KEY, SERVER_PASSWORD },
            {USERID, sqlID }
        };

        Network.Instance.Post(uri, data, (arrString) => SuccessLoadDeck(sqlID, player, arrString));
    }

    private void SuccessLoadDeck(string sqlID, PlayerPrefab player, string arrString)
    {
        string json = "{\"arr\":" + arrString + "}";

        string[] cardIDs = JsonUtility.FromJson<StringArray>(json).arr;

        if(!player)
        {
            Debug.LogWarning("Player is null");
            return;
        }

        player.SetSqlID(sqlID);
        player.SetDeck(cardIDs);
        player.SuccessConnect(cardIDs);

        if(_players.Count == 0)
        {
            _players.Add(player);
            return;
        }

        PlayerPrefab secondaryPlayer = _players[0];
        _players.RemoveAt(0);

        StartCoroutine(StartMatch(player, secondaryPlayer));
    }

    private IEnumerator StartMatch(PlayerPrefab player1, PlayerPrefab player2)
    {
        yield return SceneManager.LoadSceneAsync(_additiveGameScene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => _localSceneDependencies.Count > 0);
        var localSceneDependency = _localSceneDependencies.Dequeue();

        int matchHeight = GetMatchHeight();

        localSceneDependency.InitServer(player1, player2, matchHeight);

        localSceneDependency.CardManager.Init(player1.Deck, player2.Deck);

        SpawnNavMeshPlane(localSceneDependency.transform, matchHeight);

        ChangePlayerScene(player1, localSceneDependency.gameObject.scene);
        ChangePlayerScene(player2, localSceneDependency.gameObject.scene);
        player1.StartMatch(player1.Deck, player2.Deck, matchHeight, false);
        player2.StartMatch(player2.Deck, player1.Deck, matchHeight, true);
    }

    private void SpawnNavMeshPlane(Transform localSceneDependency, int matchHeight)
    {
        float height = matchHeight * LocalSceneDependency.LIFTING_HEIGHT;

        Instantiate(_navMeshPlanePrefab, Vector3.up * height, Quaternion.identity, localSceneDependency);

        _navMeshSurface.BuildNavMesh();
    }

    private void ChangePlayerScene(PlayerPrefab player, Scene scene)
    {
        var client = player.connectionToClient;

        NetworkServer.RemovePlayerForConnection(client, false);
        SceneManager.MoveGameObjectToScene(player.gameObject, scene);

        client.Send(new SceneMessage
        {
            sceneName = scene.name,
            sceneOperation = SceneOperation.LoadAdditive,
            customHandling = true
        });

        NetworkServer.AddPlayerForConnection(client, player.gameObject);
    }

    private int GetMatchHeight()
    {
        int height = UnityEngine.Random.Range(0, 50);
        return height;
    }

    [Server]
    public void OnLeave(PlayerPrefab player)
    {
        if (_players.Contains(player))
            _players.Remove(player);
    }

    public void AddNewSceneServer(LocalSceneDependency localSceneDependency)
    {
        _localSceneDependencies.Enqueue(localSceneDependency);
    }
#endif
    #endregion
}
