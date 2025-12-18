using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkSpawner : NetworkBehaviour,IPlayerLeft
{
    [SerializeField] private List<NetworkPrefabRef> playerPref;
    [SerializeField, Range(0, 10)] private float randomSpawnPosition;

    private Dictionary<PlayerRef, NetworkObject> playersInGame = new();

    private void SpawnAllPlayers()
    {
        foreach (var player in Runner.ActivePlayers)
        {
            SpawnPlayer(player);
        }
    }

    public override void Spawned()
    {
        if (!Runner.IsServer)
            return;

        foreach (var player in Runner.ActivePlayers)
        {
            SpawnPlayer(player);
        }
    }

    private void SpawnPlayer(PlayerRef player)
    {
        if (playersInGame.ContainsKey(player))
            return;

        int prefabIndex = Manager.Instance.GetPrefabIndex(player);

        Vector3 spawnPos = new Vector3(
            Random.Range(-randomSpawnPosition, randomSpawnPosition),
            0,
            Random.Range(-randomSpawnPosition, randomSpawnPosition)
        );

        NetworkObject obj = Runner.Spawn(
            playerPref[prefabIndex],
            spawnPos, 
            Quaternion.identity,
            player
        );

        playersInGame[player] = obj;
    }


    public void PlayerLeft(PlayerRef player)
    {
        if (!Runner.IsServer)
            return;

        if (playersInGame.TryGetValue(player, out NetworkObject obj))
        {
            Runner.Despawn(obj);
            playersInGame.Remove(player);
        }
    }
}
