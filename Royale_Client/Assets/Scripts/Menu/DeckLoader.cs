using System.Collections.Generic;
using UnityEngine;

public class DeckLoader : MonoBehaviour
{
    [SerializeField] private DeckManager _manager;
    [SerializeField] private List<int> _avaliableCards = new List<int>();
    [SerializeField] private int[] _selectedCards = new int[5];

    private void Start()
    {
        StartLoad();
    }

    public void StartLoad()
    {
        Network.Instance.Post(URLLibrary.MAIN + URLLibrary.GETDECKINFO,
            new Dictionary<string, string> { { "userID", UserInfo.Instance.ID.ToString() } },
            SucessLoad, ErrorLoad
            );
    }

    private void ErrorLoad(string error)
    {
        Debug.LogError(error);
        StartLoad();
    }

    private void SucessLoad(string data)
    {
        DeckData deckData = JsonUtility.FromJson<DeckData>(data);

        _selectedCards = new int[deckData.selectedIDs.Length];
        for (int i = 0; i < _selectedCards.Length; i++)
        {
            int.TryParse(deckData.selectedIDs[i], out _selectedCards[i]);
        }

        for (int i = 0; i < deckData.avaliableCards.Length; i++)
        {
            int.TryParse(deckData.avaliableCards[i].id, out int id);
            _avaliableCards.Add(id);
        }

        _manager.Init(_avaliableCards, _selectedCards);
    }
}

[System.Serializable]
public class DeckData
{
    public Avaliablecard[] avaliableCards;
    public string[] selectedIDs;
}

[System.Serializable]
public class Avaliablecard
{
    public string name;
    public string id;
}