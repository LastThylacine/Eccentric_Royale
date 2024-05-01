using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Card[] _cards;
    [SerializeField] private List<Card> _availableCards = new List<Card>();
    [SerializeField] private List<Card> _selectedCards = new List<Card>();

    public IReadOnlyList<Card> AvailableCards { get { return _availableCards; } }
    public IReadOnlyList<Card> SelectedCards { get { return _selectedCards; } }
    public event Action<IReadOnlyList<Card>, IReadOnlyList<Card>> UpdateAvailable;
    public event Action<IReadOnlyList<Card>> UpdateSelected;

    #region Editor
#if UNITY_EDITOR
    [SerializeField] private AvailableDeckUI _avaliableDeckUI;
    private void OnValidate()
    {
        _avaliableDeckUI.SetAllCardsCount(_cards);
    }
#endif
    #endregion

    public void Init(List<int> availableCardIndexes, int[] selectedCardIndexes)
    {
        for (int i = 0; i < availableCardIndexes.Count; i++)
        {
            _availableCards.Add(_cards[availableCardIndexes[i]]);
        }

        for (int i = 0; i < selectedCardIndexes.Length; i++)
        {
            _selectedCards.Add(_cards[selectedCardIndexes[i]]);
        }

        UpdateAvailable?.Invoke(AvailableCards, SelectedCards);
        UpdateSelected?.Invoke(SelectedCards);
    }

    public void SetSelectedCards(IReadOnlyList<Card> cards)
    {
        _selectedCards.Clear();

        for (int i = 0; i < cards.Count; i++)
        {
            _selectedCards.Add(_cards[cards[i].ID]);
        }

        UpdateAvailable?.Invoke(AvailableCards, SelectedCards);
        UpdateSelected?.Invoke(SelectedCards);
    }
}

[System.Serializable]
public class Card
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}