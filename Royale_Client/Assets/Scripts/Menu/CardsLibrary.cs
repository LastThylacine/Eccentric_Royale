using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsLibrary : MonoBehaviour
{
    [field: SerializeField] public Card[] Cards { get; private set; }

    public bool TryGetDeck(string[] cardsIDs, out Dictionary<string, Card> deck)
    {
        deck = new Dictionary<string, Card>();

        for (int i = 0; i < cardsIDs.Length; i++)
        {
            if (!int.TryParse(cardsIDs[i], out int id) || id == 0) return false;

            Card card = GetCardByID(id);

            if (card == null) return false;

            deck.Add(cardsIDs[i], card);
        }

        return true;
    }

    public Card GetCardByID(int id) => Cards.FirstOrDefault(c => c.ID == id);
}

[System.Serializable]
public class Card
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Unit Unit { get; private set; }
    [field: SerializeField] public GameObject Hologram { get; private set; }
}