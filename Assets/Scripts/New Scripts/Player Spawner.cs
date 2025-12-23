using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkObject[] playerPrefabs;

    private NetworkRunner runner;

    void Awake()
    {
        runner = FindAnyObjectByType<NetworkRunner>();
        runner.AddCallbacks(this);
    }

    void OnDestroy()
    {
        if (runner != null)
            runner.RemoveCallbacks(this);
    }

    // ✅ THIS IS THE CORRECT METHOD
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (!runner.IsServer) return;

        foreach (var player in runner.ActivePlayers)
        {
            NetworkObject playerObj = runner.GetPlayerObject(player);

            if (playerObj == null)
            {
                Debug.LogError($"PlayerObject NULL for {player}");
                continue;
            }

            var selection = playerObj.GetComponent<PlayerSelectionData>();
            if (selection == null)
            {
                Debug.LogError("PlayerSelectionData missing!");
                continue;
            }

            int index = selection.SelectedPrefabIndex;

            runner.Spawn(
                playerPrefabs[index],
                Vector3.zero,
                Quaternion.identity,
                player
            );
        }
    }

    // --- Required empty callbacks ---
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, System.Collections.Generic.List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, System.Collections.Generic.Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, float progress) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)  { }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
