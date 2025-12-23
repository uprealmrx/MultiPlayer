using Fusion;
using UnityEngine;

public class RoleAssignmentObject : NetworkBehaviour
{
    [Networked] public string Role { get; set; }

    // Host calls this RPC, but sends the PlayerRef of the target
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_AssignRole(string role, PlayerRef targetPlayer)
    {
        // Only the intended player applies the role
        if (Runner.LocalPlayer == targetPlayer)
        {
            Role = role;
            Debug.Log($"My role assigned: {Role}");
        }
    }
}
