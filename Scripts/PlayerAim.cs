using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControlls controlls;
    
    [Header("Aim Info")]
    [SerializeField] private Transform aim;
    
    
    [Header("Camera Info")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private LayerMask aimLayerMask;
    [Range(.5f,1)]
    [SerializeField] private float minCameraDistance;
    [Range(1f,3f)]
    [SerializeField] private float maxCameraDistance;
    [Range(3f,5f)]
    [SerializeField] private float cameraSensitivity;

    private Vector2 aimInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {

        aim.position = GetMouseHitInfo().point;
        aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), cameraSensitivity * Time.deltaTime);
    }

    private Vector3 DesieredCameraPosition()
    {

        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;
        Vector3 desieredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desieredCameraPosition - transform.position).normalized;
        
        float distanceToDesiredPosition = Vector3.Distance(transform.position, desieredCameraPosition);
        float clampledDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, maxCameraDistance);

        desieredCameraPosition = transform.position + aimDirection * clampledDistance;
        desieredCameraPosition.y = transform.position.y + 1;

        return desieredCameraPosition;
    }


    public RaycastHit GetMouseHitInfo()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask)) ;
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }
        return lastKnownMouseHit; 
    }
    private void AssignInputEvents()
    {
        controlls = player.controlls;
        controlls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controlls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
