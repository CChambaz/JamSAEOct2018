using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement attributs")]
    [SerializeField] float moveSpeed;

    int movementUp;
    int movementDown;
    int movementRight;
    int movementLeft;

    PlayerRage rage;

    private void Start()
    {
        rage = GetComponent<PlayerRage>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
            movementUp = 1;
        else
            movementUp = 0;

        if (Input.GetKey(KeyCode.S))
            movementDown = 1;
        else
            movementDown = 0;

        if (Input.GetKey(KeyCode.D))
            movementRight = 1;
        else
            movementRight = 0;

        if (Input.GetKey(KeyCode.A))
            movementLeft = 1;
        else
            movementLeft = 0;

        Vector3 moveVector = new Vector3(movementRight - movementLeft, movementUp - movementDown);

        moveVector *= moveSpeed * rage.activeRageMultiplier;

        if (moveVector != Vector3.zero)
            transform.position += moveVector * Time.deltaTime;
    }
}
