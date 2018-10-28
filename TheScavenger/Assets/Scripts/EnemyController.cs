using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Applydamage a faire
public class EnemyController : MonoBehaviour
{
    int healtPoint = 30;

    private const int FIELD_OF_VIEW = 500;
    private const float DISTANCE_MIN_NODE = 0.25f;

    private const float TIME_BEFORE_TO_JUMP = 0.6f;
    private float counter_timer;

    private Transform target;
    private Vector2 move_monster;
    private float speed = 3.4f;

    private SoundWolfManager sound;

    public enum EnemyState
    {
        Idle,
        Follow,
        Walk,
        Jump,
        Attack,
        Hurt
    }

    [SerializeField] Animator anim;
    public EnemyState state;

    [SerializeField] GameObject player;

    [SerializeField] float distanceRange;
    Vector3 reachPos;
    Rigidbody2D rigid;
    [SerializeField] float jumpForce;
    SpriteRenderer renderer;
    [SerializeField] float attackDistance;
    [SerializeField] float attackSpeed;
    Vector3 basePos;
    bool flipped = false;

    private Vector3 walkDir = new Vector3(1, 0, 0);

    bool isAlive = true;

    [SerializeField] float deltaPosPath;

    TransitionManager transitionManager;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        state = EnemyState.Idle;
        renderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        sound = GetComponent<SoundWolfManager>();
        transitionManager = FindObjectOfType<TransitionManager>();
    }

    // Update is called once per frame
    void Update()
    {

        //Enemy states
        switch (state)
        {
            case EnemyState.Idle:
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance > attackDistance + 2) SearchPlayer();
                break;
            case EnemyState.Follow:
                if (target != null)
                {
                    move_monster = new Vector2(0.0f, 0.0f);
                    if ((target.position - transform.position).magnitude > 4.5f)
                    {
                        move();
                        this.gameObject.transform.position += (Vector3) move_monster * Time.deltaTime * speed;
                    }
                    else
                    {
                        sound.Play(SoundWolfManager.SoundWolf.Attack);
                        state = EnemyState.Attack;
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
                if (distance < 3f)
                {
                    //Attack
                    walkDir = new Vector3(-walkDir.x, 0);
                    reachPos = new Vector3(
                        transform.position.x + (transform.position.x - player.transform.position.x / 2f),
                        transform.position.y + 3f);
                    basePos = transform.position;
                    sound.Play(SoundWolfManager.SoundWolf.Attack);
                    state = EnemyState.Attack;
                 
                }
                else
                    transform.Translate(walkDir * speed);

                break;

            case EnemyState.Jump:
                if (counter_timer < 0.5f)
                {
                    counter_timer += Time.deltaTime;
                    Jump();
                }
                else
                {
                    dir = Vector3.zero;
                    state = EnemyState.Follow;
                    counter_timer = 0.0f;
                }

                break;

            case EnemyState.Attack:
                counter_timer += Time.deltaTime;
                if (counter_timer >= TIME_BEFORE_TO_JUMP)
                {
                    counter_timer = 0.0f;
                    Dir = Vector2.zero;
                    state = EnemyState.Jump;
                }

                break;

            case EnemyState.Hurt:
                counter_timer += Time.deltaTime;
                GetComponent<SpriteRenderer>().color = Color.red;
                transform.position += (Vector3)Dir * jumpForce;
                if (counter_timer > 0.3f)
                {
                    GetComponent<SpriteRenderer>().color = Color.gray;
                    Dir = Vector2.zero;
                    counter_timer = 0.0f;
                    state = EnemyState.Follow;
                }
                break;
               
            default:
                SearchPlayer();
                break;
        }

        Debug.Log("enemyState" + state);
    }

    private void Attack()
    {

        float nextX = Mathf.MoveTowards(gameObject.transform.position.x, target.position.x,
            attackSpeed * Time.deltaTime);
        Vector3 nextPosition = new Vector2(nextX, transform.position.y);
        gameObject.transform.position += nextPosition;

    }

    Vector2 Dir = Vector2.zero;

    public void Jump()
    {
        if (Dir == Vector2.zero)
        {
            Dir = (target.position - transform.position).normalized;
        }
        else
        {
            transform.position += (Vector3) Dir * jumpForce;
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

        if (distance <= distanceRange)
        {

            if (!Sniff())
            {
                TurnToPlayer(false, new Vector3());
                //state = EnemyState.Walk;
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
            if (walkDir.x > 0)
            {
                if (flipped)
                {
                    renderer.flipX = false;
                    flipped = false;
                }

                anim.SetBool("isWalkingH", true);
            }

            if (walkDir.x < 0)
            {
                if (!flipped)
                {
                    renderer.flipX = true;
                    flipped = true;
                }

                anim.SetBool("isWalkingH", true);
            }

            if (walkDir.y > 0)
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
            if ((transform.position - new Vector3((int) Path[0].Position.x, (int) Path[0].Position.y)).sqrMagnitude <=
                DISTANCE_MIN_NODE)
            {
                Path.RemoveAt(0);
            }

            move_monster = (new Vector3((int) Path[0].Position.x, (int) Path[0].Position.y) - transform.position)
                .normalized;
            //this.gameObject.GetComponent<Rigidbody2D>().velocity = (speed) * dir;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("PlayerAttack"))
        {
            Debug.Log("HURT");
            healtPoint -= target.GetComponent<PlayerController>().GetForceAttack();
            counter_timer = 0.0f;
            state = EnemyState.Hurt;
            Dir = (transform.position - target.position).normalized;
            if (healtPoint <= 0 && isAlive)
            {
                isAlive = false;
                Destroy(this, 0.3f);
            }
        }


    }

    private void OnDestroy()
    {
        transitionManager.activeEnemyCount--;
    }
}
