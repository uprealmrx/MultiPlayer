using Fusion;
using System.Collections.Generic;
using UnityEngine;
public class LocalManager : NetworkBehaviour
{
    public static LocalManager Instance;
    public LobbyManager LobbyManager;
    // Player → Selected Prefab Index
    public Dictionary<PlayerRef, int> playerSelections = new();
    public override void Spawned()
    {
        if (Instance == null)
            Instance = this;
    }
    public void LockMyPlayer(PlayerRef player, string role)
    {
        LobbyManager.AssignRole(player, role);
    }
    public void SetSelection(PlayerRef player, int index)
    {
        if (!Runner.IsServer) return;

        var playerObj = Runner.GetPlayerObject(player);
        if (playerObj == null) return;

        var selection = playerObj.GetComponent<PlayerSelectionData>();
        selection.SelectedPrefabIndex = index;
    }
}
