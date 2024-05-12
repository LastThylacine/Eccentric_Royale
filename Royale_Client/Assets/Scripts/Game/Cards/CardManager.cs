using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardController[] _cardControllers;
    [SerializeField] private Image _nextCardImage;
    [SerializeField] private int _layerIndex = 6;
    private CardsInGame _cardsInGame;
    private string[] _ids;
    private Camera _camera;
    private List<string> _freeCardsIDs;
    private string _nextCardID;
    private Spawner _spawner;
    private MultiplayerManager _multiplayerManager;

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
        _spawner = FindObjectOfType<Spawner>();

        _ids = new string[_cardControllers.Length];
        _camera = Camera.main;
        _cardsInGame = CardsInGame.Instance;
        _freeCardsIDs = _cardsInGame.GetAllIDs();

        MixList(_freeCardsIDs);

        for (int i = 0; i < _cardControllers.Length; i++)
        {
            string cardID = _freeCardsIDs[0];
            _freeCardsIDs.RemoveAt(0);
            _ids[i] = cardID;
            _cardControllers[i].Init(this, i, _cardsInGame.PlayerDeck[cardID].Sprite);
        }

        SetNextRandom();

        _multiplayerManager.Spawn += Spawn;
    }

    private void SetNextRandom()
    {
        int randomIndex = UnityEngine.Random.Range(0, _freeCardsIDs.Count);
        _nextCardID = _freeCardsIDs[randomIndex];
        _freeCardsIDs.RemoveAt(randomIndex);
        _nextCardImage.sprite = _cardsInGame.PlayerDeck[_nextCardID].Sprite;
    }

    private void MixList(List<string> ids)
    {
        int length = ids.Count;

        int[] arr = new int[length];

        for (int i = 0; i < length; i++) arr[i] = i;

        System.Random rand = new System.Random();
        arr = arr.OrderBy(x => rand.Next()).ToArray();

        string[] tempArr = new string[length];
        for (int i = 0; i < length; i++) tempArr[i] = ids[i];

        for (int i = 0; i < length; i++) ids[i] = tempArr[arr[i]];
    }

    public void Release(int controllerIndex, in Vector3 screenPointPosition)
    {
        if (!TryGetSpawnPoint(screenPointPosition, out Vector3 spawnPoint)) return;

        string id = _ids[controllerIndex];

        _freeCardsIDs.Add(id);
        _ids[controllerIndex] = _nextCardID;
        _cardControllers[controllerIndex].SetSprite(_cardsInGame.PlayerDeck[_nextCardID].Sprite);

        SetNextRandom();

        //_spawner.Spawn(id, spawnPoint, false);

        SendSpawnInfo(in id, in spawnPoint);
    }

    private void SendSpawnInfo(in string id, in Vector3 position)
    {
        CardInfo cardInfo = new CardInfo();

        cardInfo.id = id;

        cardInfo.x = position.x;
        cardInfo.z = position.z;

        cardInfo.playerID = _multiplayerManager.ClientID;

        string json = JsonUtility.ToJson(cardInfo);

        _multiplayerManager.SendMessage("spawn", json);
    }

    private void Spawn(string cardInfoJSON)
    {
        CardInfo cardInfo = JsonUtility.FromJson<CardInfo>(cardInfoJSON);

        Vector3 position = new Vector3(cardInfo.x, 0, cardInfo.z);

        bool isEnemy = _multiplayerManager.ClientID == cardInfo.playerID ? false : true;

        _spawner.Spawn(cardInfo.id, position, isEnemy);
    }

    private bool TryGetSpawnPoint(Vector3 screenPointPosition, out Vector3 spawnPoint)
    {
        Ray ray = _camera.ScreenPointToRay(screenPointPosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.layer == _layerIndex)
        {
            spawnPoint = hit.point;
            spawnPoint.y = 0;
            return true;
        }

        spawnPoint = Vector3.zero;
        return false;
    }

    private void OnDestroy()
    {
        _multiplayerManager.Spawn -= Spawn;
    }

    [System.Serializable]
    private class CardInfo
    {
        public string id;
        public float x;
        public float z;
        public string playerID;
    }
}
