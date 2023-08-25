using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private bool isWalking;

    // Update is called once per frame
    void Update(){
        Vector3 movementVector = GameInput.GetMovementVectorNormalized();
        transform.position += movementVector * movementSpeed * Time.deltaTime;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movementVector, Time.deltaTime * rotateSpeed);

        isWalking = movementVector != Vector3.zero;
    }

    public bool IsWalking() { return isWalking; }
}
