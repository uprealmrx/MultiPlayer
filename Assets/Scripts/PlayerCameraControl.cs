using Fusion;
using UnityEngine;

public class PlayerCameraControl : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
        }
    }
}
