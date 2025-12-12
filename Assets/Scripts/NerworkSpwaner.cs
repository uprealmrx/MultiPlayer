using Fusion;
using UnityEngine;

public class NerworkSpwaner : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef demoPref;

    [SerializeField,Range(0,10)]
    private float randomeSpawnPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
