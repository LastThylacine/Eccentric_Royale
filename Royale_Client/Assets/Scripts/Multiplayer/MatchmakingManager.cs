using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchmakingManager : MonoBehaviour
{
    [System.Serializable]
    public class Decks
    {
        public string player1ID;
        public string[] player1;
        public string[] player2;
    }

    [SerializeField] private string _gameSceneName = "Game";
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _matchmakingCanvas;
    [SerializeField] private Button _cancelButton;

    public void Subscribe()
    {
        MultiplayerManager.Instance.GetReady += GetReady;
        MultiplayerManager.Instance.StartGame += StartGame;
        MultiplayerManager.Instance.CancelStart += CancelStart;
    }

    public void Unsubscribe()
    {
        MultiplayerManager.Instance.GetReady -= GetReady;
        MultiplayerManager.Instance.StartGame -= StartGame;
        MultiplayerManager.Instance.CancelStart -= CancelStart;
    }

    private void GetReady()
    {
        _cancelButton.interactable = false;
    }

    private void StartGame(string jsonDecks)
    {
        Decks decks = JsonUtility.FromJson<Decks>(jsonDecks);

        string[] playerDeck;
        string[] enemyDeck;

        Debug.Log($"{MultiplayerManager.Instance.ClientID} || {jsonDecks}");

        if (decks.player1ID == MultiplayerManager.Instance.ClientID)
        {
            playerDeck = decks.player1;
            enemyDeck = decks.player2;
        }
        else
        {
            playerDeck = decks.player2;
            enemyDeck = decks.player1;
        }

        CardsInGame.Instance.SetDecks(playerDeck, enemyDeck);

        SceneManager.LoadScene(_gameSceneName);
    }

    private void CancelStart()
    {
        _cancelButton.interactable = true;
    }

    public async void FindOpponent()
    {
        _mainMenuCanvas.SetActive(false);
        _matchmakingCanvas.SetActive(true);

        await MultiplayerManager.Instance.Connect();

        _cancelButton.interactable = true;
    }

    public void CancelFind()
    {
        _matchmakingCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(true);

        MultiplayerManager.Instance.Leave();
    }
}
