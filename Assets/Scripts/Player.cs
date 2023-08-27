using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    public event EventHandler OnPickupSomething;

    [SerializeField] private float movementSpeed;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

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
        GameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }
    
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null) 
            selectedCounter.Interact(this);
    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
            selectedCounter.InteractAlternate(this);
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
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                SetSelectedCounter(baseCounter);
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    void SetSelectedCounter(BaseCounter selectedCounter) {
        if (this.selectedCounter == selectedCounter) return;
       
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

    
    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
            OnPickupSomething?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
