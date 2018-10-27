using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Applydamage a faire
public class EnemyController : MonoBehaviour
{
    private const int FIELD_OF_VIEW = 50;
    private const float DISTANCE_MIN_NODE = 0.25f;

    private Transform target;
    private Vector2 move_monster;
    private float speed = 3.4f;

    enum EnemyState
    {
        Idle,
        Follow,
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
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        //Enemy states
        switch (state)
        {
            case EnemyState.Idle:
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance > attackDistance+2) SearchPlayer();
                break;
            case EnemyState.Follow:
                if (target != null)
                {
                    move_monster = new Vector2(0.0f, 0.0f);
                    if ((target.position - transform.position).magnitude > 1.5f)
                    {
                        move();
                        this.gameObject.transform.position += (Vector3)move_monster * Time.deltaTime * speed;
                    }
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Player") != null)
                    {
                        target = GameObject.FindGameObjectWithTag("Player").transform;
                    }
                }
                break;
            case EnemyState.Walk:
                 distance = Vector3.Distance(transform.position, target.position);
                if (distance< 3f)
                {
                    //Attack
                    walkDir = new Vector3(-walkDir.x, 0);
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
        float distance = Vector3.Distance(transform.position, target.position);
        
            if (distance<=distanceRange)
            {

                if (!Sniff())
                {
                    TurnToPlayer(false, new Vector3());
                    state = EnemyState.Walk;
                }
                else
                {
                    state = EnemyState.Follow;
                }
            }
            else
            {
                state = EnemyState.Follow;
            }


        

        
    }


    private void FollowPath()
    {
        Vector3 frontier = new Vector3();
        frontier = transform.position;
        Vector3 current = frontier;
        Vector3[] visited = { frontier };
        while (frontier != null)
        {
            current = frontier;
            foreach (Vector3 next in neighbors(current))
            {
                foreach (Vector3 pos in visited)
                {
                    if (pos != next)
                    {
                        TurnToPlayer(true, pos);
                        frontier = next;
                        visited[visited.Length - 1] = next;
                        break;
                    }
                    if (current != frontier)
                        break;

                }
                
            }

            
        }

    }
    private Vector3[] neighbors(Vector3 current)
    {
        Vector3[] arrayPos = new Vector3[] { };
        
        Vector3 leftPos = new Vector3(current.x - deltaPosPath, current.y);
        RaycastHit2D rayleft = Physics2D.Raycast(transform.position, leftPos);
        if(rayleft.collider==null|| (rayleft.collider != null && rayleft.collider.tag=="Player"))
        {
            arrayPos[arrayPos.Length - 1] = leftPos;
        }
        Vector3 rightPos = new Vector3(current.x + deltaPosPath, current.y);
        RaycastHit2D rayRight = Physics2D.Raycast(transform.position, rightPos);
        if (rayRight.collider == null || (rayRight.collider != null && rayRight.collider.tag == "Player"))
        {
            arrayPos[arrayPos.Length - 1] = rightPos;
        }
        Vector3 upPos = new Vector3(current.x, current.y + deltaPosPath);
        RaycastHit2D rayUp = Physics2D.Raycast(transform.position, upPos);
        if (rayUp.collider == null || (rayUp.collider != null && rayUp.collider.tag == "Player"))
        {
            arrayPos[arrayPos.Length - 1] = upPos;
        }
        Vector3 downPos = new Vector3(current.x, current.y - deltaPosPath);
        RaycastHit2D rayDown = Physics2D.Raycast(transform.position, downPos);
        if (rayDown.collider == null || (rayDown.collider != null && rayDown.collider.tag == "Player"))
        {
            arrayPos[arrayPos.Length - 1] = downPos;
        }
        return arrayPos;
    }





    //Se trourne en direction du joueur
    private void TurnToPlayer(bool smart, Vector3 pos)
    {
        float playerPosX = target.position.x;
        float playerPosY = target.position.y;
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

    //BFS SEARCH
    List<Node> Path;
    Vector3 dir;
    BFS bfs;

    private void move()
    {

        if ((target.transform.position - transform.position).sqrMagnitude < FIELD_OF_VIEW)
        {
            if (bfs == null)
            {
                bfs = new BFS(GameObject.Find("GameCore").GetComponent<GameManagerSample>().GetColumns(),
                    GameObject.Find("GameCore").GetComponent<GameManagerSample>().GetRows());

            }

            Path = bfs.CalculateBFS(GameObject.Find("BoardCreator").GetComponent<Grid>(), target.transform.position,
                transform.position);
            if ((transform.position - new Vector3((int)Path[0].Position.x, (int)Path[0].Position.y)).sqrMagnitude <=
                DISTANCE_MIN_NODE)
            {
                Path.RemoveAt(0);
            }

            move_monster = (new Vector3((int)Path[0].Position.x, (int)Path[0].Position.y) - transform.position).normalized;
            //this.gameObject.GetComponent<Rigidbody2D>().velocity = (speed) * dir;
        }
    }

}
