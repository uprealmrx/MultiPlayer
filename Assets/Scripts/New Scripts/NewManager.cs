using Fusion;
using System.Collections.Generic;
using UnityEngine;
public class NewManager : NetworkBehaviour
{
    public static NewManager Instance;

    public Dictionary<PlayerRef, int> playerSelections = new();

    public override void Spawned()
    {
        Instance = this;

        if (Runner.IsServer)
        {
            playerSelections = new Dictionary<PlayerRef, int>(SelectionCache.Selections);
            Debug.Log("Selections transferred to Gameplay");
        }
    }
}
