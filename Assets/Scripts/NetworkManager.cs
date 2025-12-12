using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour,INetworkRunnerCallbacks
{
    [SerializeField]
    private NetworkRunner _networkRunner;
    private NetworkRunner _currentnetworkRunner;
    public readonly int _sceneIndex = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public async Task<bool> StartGame(GameMode mode, string RoomID, CancellationToken can)
    {
        try
        {
            // Always create a fresh runner
            _currentnetworkRunner = Instantiate(_networkRunner, transform);
            _currentnetworkRunner.AddCallbacks(this);
            _currentnetworkRunner.ProvideInput = true;

            Debug.Log(mode == GameMode.Host ? "Joined As Host" : "Joined As Client");

            var startTask = _currentnetworkRunner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = RoomID,
                Scene = SceneRef.FromIndex(_sceneIndex),
                SceneManager = GetComponent<NetworkSceneManagerDefault>()
            });

            // ---- CANCELLATION SUPPORT ----
            var completed = await Task.WhenAny(startTask, Task.Delay(Timeout.Infinite, can));

            if (completed != startTask)
                throw new OperationCanceledException(can);

            await startTask;
            return true;
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Start Canceling");
            OnStartCancel();
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }
    private void OnStartCancel()
    {
        if (_currentnetworkRunner)
        {
            _currentnetworkRunner.Shutdown();
            Destroy(_currentnetworkRunner.gameObject);
            _currentnetworkRunner = null;
            Debug.Log("Start Canceled");
        }
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

}
