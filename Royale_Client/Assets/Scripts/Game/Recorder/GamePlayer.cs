using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    private void Start()
    {
        if (FindObjectOfType<GameRecorder>().IsPlay)
            FindObjectOfType<GameRecorder>().ContinueLoadGame();
    }

    public void SaveGame()
    {
        FindObjectOfType<GameRecorder>().SaveGame();
    }
}
