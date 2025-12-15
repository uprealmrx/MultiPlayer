using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkSpwaner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private List<NetworkPrefabRef> playerPref;

    [SerializeField, Range(0, 10)]
    private float randomeSpawnPosition;
    public Dictionary<PlayerRef, NetworkObject> playersinLobby;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playersinLobby = new Dictionary<PlayerRef, NetworkObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayerPrefab(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        DeSpawnPrefab(player);
    }
    private void SpawnPlayerPrefab(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            foreach(var r in Manager.Instance.playersinLobby)
            {
                var spwanPos = new Vector3(Random.Range(-randomeSpawnPosition, randomeSpawnPosition), 0, Random.Range(-randomeSpawnPosition, randomeSpawnPosition));
                var cubePlayer = Runner.Spawn(playerPref[r._prefabIndex], spwanPos, Quaternion.identity, player);
                playersinLobby.Add(player, cubePlayer);
            }
        }
    }
    private void DeSpawnPrefab(PlayerRef pref)
    {
        if (playersinLobby.TryGetValue(pref, out var obj))
        {
            Runner.Despawn(obj);
            playersinLobby.Remove(pref);
        }
    }
}
