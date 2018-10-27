using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum Orientation
    {
        UP,
        DOWN,
        HORIZONTAL
    }

    [Header("Movement attributs")]
    [SerializeField] float moveSpeed;

    [Header("Dash attributs")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCoolDown;

    [Header("Attack attributs")]
    [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    [SerializeField] float attackCoolDown;
    [SerializeField] BoxCollider2D attackRightCollider;
    [SerializeField] BoxCollider2D attackLeftCollider;
    [SerializeField] BoxCollider2D attackUpCollider;
    [SerializeField] BoxCollider2D attackDownCollider;

    int movementUp;
    int movementDown;
    int movementRight;
    int movementLeft;

    Orientation actualOrientation;

    float dashStartAt = 0;
    float lastDashAt = 0;

    PlayerRage rage;
    Animator animator;
    SpriteRenderer renderer;

    public bool isDashing = false;
    public bool isAttacking = false;
    public bool isMoving = false;

    private void Start()
    {
        rage = GetComponent<PlayerRage>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        actualOrientation = Orientation.HORIZONTAL;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
            return;

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

        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartAttack();

        if(!isMoving)
        {
            animator.SetBool("isWalkingHorizontal", false);
            animator.SetBool("isWalkingUp", false);
            animator.SetBool("isWalkingDown", false);

            switch (actualOrientation)
            {
                case Orientation.UP:
                    animator.SetBool("isIdleUp", true);
                    animator.SetBool("isIdleDown", false);
                    break;
                case Orientation.DOWN:
                    animator.SetBool("isIdleUp", false);
                    animator.SetBool("isIdleDown", true);
                    break;
                case Orientation.HORIZONTAL:
                    animator.SetBool("isIdleUp", false);
                    animator.SetBool("isIdleDown", false);
                    break;
            }
        }
        else
        {
            animator.SetBool("isIdleUp", false);
            animator.SetBool("isIdleDown", false);
        }
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

        isMoving = false;

        // Return if doesn't move
        if (moveVector == Vector3.zero)
            return;

        isMoving = true;

        transform.position += moveVector * Time.deltaTime;

        // Animator managing state
        if(movementRight > 0 || movementLeft > 0)
        {
            if (movementRight > 0)
                renderer.flipX = false;
            else
                renderer.flipX = true;

            animator.SetBool("isWalkingHorizontal", true);
            animator.SetBool("isWalkingUp", false);
            animator.SetBool("isWalkingDown", false);

            actualOrientation = Orientation.HORIZONTAL;
        }
        else if(movementUp > 0)
        {
            animator.SetBool("isWalkingHorizontal", false);
            animator.SetBool("isWalkingUp", true);
            animator.SetBool("isWalkingDown", false);

            actualOrientation = Orientation.UP;
        }
        else if(movementDown > 0)
        {
            animator.SetBool("isWalkingHorizontal", false);
            animator.SetBool("isWalkingUp", false);
            animator.SetBool("isWalkingDown", true);

            actualOrientation = Orientation.DOWN;
        }
    }

    void StartDash()
    {
        isDashing = true;

        animator.SetBool("isWalkingHorizontal", false);
        animator.SetBool("isWalkingUp", false);
        animator.SetBool("isWalkingDown", false);
        animator.SetBool("isDashing", true);

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

        animator.SetBool("isDashing", false);

        lastDashAt = Time.time;
    }

    void StartAttack()
    {
        isAttacking = true;

        animator.SetBool("isAttacking", true);

        switch(actualOrientation)
        {
            case Orientation.UP:
                attackUpCollider.gameObject.SetActive(true);
                break;
            case Orientation.DOWN:
                attackDownCollider.gameObject.SetActive(true);
                break;
            case Orientation.HORIZONTAL:
                if (renderer.flipX)
                    attackLeftCollider.gameObject.SetActive(true);
                else
                    attackRightCollider.gameObject.SetActive(true);
                break;
        }
    }

    void EndAttack()
    {
        Debug.Log("bheu");
        attackUpCollider.gameObject.SetActive(false);
        attackDownCollider.gameObject.SetActive(false);
        attackRightCollider.gameObject.SetActive(false);
        attackLeftCollider.gameObject.SetActive(false);

        animator.SetBool("isAttacking", false);

        isAttacking = false;
    }

    public void IncreaseDamage(int amount)
    {
        attackDamage += amount;
    }
}
