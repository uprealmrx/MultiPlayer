using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public NetworkManager _networkManager;
    public LobbyUIManager _lobbyUIManager;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
