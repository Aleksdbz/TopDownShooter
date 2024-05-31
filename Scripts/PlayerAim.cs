using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControlls controlls;
    
    [Header("Aim Info")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;

    private Vector2 aimInput;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = new Vector3(GetMousePosition().x, transform.position.y + 1, GetMousePosition().z);
    }


    public Vector3 GetMousePosition()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask)) ;
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
    private void AssignInputEvents()
    {
        controlls = player.controlls;
        controlls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controlls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
