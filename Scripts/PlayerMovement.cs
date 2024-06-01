
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
  [SerializeField] private float walkspeed;
  [SerializeField] private float runspeed;
  [SerializeField] private float turnspeed;
  
  private float speed;
  private bool isRunning;
  private float verticalvelocity;
  private float gravityscale = 9.81f;
  public Vector2 moveInput { get; private set; }
  private Vector3 movementdirection;
  
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
    ApplyRotation();
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
  private void ApplyRotation()
  {
   
    Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
    lookingDirection.y = 0f;
    lookingDirection.Normalize();

    Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);
    transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnspeed * Time.deltaTime);

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
