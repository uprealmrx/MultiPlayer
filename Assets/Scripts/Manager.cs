using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class Manager : NetworkBehaviour
{
    public NetworkManager _networkManager; 
    public LobbyUIManager _lobbyUIManager;

    //public LevelData _levelData;

    public static Manager Instance;

    public List<LevelData> playersInLobby = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayer(PlayerRef player, int playerIndex)
    {
        //if (playersInLobby.Exists(p => p.player == player))
        //    return;

        playersInLobby.Add(new LevelData
        {
            player = player,
            _playerIndex = playerIndex
        });
    }

    public void RemovePlayer(PlayerRef player)
    {
        playersInLobby.RemoveAll(p => p.player == player);
    }

    public int GetPrefabIndex(PlayerRef player)
    {
        var data = playersInLobby.Find(p => p.player == player);
        return data != null ? data._prefabIndex : 0;
    }
    public void SetPrefabIndex(PlayerRef player, int index, int myindex)
    {
        foreach (var p in playersInLobby)
        {
           if(p._playerIndex == myindex)
           {
                if (p != null)
                {
                    p._prefabIndex = index;
                }
           }
        }
        //var data = playersInLobby.Find(p => p.player == player);

    }

}
