using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private AvailableDeckUI _availableDeckUI;
    [SerializeField] private SelectedDeckUI _selectedDeckUI;
    [SerializeField] private Button _saveDeckButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Canvas _selectCardsCanvas;

    private List<Card> _availableCards = new List<Card>();
    private List<Card> _selectedCards = new List<Card>();
    private int _selectToggleIndex = 0;
    private Toggle[] _toggles;

    public IReadOnlyList<Card> AvailableCards { get { return _availableCards; } }
    public IReadOnlyList<Card> SelectedCards { get { return _selectedCards; } }

    private void Start()
    {
        _cancelButton.onClick.AddListener(Cancel);
        _saveDeckButton.onClick.AddListener(SaveDeck);

        _toggles = GetComponentsInChildren<Toggle>();
    }

    private void OnEnable()
    {
        _availableCards.Clear();

        for (int i = 0; i < _deckManager.AvailableCards.Count; i++)
        {
            _availableCards.Add(_deckManager.AvailableCards[i]);
        }

        _selectedCards.Clear();

        for (int i = 0; i < _deckManager.SelectedCards.Count; i++)
        {
            _selectedCards.Add(_deckManager.SelectedCards[i]);
        }
    }

    public void SetSelectToggleIndex(int index)
    {
        _selectToggleIndex = index;
    }

    public void SelectCard(int cardID)
    {
        _selectedCards[_selectToggleIndex] = _availableCards[cardID - 1];
        _selectedDeckUI.UpdateCardsList(SelectedCards);
        _availableDeckUI.UpdateCardsList(AvailableCards, SelectedCards);
    }

    private void SaveDeck()
    {
        SelectedID selectedID = new SelectedID();

        for (int i = 0; i < _selectedCards.Count; i++)
        {
            selectedID.selectedID.Add(_selectedCards[i].ID);
        }

        Network.Instance.Post(URLLibrary.MAIN + URLLibrary.SETDECK,
            new Dictionary<string, string> {
                { "userID", "1"/*UserInfo.Instance.ID.ToString()*/ },
                { "selectedID", JsonUtility.ToJson(selectedID)} },
            SucessLoad, ErrorLoad
            );

        _loadingPanel.SetActive(true);
    }

    private void SucessLoad(string data)
    {
        _deckManager.SetSelectedCards(SelectedCards);

        _mainCanvas.gameObject.SetActive(true);
        _selectCardsCanvas.gameObject.SetActive(false);

        for (int i = 0; i < _toggles.Length; i++)
        {
            _toggles[i].isOn = false;
        }

        _toggles[0].isOn = true;

        SetSelectToggleIndex(0);

        _loadingPanel.SetActive(false);
    }

    private void ErrorLoad(string error)
    {
        Debug.LogError(error);
    }

    private void Cancel()
    {
        _availableCards.Clear();

        for (int i = 0; i < _deckManager.AvailableCards.Count; i++)
        {
            _availableCards.Add(_deckManager.AvailableCards[i]);
        }

        _selectedCards.Clear();

        for (int i = 0; i < _deckManager.SelectedCards.Count; i++)
        {
            _selectedCards.Add(_deckManager.SelectedCards[i]);
        }

        _selectedDeckUI.UpdateCardsList(SelectedCards);
        _availableDeckUI.UpdateCardsList(AvailableCards, SelectedCards);

        _mainCanvas.gameObject.SetActive(true);
        _selectCardsCanvas.gameObject.SetActive(false);

        for (int i = 0; i < _toggles.Length; i++)
        {
            _toggles[i].isOn = false;
        }

        _toggles[0].isOn = true;

        SetSelectToggleIndex(0);
    }
}

[System.Serializable]
public class SelectedID
{
    public List<int> selectedID = new List<int>();
}