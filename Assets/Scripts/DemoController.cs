using Fusion;
using Fusion.Sockets;
using NetworkInputs;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : NetworkBehaviour
{
    [SerializeField]
    private GameObject _localSideParent;
    [SerializeField]
    private float _force;
    private Rigidbody _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Spawned()
    {
        if(Object.HasInputAuthority)
        {
            _localSideParent.SetActive(true);
        }
        else
        {
            Destroy(_localSideParent);
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out cubeinput data))
        {
            PlayerMovement(data);
        }
    }
    private void PlayerMovement(cubeinput data)
    {
        var movement = new Vector3(data.horizontalValue, 0, data.verticalValue);
        _rb.AddForce(movement*_force*Runner.DeltaTime);

    }
}
