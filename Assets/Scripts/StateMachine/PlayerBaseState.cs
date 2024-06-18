using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Player.Input;

        // input 또는 input.playerActions가 null인지 확인합니다.
        if (input == null)
        {
            Debug.LogError("PlayerInput is null in AddInputActionsCallbacks");
            return;
        }

        if (input.actions == null)
        {
            Debug.LogError("PlayerActions is null in AddInputActionsCallbacks");
            return;
        }

        input.actions["Movement"].canceled += OnMovementCanceled;
        input.actions["Run"].started += OnRunStarted;
        input.actions["Run"].canceled += OnRunCanceled;
        stateMachine.Player.Input.actions["Jump"].started += OnJumpStarted;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Player.Input;

        // input 또는 input.playerActions가 null인지 확인합니다.
        if (input == null)
        {
            Debug.LogError("PlayerInput is null in RemoveInputActionsCallbacks");
            return;
        }

        if (input.actions == null)
        {
            Debug.LogError("PlayerActions is null in RemoveInputActionsCallbacks");
            return;
        }

        input.actions["Movement"].canceled -= OnMovementCanceled;
        input.actions["Run"].started -= OnRunStarted;
        input.actions["Run"].canceled -= OnRunCanceled;
        stateMachine.Player.Input.actions["Jump"].started -= OnJumpStarted;
    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
        // 실행 시 로직 추가
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // 실행 시 로직 추가
    }
    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {

    }
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }
    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        // 물리 업데이트 로직 추가
    }
    public virtual void Update()
    {
        Move();
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    private void ReadMovementInput()
    {
        if (stateMachine.Player.Input != null && stateMachine.Player.Input.actions != null)
        {
            stateMachine.MovementInput = stateMachine.Player.Input.actions["Movement"].ReadValue<Vector2>();
        }
        else
        {
            Debug.LogWarning("Player Input or Player Actions is not initialized.");
        }
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        Rotate(movementDirection);
        Move(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCamTransform.forward;
        Vector3 right = stateMachine.MainCamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Player.Controller.Move(((direction * movementSpeed) +stateMachine.Player.ForceReceiver.Movement)* Time.deltaTime);
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return movementSpeed;
    }
}
