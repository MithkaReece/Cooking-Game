using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField] private float movementSpeed;

    private bool isWalking;

    // Update is called once per frame
    void Update(){
        Vector3 movementVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) {
            movementVector.z = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementVector.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementVector.z = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementVector.x = 1;
        }
        movementVector.Normalize();

        transform.position += movementVector * movementSpeed * Time.deltaTime;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movementVector, Time.deltaTime * rotateSpeed);

        isWalking = movementVector != Vector3.zero;
    }

    public bool IsWalking() { return isWalking; }
}
