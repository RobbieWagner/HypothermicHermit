using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CombatCameraMovement : MonoBehaviour
{

    private float cameraZPosition;
    [HideInInspector] public bool canMove;
    [SerializeField] private float cameraSpeed = 5f;
    Vector2 moveDirection;
    Vector2 lowerMovementBounds;
    Vector2 upperMovementBounds;
    PlayerControls playerControls;
    Rigidbody2D body;

    public static CombatCameraMovement Instance {get; private set;}
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        cameraZPosition = transform.position.z;
        canMove = false;
        moveDirection = Vector2.zero;
        playerControls = new PlayerControls();
        body = GetComponent<Rigidbody2D>();

        CombatManager.Instance.OnCreateNewCombat += DeparentCamera;
        CombatManager.Instance.OnEndCombat += CameraFollowPlayer;
    }
    
    private void DeparentCamera()
    {
        transform.parent = null;
        
        List<Vector2> bounds = BattleGrid.Instance.CalculateGridBounds();
        lowerMovementBounds = bounds[0];
        upperMovementBounds = bounds[1];

        //Debug.Log(lowerMovementBounds.ToString());
        //Debug.Log(upperMovementBounds.ToString());
    }

    private void CameraFollowPlayer()
    {
        ResetCamera();
        transform.parent = Player.Instance.transform;
    }

    public void OnMove(InputValue value) 
    {
        if(canMove)
        {
            //check if the player is coming from a stationary state
            moveDirection = value.Get<Vector2>();
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            body.velocity = new Vector2(moveDirection.x * cameraSpeed, moveDirection.y * cameraSpeed);
            if((body.velocity.x > 0 && transform.position.x >= upperMovementBounds.x)
              || body.velocity.x < 0 && transform.position.x <= lowerMovementBounds.x) body.velocity = new Vector2(0, body.velocity.y);
            if((body.velocity.y > 0 && transform.position.y >= upperMovementBounds.y)
              || body.velocity.y < 0 && transform.position.y <= lowerMovementBounds.y) body.velocity = new Vector2(body.velocity.x, 0);
        }
        else body.velocity = Vector2.zero;
    }

    public void ResetCamera()
    {
        bool wasCanMoveTrue = canMove;
        Vector3 position = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, cameraZPosition);
        StartCoroutine(MoveCamera(position, wasCanMoveTrue));
    }

    public void MoveCamera(Vector2 position)
    {
        bool wasCanMoveTrue = canMove;
        canMove = false;
        Vector3 newPosition = new Vector3(position.x, position.y, cameraZPosition);
        StartCoroutine(MoveCamera(newPosition, wasCanMoveTrue));
    }

    private IEnumerator MoveCamera(Vector3 position, bool wasCanMoveTrue, float duration = .4f)
    {
        yield return transform.DOMove(position, duration, false)
            .WaitForCompletion();
        StopCoroutine(MoveCamera(position, wasCanMoveTrue, duration));
        if(wasCanMoveTrue) canMove = true;
    }
}
