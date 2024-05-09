using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class CardsInGame : MonoBehaviour
{
    public static CardsInGame Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public ReadOnlyDictionary<string, Card> PlayerDeck { get; private set; }
    public ReadOnlyDictionary<string, Card> EnemyDeck { get; private set; }

    public void SetDecks(string[] playerCards, string[] enemyCards)
    {
        DeckManager deckManager = FindObjectOfType<DeckManager>();

        bool player = deckManager.TryGetDeck(playerCards, out Dictionary<string, Card> playerDeck);
        bool enemy = deckManager.TryGetDeck(enemyCards, out Dictionary<string, Card> enemyDeck);

        if (!player || !enemy)
        {
            Debug.LogError($"Ќе удалось загрузить какую-то колоду player = {player} | enemy = {enemy}");
        }

        PlayerDeck = new ReadOnlyDictionary<string, Card>(playerDeck);
        EnemyDeck = new ReadOnlyDictionary<string, Card>(enemyDeck);
    }

    public List<string> GetAllIDs() => PlayerDeck.Keys.ToList();
}
