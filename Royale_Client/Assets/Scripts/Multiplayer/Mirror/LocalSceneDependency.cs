using Mirror;
using System;
using UnityEngine;

public class LocalSceneDependency : MonoBehaviour
{
    public const float LIFTING_HEIGHT = 5f;
        
    private void Start()
    {
#if UNITY_SERVER
        StartServer();
#else 
        StartClient();
#endif
    }

    public Spawner Spawner { get; private set; }

    public void SetSpawner(Spawner spawner) => Spawner = spawner;

    #region SERVER
    [SerializeField] private Transform[] _playerTowerPoints;
    [SerializeField] private Transform[] _enemyTowerPoints;
    [SerializeField] private Tower _towerPrefab;
    [SerializeField] private Spawner _spawnerPrefab;
    [SerializeField] private ServerTimer _serverTimerPrefab;
    [SerializeField] private MapInfo _mapInfo;
#if UNITY_SERVER
    private PlayerPrefab _player1Player;
    private PlayerPrefab _player2Enemy;

    private void StartServer()
    {
        CreateSpawner();

        CreateTower();

        CreateTimer();
    }

    private void CreateSpawner()
    {
        MatchmakingManager.Instance.AddNewSceneServer(this);
        Spawner = Instantiate(_spawnerPrefab, transform);
        Spawner.InitServer(CardManager, _mapInfo);
        NetworkServer.Spawn(Spawner.gameObject);
    }

    private void CreateTower()
    {
        for (int i = 0; i < _playerTowerPoints.Length; i++)
        {
            Transform point = _playerTowerPoints[i];

            var tower = Instantiate(_towerPrefab, point.position, point.rotation, point);

            _mapInfo.AddTower(tower, false);

            NetworkServer.Spawn(tower.gameObject);
        }

        for (int i = 0; i < _enemyTowerPoints.Length; i++)
        {
            Transform point = _enemyTowerPoints[i];

            var tower = Instantiate(_towerPrefab, point.position, point.rotation, point);

            _mapInfo.AddTower(tower, true);

            NetworkServer.Spawn(tower.gameObject);
        }
    }

    private void CreateTimer()
    {
        var timer = Instantiate(_serverTimerPrefab, transform);
        NetworkServer.Spawn(timer.gameObject);
        timer.StartTick();
    }

    public void InitServer(PlayerPrefab player1Player, PlayerPrefab player2Enemy, int matchHeight)
    {
        _player1Player = player1Player;
        _player2Enemy = player2Enemy;
        player1Player.SetLocalSceneDependency(this);
        player2Enemy.SetLocalSceneDependency(this);

        Spawner.SetHeight(matchHeight * LIFTING_HEIGHT);

        SetSceneHeight(matchHeight);
    }

    public bool IsEnemy(PlayerPrefab player) => player == _player2Enemy;
#endif
    #endregion

    [SerializeField] private Transform[] _objectsToLift;

    #region CLIENT 
    [SerializeField] private Transform[] _spawnPlanes;
    [field: SerializeField] public CardManager CardManager { get; private set; }
    [field: SerializeField] public StartTimer StartTimer { get; private set; }
    private void StartClient()
    {
        MatchmakingManager.Instance.AddNewSceneClient(this);
    }

    public void SetScene(int matchHeight, bool isEnemy)
    {
        if (isEnemy) SetEnemyScene();

        SetSceneHeight(matchHeight);
    }

    private void SetEnemyScene()
    {
        Transform camera = Camera.main.transform;

        Vector3 cameraRotation = camera.eulerAngles;
        cameraRotation.y = 180;
        camera.eulerAngles = cameraRotation;

        Vector3 cameraPosition = camera.position;
        cameraPosition.z *= -1;
        camera.position = cameraPosition;

        for (int i = 0; i < _spawnPlanes.Length; i++)
        {
            Transform plane = _spawnPlanes[i];

            Vector3 planeRotation = plane.eulerAngles;
            planeRotation.y = 180;
            plane.eulerAngles = planeRotation;

            Vector3 planePosition = plane.position;
            planePosition.z *= -1;
            plane.position = planePosition;
        }
    }
    #endregion

    private void SetSceneHeight(int matchHeight)
    {
        float y = matchHeight * LIFTING_HEIGHT;

        Transform camera = Camera.main.transform;
        Vector3 cameraPosition = camera.position;
        cameraPosition.y += y;
        camera.position = cameraPosition;

        for (int i = 0; i < _objectsToLift.Length; i++)
        {
            Vector3 position = _objectsToLift[i].position;
            position.y = y;
            _objectsToLift[i].position = position;
        }
    }
}
