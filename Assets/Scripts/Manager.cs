using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class Manager : NetworkBehaviour
{
    public static Manager Instance;

    public NetworkManager _networkManager;
    public LobbyUIManager _lobbyUIManager;
    public LevelData _levelData;
    public List<LevelData> playersinLobby;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        playersinLobby = new List<LevelData>();
    }
    public void addtolist(LevelData data)
    {
        playersinLobby.Add(data);
    }
    public void removetolist(LevelData data)
    {
        playersinLobby.Remove(data);
    }
}
