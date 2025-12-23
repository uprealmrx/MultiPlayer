using Fusion;
using UnityEngine;

public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner runner;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (runner == null)
            runner = gameObject.AddComponent<NetworkRunner>();
    }
}
