using Fusion;
using UnityEngine;

public class SelectionScene : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    private NetworkSceneManagerDefault _networkSceneManagerDefault;
    public Transform _parentPlayerSelection;
    public NetworkPrefabRef _prefab;
    public int PlayerCount;
    public void PlayerJoined(PlayerRef player)
    {
        SpawnPrefab(PlayerCount);
    }

    public void PlayerLeft(PlayerRef player)
    {

    }
    public void  LoadScene()
    {

    }
    private void SpawnPrefab(float f)
    {
        if (Runner.IsServer)
        {
            var button = Runner.Spawn(_prefab, transform.position, Quaternion.identity);
            button.GetComponent<ButtonsScript>()._myIndex = PlayerCount;
            LevelData data = new LevelData();
            Manager.Instance.addtolist(data);
            button.transform.SetParent(_parentPlayerSelection);
        }
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
