using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private CardController[] _cardControllers;
    [SerializeField] private Image _nextCardImage;
    [SerializeField] private int _layerIndex = 6;
    private CardsInGame _cardsInGame;
    private string[] _ids;
    private Camera _camera;
    private List<string> _freeCardsIDs;
    private string _nextCardID;

    private void Start()
    {
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

        _spawner.SendSpawn(id, spawnPoint);
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
}
