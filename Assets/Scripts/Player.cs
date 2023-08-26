using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float movementSpeed;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDirection;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if(Instance != null) {
            Debug.Log("Multiple players error");
        }
        Instance = this;
    }

    private void Start()
    {
        GameInput.OnInteractAction += GameInput_OnInteractAction;
    }
    
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
            selectedCounter.Interact();
    }

    // Update is called once per frame
    void Update(){

        HandleMovement();
        HandleInteractions();
    }

    void HandleInteractions() {
        Vector3 movementVector = GameInput.GetMovementVectorNormalized();

        if(movementVector != Vector3.zero) 
            lastInteractDirection = movementVector;
        
        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                SetSelectedCounter(clearCounter);
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    void SetSelectedCounter(ClearCounter selectedCounter) {
        if (this.selectedCounter == selectedCounter)
            return;
        this.selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        }); ;
    }

    void HandleMovement() {
        Vector3 movementVector = GameInput.GetMovementVectorNormalized();

        // Try move player
        if (!MovePlayer(movementVector)) {
            // Try x only movement
            if (!MovePlayer(new Vector3(movementVector.x, 0, 0))) {
                // Try z only movement
                MovePlayer(new Vector3(0, 0, movementVector.z));
            }
        }

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movementVector, Time.deltaTime * rotateSpeed);

    }

    bool MovePlayer(Vector3 movementVector) {
        movementVector.Normalize();

        float playerRadius = .7f;
        float playerHeight = 2f;

        if (Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movementVector, movementSpeed * Time.deltaTime)){
            return false; //Collision
        }
        transform.position += movementVector * movementSpeed * Time.deltaTime;
        isWalking = movementVector != Vector3.zero;
        return true;
    }

    public bool IsWalking() { return isWalking; }
}
