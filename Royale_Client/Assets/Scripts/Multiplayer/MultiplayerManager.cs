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
    private const string START_TICK_NAME = "StartTick";
    private const string SPAWN_PLAYER_NAME = "SpawnPlayer";
    private const string SPAWN_ENEMY_NAME = "SpawnEnemy";
    private const string CHEAT_NAME = "Cheat";

    private ColyseusRoom<State> _room;
    public event Action GetReady;
    public event Action<string> StartGame;
    public event Action CancelStart;
    public event Action<string> StartTick;
    public event Action<string> SpawnPlayer;
    public event Action<string> SpawnEnemy;
    public event Action Cheat;

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
        _room.OnMessage<string>(START_TICK_NAME, (tick) => StartTick?.Invoke(tick));
        _room.OnMessage<string>(SPAWN_PLAYER_NAME, (spawnData) => SpawnPlayer?.Invoke(spawnData));
        _room.OnMessage<string>(SPAWN_ENEMY_NAME, (spawnData) => SpawnEnemy?.Invoke(spawnData));
        _room.OnMessage<object>(CHEAT_NAME, (empty) => Cheat?.Invoke());
    }

    public void Leave()
    {
        _room?.Leave();
        _room = null;
    }

    public void SendMessage(string key, Dictionary<string, string> data )
    {
        _room.Send(key, data);
    }
}
