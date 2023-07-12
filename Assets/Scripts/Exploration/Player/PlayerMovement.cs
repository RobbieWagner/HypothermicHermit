using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D body;
    [SerializeField] Animator animator;
    public PlayerControls playerControls;

    Vector2 moveDirection;

    private float movementSpeed;
    public float defaultWalkSpeed = 3f;
    private float currentWalkSpeed;
    bool running;
    public float defaultRunSpeed = 6f;
    private float currentRunSpeed;

    public bool canMove;
    [HideInInspector] public bool hasRecentlyMoved;

    [SerializeField] public MovementSounds movementSounds;

    void Start() 
    {
        canMove = true;
        movementSpeed = defaultWalkSpeed;
        running = false;
        hasRecentlyMoved = false;

        currentRunSpeed = defaultRunSpeed;
        currentWalkSpeed = defaultWalkSpeed;
    }

    void Awake() => playerControls = new PlayerControls();

    public void OnMove(InputValue value) 
    {
        hasRecentlyMoved = true;
        if(canMove)
        {
            //check if the player is coming from a stationary state
            if(animator.GetFloat("vertical") == 0 && animator.GetFloat("horizontal") == 0)
            {
                animator.SetBool("walking", true);
                movementSounds.ToggleMovementSounds(true);
            }

            moveDirection = value.Get<Vector2>();
            animator.SetFloat("vertical", moveDirection.y);
            animator.SetFloat("horizontal", moveDirection.x);

            //check if the player is going to a stationary state
            if(animator.GetFloat("vertical") == 0 && animator.GetFloat("horizontal") == 0)
            {
                animator.SetBool("walking", false);
                movementSounds.ToggleMovementSounds(false);
                hasRecentlyMoved = false;
            }
        }
    }

    public void OnMove(Vector2 move)
    {
        if(canMove)
        {
            //check if the player is coming from a stationary state
            if(animator.GetFloat("vertical") == 0 && animator.GetFloat("horizontal") == 0)
            {
                movementSounds.ToggleMovementSounds(true);
            }

            moveDirection = move;
            animator.SetFloat("vertical", moveDirection.y);
            animator.SetFloat("horizontal", moveDirection.x);

            //check if the player is going to a stationary state
            if(animator.GetFloat("vertical") == 0 && animator.GetFloat("horizontal") == 0)
            {
                movementSounds.ToggleMovementSounds(false);
            }
        }
    }

    public void OnRun(InputValue value)
    {
        animator.SetBool("running", !animator.GetBool("running"));
        running = animator.GetBool("running");
        if(running) 
        {
            movementSounds.ToggleRun(true);
            movementSpeed = currentRunSpeed;
        }
        else 
        {
            movementSounds.ToggleRun(false);
            movementSpeed = currentWalkSpeed;
        }
    }

    private void FixedUpdate()
    {
        if(Manager.Instance.GameState == (int) GameStateEnum.explore)
        body.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
        else body.velocity = Vector2.zero;
    }

    public void ChangePlayerSpeed(float newWalkSpeed, float newRunSpeed)
    {
        currentWalkSpeed = newWalkSpeed;
        currentRunSpeed = newRunSpeed;

        if(running) movementSpeed = currentRunSpeed;
        else movementSpeed = currentWalkSpeed;
    }

    public void ResetSpeeds()
    {
        currentRunSpeed = defaultRunSpeed;
        currentWalkSpeed = defaultWalkSpeed;

        if(running) movementSpeed = currentRunSpeed;
        else movementSpeed = currentWalkSpeed;
    }
}
