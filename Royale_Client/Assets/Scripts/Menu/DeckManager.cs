using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private List<Card> _availableCards = new List<Card>();
    [SerializeField] private List<Card> _selectedCards = new List<Card>();

    [field: SerializeField] public CardsLibrary Library { get; private set; }
    public IReadOnlyList<Card> AvailableCards { get { return _availableCards; } }
    public IReadOnlyList<Card> SelectedCards { get { return _selectedCards; } }
    public event Action<IReadOnlyList<Card>, IReadOnlyList<Card>> UpdateAvailable;
    public event Action<IReadOnlyList<Card>> UpdateSelected;

    #region Editor
#if UNITY_EDITOR
    [field: SerializeField] public AvailableDeckUI AvaliableDeckUI { get; private set; }
#endif
    #endregion

    public void Init(List<int> availableCardIndexes, int[] selectedCardIndexes)
    {
        for (int i = 0; i < availableCardIndexes.Count; i++)
        {
            _availableCards.Add(Library.Cards[availableCardIndexes[i]]);
        }

        for (int i = 0; i < selectedCardIndexes.Length; i++)
        {
            _selectedCards.Add(Library.Cards[selectedCardIndexes[i]]);
        }

        UpdateAvailable?.Invoke(AvailableCards, SelectedCards);
        UpdateSelected?.Invoke(SelectedCards);

        _loadingCanvas.SetActive(false);
    }

    public void ChangeDeck(IReadOnlyList<Card> selectedCards, Action success)
    {
        _loadingCanvas.SetActive(true);

        int[] IDs = new int[selectedCards.Count];
        for (int i = 0; i < selectedCards.Count; i++)
        {
            IDs[i] = selectedCards[i].ID;
        }

        string json = JsonUtility.ToJson(new Wrapper(IDs));
        string uri = URLLibrary.MAIN + URLLibrary.SETSELECTEDDECK;
        Dictionary<string, string> data = new Dictionary<string, string> {
            { "userID", UserInfo.Instance.ID.ToString() },
            {"json", json }
        };

        success += () =>
        {
            for (int i = 0; i < _selectedCards.Count; i++)
            {
                _selectedCards[i] = selectedCards[i];
            }

            UpdateSelected?.Invoke(SelectedCards);
        };

        Network.Instance.Post(uri, data, (s) => SendSuccess(s, success), Error);
    }

    private void SendSuccess(string obj, Action success)
    {
        if (obj != "ok")
        {
            Error(obj);
            return;
        }

        success?.Invoke();
        _loadingCanvas.SetActive(false);
    }

    private void Error(string obj)
    {
        Debug.LogError("Неудачная попытка отправки новой колоды: " + obj);
        _loadingCanvas.SetActive(false);
    }

    [System.Serializable]
    private class Wrapper
    {
        public int[] IDs;

        public Wrapper(int[] ids)
        {
            this.IDs = ids;
        }
    }
}