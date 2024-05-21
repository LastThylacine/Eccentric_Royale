using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public SpawnData(string id, Vector3 spawnPoint)
        {
            cardID = id;
            x = spawnPoint.x;
            y = spawnPoint.y;
            z = spawnPoint.z;
        }

        public string cardID;
        public float x;
        public float y;
        public float z;
        public uint serverTime;
    }

    [SerializeField] private TimeManager _timeManager;
    private Queue<GameObject> _holograms = new Queue<GameObject>();

    private void Start()
    {
        MultiplayerManager.Instance.SpawnPlayer += SpawnPlayer;
        MultiplayerManager.Instance.SpawnEnemy += SpawnEnemy;
        MultiplayerManager.Instance.Cheat += CancelSpawn;
    }

    private void OnDestroy()
    {
        MultiplayerManager.Instance.SpawnPlayer -= SpawnPlayer;
        MultiplayerManager.Instance.SpawnEnemy -= SpawnEnemy;
        MultiplayerManager.Instance.Cheat -= CancelSpawn;
    }

    private void SpawnPlayer(string json) => StartCoroutine(Spawn(json, false));

    private void SpawnEnemy(string json) => StartCoroutine(Spawn(json, true));

    public IEnumerator Spawn(string jsonSpawnData, bool isEnemy)
    {
        SpawnData data = JsonUtility.FromJson<SpawnData>(jsonSpawnData);
        string id = data.cardID;
        Vector3 spawnPoint = new Vector3(data.x, data.y, data.z);

        Unit unitPrefab;
        Quaternion rotation = Quaternion.identity;

        if (isEnemy)
        {
            unitPrefab = CardsInGame.Instance.EnemyDeck[id].Unit;
            rotation = Quaternion.Euler(0, 180, 0);
            spawnPoint *= -1;
        }
        else
            unitPrefab = CardsInGame.Instance.PlayerDeck[id].Unit;

        float diff = _timeManager.GetConvertTime(data.serverTime) - Time.time;

        if (diff < 0)
        {
            Debug.LogError("Время с сервера было отправлено слишком поздно");
        }
        else
        {
            yield return new WaitForSeconds(diff);
        }

        if (!isEnemy && _holograms.Count > 0)
        {
            var hologram = _holograms.Dequeue();
            Destroy(hologram);
        }

        Unit unit = Instantiate(unitPrefab, spawnPoint, rotation);
        unit.Init(isEnemy);
        MapInfo.Instance.AddUnit(unit);
    }

    private void CancelSpawn()
    {
        if (_holograms.Count < 1) return;

        var hologram = _holograms.Dequeue();
        Destroy(hologram);
    }

    public void SendSpawn(string id, Vector3 spawnPoint)
    {
        var hologram = Instantiate(CardsInGame.Instance.PlayerDeck[id].Hologram, spawnPoint, Quaternion.identity);

        _holograms.Enqueue(hologram);

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "json", JsonUtility.ToJson(new SpawnData(id, spawnPoint)) }
        };

        MultiplayerManager.Instance.SendMessage("spawn", data);
    }
}
