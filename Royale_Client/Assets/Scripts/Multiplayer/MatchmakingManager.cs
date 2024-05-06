using UnityEngine;
using UnityEngine.UI;

public class MatchmakingManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _matchmakingCanvas;
    [SerializeField] private Button _cancelButton;

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
