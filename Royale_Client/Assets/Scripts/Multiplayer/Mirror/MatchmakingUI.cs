using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingUI : MonoBehaviour
{
    [SerializeField] private CardsLibrary _library;
    [SerializeField] private Image[] _images;
    [SerializeField] private Button _cancelButton;

    private void Start()
    {
        _cancelButton.interactable = false;

        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].enabled = false;
        }
    }

    public void ClickCancel()
    {
        NetworkManager.singleton.StopClient();
    }

    [Client]
    public void SetImages(string[] cardIDs)
    {
        _cancelButton.interactable = true;

        if (_images.Length != cardIDs.Length)
        {
            Debug.Log($"{_images.Length} != {cardIDs.Length}");
            return;
        }
        for (int i = 0; i < cardIDs.Length; i++)
        {
            int.TryParse(cardIDs[i], out int id);
            _images[i].sprite = _library.GetCardByID(id).Sprite;
            _images[i].enabled = true;
        }
    }

    [Client]
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
