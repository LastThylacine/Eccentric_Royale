using Colyseus;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    private const string RoomName = "state_handler";
    private ColyseusRoom<State> _room;

    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();
        DontDestroyOnLoad(gameObject);
    }

    public async Task Connect()
    {
        _room = await Instance.client.JoinOrCreate<State>(RoomName);
    }

    public void Leave()
    {
        _room?.Leave();
        _room = null;
    }
}
