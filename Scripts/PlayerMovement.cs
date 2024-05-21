using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  private PlayerControlls controlls;
  private CharacterController CharacterController;
  
  [Header("Movement Info")]
  [SerializeField]  private Vector3 movementdirection;
  [SerializeField] private float walkspeed;

  public Vector2 moveInput;
  public Vector2 aimInput;

  private void Awake()
  {
    controlls = new PlayerControlls();
    controlls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
    controlls.Character.Movement.canceled += context => moveInput = Vector2.zero;

    controlls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
    controlls.Character.Aim.canceled += context => aimInput = Vector2.zero;
  }

  private void Start()
  {
    CharacterController = GetComponent<CharacterController>();
  }

  private void Update()
  {
    ApplyMovement();
  }

  private void ApplyMovement()
  {
    movementdirection = new Vector3(moveInput.x, 0, moveInput.y);

    if (movementdirection.magnitude > 0)
    {
      CharacterController.Move(movementdirection * (Time.deltaTime * walkspeed));
    }
  }


  private void OnEnable()
  {
    controlls.Enable();
  }

  private void OnDisable()
  {
    controlls.Disable();
  }
}
