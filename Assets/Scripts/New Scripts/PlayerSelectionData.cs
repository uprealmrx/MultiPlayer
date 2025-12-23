using Fusion;

public class PlayerSelectionData : NetworkBehaviour
{
    [Networked] public int SelectedPrefabIndex { get; set; }
}
