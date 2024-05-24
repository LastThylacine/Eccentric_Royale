using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvas;

    public void Click()
    {
        _mainMenuCanvas.SetActive(false);
        NetworkManager.singleton.StartClient();
    }
}
