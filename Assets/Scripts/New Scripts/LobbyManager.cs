using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;

[System.Serializable]
public struct RolePrefab
{
    public string roleName;
    public NetworkObject prefab;
}

public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkObject playerObjectPrefab;
    public NetworkRunner runner;
    public bool isHost = false;

    // Role → Prefab mapping
    public RolePrefab[] rolePrefabsArray;
    [HideInInspector] public Dictionary<string, NetworkObject> rolePrefabs = new Dictionary<string, NetworkObject>();

    // Player roles
    public Dictionary<PlayerRef, string> playerRoles = new Dictionary<PlayerRef, string>();

    // Local assigned role
    public string MyAssignedRole;

    [Header("Panels UI")]
    public GameObject joiningPanel;
    public GameObject lobbyPanel;

    [Header("Lobby UI")]
    public GameObject playerEntryPrefab;       // Assign your PlayerEntryPrefab here
    public Transform playerListContainer;      // Assign your PlayerListContainer here

    private void Awake()
    {
        runner = FindObjectOfType<NetworkRunner>();
        if (runner == null)
        {
            GameObject go = new GameObject("NetworkRunner");
            runner = go.AddComponent<NetworkRunner>();
        }

        DontDestroyOnLoad(runner.gameObject);

        // Convert array → dictionary
        foreach (var r in rolePrefabsArray)
            rolePrefabs[r.roleName] = r.prefab;
    }

    private void Start()
    {
        runner.ProvideInput = true;
        runner.AddCallbacks(this);
    }

    // ----------------
    // Lobby actions
    // ----------------

    public async void CreateLobby()
    {
        isHost = true;
        await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Host,
            SessionName = "MyLobby",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = runner.SceneManager
        });
        joiningPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public async void JoinLobby()
    {
        await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Client,
            SessionName = "MyLobby",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = runner.SceneManager
        });
        joiningPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    // Assign role to player
    public void AssignRole(PlayerRef player, string role)
    {
        if (!isHost) return;

        playerRoles[player] = role;

        // RPC to all; only the target applies it
        RPC_AssignRole(role, player);
    }

    // Start the game → load GameplayScene
    public async void StartGame()
    {
        if (!isHost) return;
        await runner.LoadScene("New Game");

    }

    // ----------------
    // RPC
    // ----------------
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_AssignRole(string role, PlayerRef targetPlayer)
    {
        if (runner.LocalPlayer == targetPlayer)
        {
            MyAssignedRole = role;
            Debug.Log($"Assigned role: {role}");
        }
    }

    // ----------------
    // Callbacks
    // ----------------
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player joined: {player}");

        if (runner.IsServer)
        {
            // Spawn PlayerObject ONCE per player
            NetworkObject playerObj = runner.Spawn(
                playerObjectPrefab,
                Vector3.zero,
                Quaternion.identity,
                player
            );

            // 🔴 THIS LINE IS REQUIRED
            runner.MakeDontDestroyOnLoad(playerObj.gameObject);
            runner.SetPlayerObject(player, playerObj);
        }


        // ---------- UI (local only) ----------
        if (playerEntryPrefab != null && playerListContainer != null)
        {
            GameObject entry = Instantiate(playerEntryPrefab, playerListContainer);
            entry.name = "Player_" + player.PlayerId;

            var text = entry.GetComponentInChildren<UnityEngine.UI.Text>();
            if (text != null)
                text.text = $"Player {player.PlayerId}";

            var buttons = entry.GetComponentInChildren<ButtonsScript>(true);
            if (buttons != null)
            {
                buttons.Init(player);

                if (isHost)
                {
                    buttons.OnButtons(); // 🔥 THIS is what you want
                }
                else
                {
                    buttons.gameObject.SetActive(false);
                }
            }
        }
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (playerRoles.ContainsKey(player))
            playerRoles.Remove(player);

        // Destroy UI entry
        Transform entry = playerListContainer.Find("Player_" + player.PlayerId);
        if (entry != null)
            Destroy(entry.gameObject);
    }


    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
