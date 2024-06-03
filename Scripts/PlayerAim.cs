
using UnityEngine;
/*
    This script handles player aiming mechanics and camera movement based on mouse input.
    Features:
    - Tracks mouse input to determine the point of aim in the game world.
    - Adjusts camera position dynamically based on the aim direction and player movement.
    - Provides functionality to retrieve information about the object aimed at by the player.
*/

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControlls controlls;
    [Header("Aim Visual - Laser")]
    [SerializeField] private LineRenderer aimLaser;
    
    
    [Header("Aim Info")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockingToTarget;
    
    
    [Header("Camera Info")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private LayerMask aimLayerMask;
    [Range(.5f,1)]
    [SerializeField] private float minCameraDistance;
    [Range(1f,3f)]
    [SerializeField] private float maxCameraDistance;
    [Range(3f,5f)]
    [SerializeField] private float cameraSensitivity;

    private Vector2 MouseInput;
    private RaycastHit lastKnownMouseHit;
    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }
    private void Update()
    {
        UpdateAimPosition();
        UpdateCameraPosition();
        UpdateAimVisuals();
    }
    private void UpdateAimVisuals()
    {
        
        Transform gunPoint = player.WeaponController.GunPoint();
        Vector3 laserDirection = player.WeaponController.BulletDirection();
        float justTheTip = 0.5f;
        float gunDistance = 4f;
        
        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            justTheTip = 0;
        }
        aimLaser.SetPosition(0,gunPoint.position);
        aimLaser.SetPosition(1,endPoint);
        aimLaser.SetPosition(2,endPoint + laserDirection * justTheTip);
        
    }
    public Transform Target()
    {
        Transform target = null;
        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }

        return target;
    }
    private void UpdateCameraPosition()
    {
        cameraTarget.position = 
            Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), cameraSensitivity * Time.deltaTime);
    }
    private void UpdateAimPosition()
    {
        Transform target = Target();

        if (target != null && isLockingToTarget)
        {
            aim.position = target.position;
                return;
        }
        aim.position = GetMouseHitInfo().point;
        if(!isAimingPrecisly) aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
    }
    // Calculate the desired camera position based on aim direction and player movement
    private Vector3 DesieredCameraPosition()
    {
        // Adjust maximum camera distance based on player movement
        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;
        Vector3 desieredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desieredCameraPosition - transform.position).normalized;
        
        float distanceToDesiredPosition = Vector3.Distance(transform.position, desieredCameraPosition);
        float clampledDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, maxCameraDistance);
        
        // Calculate the desired camera position based on aim direction
        desieredCameraPosition = transform.position + aimDirection * clampledDistance;
        desieredCameraPosition.y = transform.position.y + 1;

        return desieredCameraPosition;
    }
    public bool CanAimPrecisley() => isAimingPrecisly;
    // Retrieve information about the object aimed at by the player
    public RaycastHit GetMouseHitInfo()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(MouseInput);

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
        controlls.Character.Aim.performed += context => MouseInput = context.ReadValue<Vector2>();
        controlls.Character.Aim.canceled += context => MouseInput = Vector2.zero;
    }
    public Transform Aim() => aim;
}
