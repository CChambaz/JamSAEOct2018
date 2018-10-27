using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Applydamage a faire
public class EnemyController : MonoBehaviour
{
    Coroutine followCoroutine;
    enum EnemyState
    {
        Idle,
        SmartWalk,
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
    Vector3 reachPos;
    Rigidbody2D rigid;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float speed;
    SpriteRenderer renderer;
    [SerializeField]
    float attackDistance;
    [SerializeField]
    float attackSpeed;
    Vector3 basePos;
    bool flipped = false;
    private Vector3 walkDir=new Vector3(1,0,0);
    [SerializeField]
    float parabolHigh;
    private float gravity_constant=9.8f;

    [SerializeField]
    float deltaPosPath;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
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
                if (distance > attackDistance+2) SearchPlayer();
                break;
            case EnemyState.SmartWalk:
                distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < 3f)
                {
                    //Attack
                   
                    reachPos = new Vector3(transform.position.x + (transform.position.x - player.transform.position.x / 2f), transform.position.y + 3f);
                    basePos = transform.position;
                    state = EnemyState.Jump;
                }
                else
                    transform.Translate(walkDir*speed);
                break;
            case EnemyState.Walk:
                 distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance< 3f)
                {
                    //Attack
                    
                    reachPos = new Vector3(transform.position.x+(transform.position.x -player.transform.position.x / 2f),transform.position.y+3f);
                    basePos = transform.position;
                    state = EnemyState.Jump;
                }
                else
                transform.Translate(walkDir * speed);
                
                break;
            case EnemyState.Jump:
                Jump();
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
       
            float nextX = Mathf.MoveTowards(gameObject.transform.position.x, player.transform.position.x, attackSpeed * Time.deltaTime);
         Vector3  nextPosition = new Vector2(nextX,transform.position.y);
            gameObject.transform.position = nextPosition;
   
    }

    
    public void Jump()
    {
        if (transform.position.x!=reachPos.x)
        {
            float nextX = Mathf.MoveTowards(gameObject.transform.position.x, reachPos.x, jumpForce *  Time.deltaTime);
            float nextY = Mathf.Lerp(basePos.y, reachPos.y, (nextX - basePos.x) / parabolHigh);
            float parabol = parabolHigh * (nextX - basePos.x) * (nextX - reachPos.x) / (-0.25f * Mathf.Pow(parabolHigh, 2));
            Vector2 nextPosition = new Vector2(nextX, nextY + parabol);

            gameObject.transform.position = nextPosition;

        }
        else
        {
            //Attack
            state = EnemyState.Attack;

        }

    }
  
Quaternion LookAt2D(Vector2 forward)
{
    return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg + 180);
}


    


 


  
    //Reste immobile et a une chance sur 4 de "sentir la direction ou se trouve le joueur" si le joueur est loin, sinon il sent automatiquement
    //S'il sent le joueur, il se tourne simplement dans la bonne direction.
    private void SearchPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        
            if (distance<=distanceRange)
            {

                if (!Sniff())
                {
                Debug.Log("STOP");
                if (followCoroutine != null)
                    StopCoroutine(followCoroutine);
                    TurnToPlayer(false, new Vector3());
                    state = EnemyState.Walk;
                }
                else
                {
                Debug.Log("STOP");
                if(followCoroutine==null)
               followCoroutine= StartCoroutine(FollowPath());
                state = EnemyState.SmartWalk;
                 }
            }
            else
            {
            Debug.Log("STOP");
            if (followCoroutine == null)
                followCoroutine = StartCoroutine(FollowPath());
                state = EnemyState.SmartWalk;
            }


        

        
    }


    private IEnumerator FollowPath()
    {
        Debug.Log("coroutineStarted");
        Vector3 position = new Vector3();
        position = transform.position;
        Vector3 current = position;
        List<Vector3> visited = new List<Vector3>() ;
        visited.Add(position);
        while (true)
        {
            current = position;
            foreach (Vector3 nextPos in neighbors(current))
            {
                Debug.Log("x1" + nextPos.x);
                Debug.Log("y1" + nextPos.y);
                foreach (Vector3 visitedPos in visited)
                {
                    Debug.Log("visité"+visited.Count);

                    if (visitedPos != nextPos&&nextPos!=null&& nextPos !=Vector3.zero)
                    {
                        Debug.Log("x2"+nextPos.x);
                        Debug.Log("y2" + nextPos.y);
                        TurnToPlayer(true, nextPos);
                        position = nextPos;
                        visited.Add(nextPos);
                        break;
                    }
                    

                }
               
            }

            yield return new WaitForEndOfFrame();
        }

    }
    private List<Vector3> neighbors(Vector3 current)
    {
        List<Vector3> arrayPos = new List<Vector3>();
        
        Vector3 leftPos = new Vector3(current.x - deltaPosPath, current.y);
        RaycastHit2D rayleft = Physics2D.Raycast(transform.position+Vector3.left * 4f, leftPos, 1f);
        if(rayleft.collider==null|| (rayleft.collider != null && (rayleft.collider.tag == "Player" || rayleft.collider.tag == "Enemy")))
        {
         
            arrayPos.Add(leftPos);
           
        }
        Vector3 rightPos = new Vector3(current.x + deltaPosPath, current.y);
        RaycastHit2D rayRight = Physics2D.Raycast(transform.position+Vector3.right*4f, rightPos, 1f);
        if (rayRight.collider == null || (rayRight.collider != null && (rayRight.collider.tag == "Player" || rayRight.collider.tag == "Enemy")))
        {
            arrayPos.Add(rightPos);
        }
        Vector3 upPos = new Vector3(current.x, current.y + deltaPosPath);
        RaycastHit2D rayUp = Physics2D.Raycast(transform.position+Vector3.up * 4f, upPos, 1f);
        if (rayUp.collider == null || (rayUp.collider != null && (rayUp.collider.tag == "Player" || rayUp.collider.tag == "Enemy")))
        {
            arrayPos.Add(upPos);
        }
        Vector3 downPos = new Vector3(current.x, current.y - deltaPosPath);
        RaycastHit2D rayDown = Physics2D.Raycast(transform.position+Vector3.down * 4f, downPos,1f);
        if (rayDown.collider == null || (rayDown.collider != null && (rayDown.collider.tag == "Player"|| rayDown.collider.tag == "Enemy")))
        {
            arrayPos.Add(downPos);
        }
        Debug.Log("arraypos "+arrayPos.Count);
        return arrayPos;
    }





    //Se trourne en direction du joueur
    private void TurnToPlayer(bool smart, Vector3 pos)
    {
        float playerPosX = player.transform.position.x;
        float playerPosY = player.transform.position.y;
        float distanceX = playerPosX - transform.position.x;
        float distanceY = playerPosY - transform.position.y;

        if (smart)
        {
            walkDir = pos;
            if(walkDir.x>0)
            {
                if (flipped)
                {
                    renderer.flipX = false;
                    flipped = false;
                }
                anim.SetBool("isWalkingH", true);
            }

            if(walkDir.x < 0)
            {
                if (!flipped)
                {
                    renderer.flipX = true;
                    flipped = true;
                }
                anim.SetBool("isWalkingH", true);
            }

            if(walkDir.y>0)
            {
                anim.SetBool("isWalkingUp", true);
                anim.SetBool("isWalkingDown", false);
            }

            if (walkDir.y < 0)
            {
                anim.SetBool("isWalkingUp", false);
                anim.SetBool("isWalkingDown", true);
            }
        }
        else
        {


            if (distanceX > 0)
            {
                if (walkDir.x <= 0)
                    walkDir.x = 1;

                if (flipped)
                {
                    renderer.flipX = false;
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
    }

    private bool Sniff()
    {
        UnityEngine.Random.InitState(Mathf.RoundToInt(UnityEngine.Random.Range(1f, 999f)));
        return Mathf.RoundToInt(UnityEngine.Random.Range(1f, 4f)) == 1;
    }

}
