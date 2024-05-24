using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerInitializer : MonoBehaviour
{
#if UNITY_SERVER || UNITY_EDITOR
    [Scene, SerializeField] private string _offlineServerScene;
    [Scene, SerializeField] private string _menuScene;
    [SerializeField] private CustomNetworkManager _customNetworkManagerPrefab;
    private void Start()
    {
#if UNITY_SERVER
        StartServer();
#else 
        StartClient();
#endif
    }

    private void StartServer()
    {
        if (!NetworkManager.singleton)
        {
            var manager = Instantiate(_customNetworkManagerPrefab);
            manager.offlineScene = _offlineServerScene;
        }

        NetworkManager.singleton.StartServer();
    }

    private void StartClient()
    {
        if (ParrelSync.ClonesManager.IsClone())
        {
            Debug.Log("This is a clone project.");

            string customArgument = ParrelSync.ClonesManager.GetArgument();

            int.TryParse(customArgument, out int id);

            UserInfo.Instance.SetID(id);

            SceneManager.LoadScene(_menuScene);
        }
    }
#endif
}
