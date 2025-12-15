using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class NerworkSpwaner : NetworkBehaviour,IPlayerJoined,IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef playerPref;
    [SerializeField] private NetworkPrefabRef demoPref;
    [SerializeField] private NetworkPrefabRef demoPref2;

    [SerializeField,Range(0,10)]
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPrefab();
        }
    }
    private void SpawnPrefab()
    {
        if(Runner.IsServer)
        {
            var spwanPos = new Vector3(Random.Range(-randomeSpawnPosition,randomeSpawnPosition), Random.Range(-randomeSpawnPosition,randomeSpawnPosition), Random.Range(-randomeSpawnPosition, randomeSpawnPosition));
            Runner.Spawn(demoPref, spwanPos,Quaternion.identity);
        }
    }
    private void DeSpawnPrefab(PlayerRef pref)
    {
        if (playersinLobby.TryGetValue(pref,out var obj))
        {
            Runner.Despawn(obj);
            playersinLobby.Remove(pref);
        }
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
            var spwanPos = new Vector3(Random.Range(-randomeSpawnPosition, randomeSpawnPosition), 0, Random.Range(-randomeSpawnPosition, randomeSpawnPosition));
            var cubePlayer = Runner.Spawn(playerPref, spwanPos, Quaternion.identity,player);
            playersinLobby.Add(player, cubePlayer);
        }
    }
}
