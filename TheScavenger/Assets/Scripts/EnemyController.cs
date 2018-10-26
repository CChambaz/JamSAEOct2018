using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   
    enum EnemyState
    {
        Idle,
        Walk,
        Jump,
        Attack
    }
    [SerializeField]
    Animator anim;
    EnemyState state;

    [SerializeField]
    GameObject player;

    [SerializeField]
    float distanceRange;

    [SerializeField]
    float speed;
    SpriteRenderer renderer;
    [SerializeField]
    float attackDistance;

    bool flipped = false;
    private Vector3 walkDir=new Vector3(1,0,0);

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState.Idle;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //Enemy states
        switch (state)
        {
            case EnemyState.Idle:
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance > attackDistance) SearchPlayer();
                break;
            case EnemyState.Walk:
                 distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance< 3f)
                {
                    //Attack
                    walkDir = new Vector3(-walkDir.x, 0);
                    state = EnemyState.Idle;
                }
                else
                transform.Translate(walkDir * speed);
                
                break;
            case EnemyState.Jump:Jump();
                break;
            case EnemyState.Attack:Attack();
                break;
            default:SearchPlayer();
                break;
        }
        Debug.Log("enemyState" + state);
    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    private void Jump()
    {
        throw new NotImplementedException();
    }

  
    //Reste immobile et a une chance sur 4 de "sentir la direction ou se trouve le joueur" si le joueur est loin, sinon il sent automatiquement
    //S'il sent le joueur, il se tourne simplement dans la bonne direction.
    private void SearchPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance<=distanceRange&&distance>attackDistance)
        {
           
            TurnToPlayer();
            state = EnemyState.Walk;
        }
        else
        {
            if (distance > distanceRange)
            {
                if (Sniff())
                {
                    TurnToPlayer();
                    state = EnemyState.Walk;
                }
            }
            


        }

        
    }

    //Se trourne en direction du joueur
    private void TurnToPlayer()
    {

        float playerPosX = player.transform.position.x;
        float playerPosY = player.transform.position.y;
        float distanceX = playerPosX - transform.position.x;
        float distanceY = playerPosY - transform.position.y;

        if (distanceX > 0)
        {
            if (walkDir.x <= 0)
                walkDir.x = 1;

            if(flipped)
            {
                renderer.flipX=false;
                flipped = false;
            }
            anim.SetBool("isWalkingH", true);
        }
        else
        {
            if (distanceX < 0)
                if (walkDir.x >= 0)
                walkDir.x = -1;

            if (!flipped)
            {
                renderer.flipX = true;
                flipped = true;
            }
            anim.SetBool("isWalkingH", true);
        }

            if (distanceY > 0)
            {
            if (walkDir.y <= 0)
                walkDir.y = 1;

            anim.SetBool("isWalkingUp", true);
            anim.SetBool("isWalkingDown", false);
        }
            else
            {
            if (distanceY < 0)
                if (walkDir.y >= 0)
                walkDir.y = -1;

            anim.SetBool("isWalkingUp", false);
            anim.SetBool("isWalkingDown", true);
        }
        }
    

    private bool Sniff()
    {
        UnityEngine.Random.InitState(Mathf.RoundToInt(UnityEngine.Random.Range(1f, 999f)));
        return Mathf.RoundToInt(UnityEngine.Random.Range(1f, 4f)) == 1;
    }

}
