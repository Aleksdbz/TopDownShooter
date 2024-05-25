
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

  private Player player;
  
  private PlayerControlls controlls;
  private CharacterController CharacterController;
  private Animator animator;
  
  [Header("Movement Info")]
  [SerializeField]  private Vector3 movementdirection;
  [SerializeField] private float walkspeed;
  [SerializeField] private float runspeed;
  private float speed;
  private bool isRunning;

  private float verticalvelocity;
  private float gravityscale = 9.81f;

  [Header("Aim Info")]
  [SerializeField] private Transform aim;
  [SerializeField] private LayerMask aimLayerMask;
  private Vector3 lookingDirection;

  public Vector2 moveInput;
  public Vector2 aimInput;
  
  private void Start()
  {

    player = GetComponent<Player>();
    CharacterController = GetComponent<CharacterController>();
    animator = GetComponentInChildren<Animator>();
    speed = walkspeed;
    AssignInputEvents();
  }

  private void Update()
  {
    ApplyMovement();
    AimTowardsMouse();
    AnimatorController();
  }
  
  private void AnimatorController()
  {
    float VelX = Vector3.Dot(movementdirection.normalized, transform.right);
    float VelY = Vector3.Dot(movementdirection.normalized, transform.forward);
    
    animator.SetFloat("VelX",VelX,.1f,Time.deltaTime);
    animator.SetFloat("VelY",VelY,.1f,Time.deltaTime);
    bool playRunAnimation = isRunning && movementdirection.magnitude > 0;
    animator.SetBool("isRunning", playRunAnimation);
  }

  private void AimTowardsMouse()
  {
    Ray ray = Camera.main.ScreenPointToRay(aimInput);

    if (!Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask)) return;
    lookingDirection = hitInfo.point - transform.position;
    lookingDirection.y = 0f;
    lookingDirection.Normalize();

    transform.forward = lookingDirection;

    aim.position = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
  }

  private void ApplyMovement()
  {
    movementdirection = new Vector3(moveInput.x, 0, moveInput.y);
    ApplyGravity();

    if (movementdirection.magnitude > 0)
    {
      CharacterController.Move(movementdirection * (Time.deltaTime * speed));
    }
  }

  private void ApplyGravity()
  {
    if (!CharacterController.isGrounded)
    {
      verticalvelocity += -gravityscale * Time.deltaTime;
      movementdirection.y = verticalvelocity;
    }
    else
      verticalvelocity = -.5f;
  }
  private void AssignInputEvents()
  {
    controlls = player.controlls;
    controlls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
    controlls.Character.Movement.canceled += context => moveInput = Vector2.zero;
    
    controlls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
    controlls.Character.Aim.canceled += context => aimInput = Vector2.zero;

    controlls.Character.Run.performed += context =>
    {
        speed = runspeed;
        isRunning = true;
    };
    controlls.Character.Run.canceled += context =>
    {
      speed = walkspeed;
      isRunning = false;
    };
  }
  
}
