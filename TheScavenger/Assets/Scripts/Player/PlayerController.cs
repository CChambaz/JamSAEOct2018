using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement attributs")]
    [SerializeField] float moveSpeed;

    [Header("Dash attributs")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCoolDown;

    int movementUp;
    int movementDown;
    int movementRight;
    int movementLeft;

    float dashStartAt = 0;
    float lastDashAt = 0;

    PlayerRage rage;

    public bool isDashing = false;

    private void Start()
    {
        rage = GetComponent<PlayerRage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            if (Time.time > dashStartAt + dashDuration)
                EndDash();
            else
            {
                Dash();
                return;
            }
        }

        Move();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartDash();
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

    void StartDash()
    {
        isDashing = true;

        dashStartAt = Time.time;
    }

    void Dash()
    {
        Vector3 dashVector = new Vector3(movementRight - movementLeft, movementUp - movementDown);

        dashVector *= dashSpeed * rage.activeRageMultiplier;

        transform.position += dashVector * Time.deltaTime;
    }

    void EndDash()
    {
        isDashing = false;

        lastDashAt = Time.time;
    }
}
