using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class Manager : NetworkBehaviour
{
    public NetworkManager _networkManager; 
    public LobbyUIManager _lobbyUIManager;

    public LevelData _levelData;

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

    // Called in selection scene
    public void AddPlayer(PlayerRef player, int prefabIndex)
    {
        if (playersInLobby.Exists(p => p.player == player))
            return;

        playersInLobby.Add(new LevelData
        {
            player = player,
            _prefabIndex = prefabIndex
        });
    }

    public void RemovePlayer(PlayerRef player)
    {
        playersInLobby.RemoveAll(p => p.player == player);
    }

    // Used in game scene
    public int GetPrefabIndex(PlayerRef player)
    {
        var data = playersInLobby.Find(p => p.player == player);
        return data != null ? data._prefabIndex : 0;
    }
    public void SetPrefabIndex(PlayerRef player, int index)
    {
        var data = playersInLobby.Find(p => p.player == player);
        if (data != null)
        {
            data._prefabIndex = index;
        }
    }

}
