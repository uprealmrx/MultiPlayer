using Fusion;
using UnityEngine;

public class Manager : NetworkBehaviour
{
    public static Manager Instance;
    public NetworkManager _networkManager;
    [Networked, Capacity(16)]
    public NetworkDictionary<PlayerRef, int> PlayerPrefabIndex => default;

    private void Awake()
    {
        Instance = this;
        Debug.Log("✅ Manager Spawned");
        DontDestroyOnLoad(gameObject);
    }
    public override void Spawned()
    {

    }

    // HOST ONLY
    public void AddPlayer(PlayerRef player)
    {
        if (!Object.HasStateAuthority) return;

        if (!PlayerPrefabIndex.ContainsKey(player))
            PlayerPrefabIndex.Add(player, 0);
    }

    // HOST ONLY
    public void RemovePlayer(PlayerRef player)
    {
        if (!Object.HasStateAuthority) return;

        if (PlayerPrefabIndex.ContainsKey(player))
            PlayerPrefabIndex.Remove(player);
    }

    // HOST ONLY
    public void SetPrefabIndex(PlayerRef player, int prefabIndex)
    {
        if (!Object.HasStateAuthority) return;

        if (PlayerPrefabIndex.ContainsKey(player))
            PlayerPrefabIndex.Remove(player);

        PlayerPrefabIndex.Add(player, prefabIndex);
    }

    public int GetPrefabIndex(PlayerRef player)
    {
        return PlayerPrefabIndex.ContainsKey(player)
            ? PlayerPrefabIndex[player]
            : 0;
    }
}
