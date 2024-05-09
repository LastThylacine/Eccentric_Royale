using Colyseus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    private const string ROOM_NAME = "state_handler";
    private const string GET_READY_NAME = "GetReady";
    private const string START_GAME_NAME = "Start";
    private const string CANCEL_START_NAME = "CancelStart";

    private ColyseusRoom<State> _room;
    public event Action GetReady;
    public event Action<string> StartGame;
    public event Action CancelStart;

    public string ClientID
    {
        get
        {
            if (_room == null) return "";
            else return _room.SessionId;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();
        DontDestroyOnLoad(gameObject);
    }

    public async Task Connect()
    {
        _room = await Instance.client.JoinOrCreate<State>(ROOM_NAME, new Dictionary<string, object> { { "id", UserInfo.Instance.ID } });

        _room.OnMessage<object>(GET_READY_NAME, (empty) => GetReady?.Invoke());
        _room.OnMessage<string>(START_GAME_NAME, (jsonDecks) => StartGame?.Invoke(jsonDecks));
        _room.OnMessage<object>(CANCEL_START_NAME, (empty) => CancelStart?.Invoke());
    }

    public void Leave()
    {
        _room?.Leave();
        _room = null;
    }
}
