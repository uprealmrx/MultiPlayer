using Fusion;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

public class SelectionScene : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    private NetworkSceneManagerDefault _networkSceneManagerDefault;
    public Transform _parentPlayerSelection;
    public NetworkPrefabRef _prefab;

    private Dictionary<PlayerRef, NetworkObject> _playerButtons = new();
    public int PlayerCount;
    public void PlayerJoined(PlayerRef player)
    {
        if (!Runner.IsServer)
            return;

        SpawnPrefab(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (!Runner.IsServer)
            return;

        if (_playerButtons.TryGetValue(player, out NetworkObject button))
        {
            Runner.Despawn(button);
            _playerButtons.Remove(player);
            Manager.Instance.RemovePlayer(player);
            PlayerCount--;
        }
    }

    public void LoadNextScene()
    {
        if (Runner.IsServer)
        {
            Runner.LoadScene("GamePlay 1");
        }
    }
    private void SpawnPrefab(PlayerRef player)
    {
        var button = Runner.Spawn(_prefab, Vector3.zero, Quaternion.identity);

        button.transform.SetParent(_parentPlayerSelection, false);
        var ui = button.GetComponent<ButtonsScript>();
        ui.Init(player);

        LevelData data = new LevelData();
        Manager.Instance.AddPlayer(Runner.LocalPlayer, PlayerCount);

        _playerButtons[player] = button;
        PlayerCount++;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
