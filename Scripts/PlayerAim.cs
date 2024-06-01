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
    [Range(.5f,1)]
    [SerializeField] private float minCameraDistance;
    [Range(1f,3f)]
    [SerializeField] private float maxCameraDistance;
    [Range(3f,5f)]
    [SerializeField] private float aimSensitivity;

    private Vector2 aimInput;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = Vector3.Lerp(aim.position, DesieredAimPosition(), aimSensitivity * Time.deltaTime);
    }

    private Vector3 DesieredAimPosition()
    {

        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;
        Vector3 desiredAimPosition = GetMousePosition();
        Vector3 aimDirection = (desiredAimPosition - transform.position).normalized;
        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredAimPosition);
        float clampledDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, maxCameraDistance);

        desiredAimPosition = transform.position + aimDirection * clampledDistance;
        desiredAimPosition.y = transform.position.y + 1;

        return desiredAimPosition;
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
