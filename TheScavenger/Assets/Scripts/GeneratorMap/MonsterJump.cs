//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MonsterJump : MonoBehaviour
//{
//    private const int FIELD_Of_VIEW = 10;
//    private const int PRODUCT_CAPESPEED = 5;
//    private const float DISTANCE_MIN_NODE = 0.25f;
//    private const float TIME_FOR_MOVE = 1;
//    private const float TIME_FOR_ATTACK = 0.25f;


//    [SerializeField] private int healt_point = 3;
//    [SerializeField] private float time_for_wait = 2.0f;
//    [SerializeField] private float time_for_jump = 0.5f;
//    private float speed =0.7f;

//    private Vector2 jump_monster;
//    private Vector2 move_monster;
//    private Transform target;
//    [SerializeField] private Transform shadow;
//    [SerializeField] private Transform anim_slime;
//    private Animator anim_monster;

//    private float counter_time;
    
//    private float time_for_hurt = 0.25f;
//    // Use this for initialization
//    void Start()
//    {
//        StartCoroutine(WaitAndSearch(1));
//        target = GameObject.FindGameObjectWithTag("Player").transform;
//        anim_monster = anim_slime.gameObject.GetComponent<Animator>();
//    }

//    enum State
//    {
//        WAIT,
//        JUMP,
//        HURT
//    }
//    State StateMonster = State.WAIT;

//    // Update is called once per frame
//    void Update()
//    {

//        move_monster = new Vector2(0.0f, 0.0f);
//        counter_time += Time.deltaTime;
//        jump_monster = anim_slime.transform.localPosition;
//        switch (StateMonster)
//        {
//            case State.WAIT:
//                if (counter_time > time_for_wait)
//                {
//                    counter_time = 0.0f;
//                    StateMonster = State.JUMP;
//                    anim_monster.SetBool("Is_Jump", true);
//                    anim_slime.localScale = new Vector2(anim_slime.localScale.x -0.05f, anim_slime.localScale.y);
                    
//                }
//                shadow.localPosition = new Vector2(anim_slime.localPosition.x, anim_slime.localPosition.y - 0.3f);
//                break;
//            case State.JUMP:
//                if (counter_time > time_for_jump)
//                {
//                    counter_time = 0.0f;
//                    StateMonster = State.WAIT;
//                    shadow.localPosition = new Vector2(anim_slime.localPosition.x, anim_slime.localPosition.y - 0.3f);
//                    anim_monster.SetBool("Is_Jump", false);
//                    anim_slime.localScale = new Vector2(anim_slime.localScale.x + 0.05f, anim_slime.localScale.y);
//                    shadow.localScale = new Vector2(0.4f,  0.2f);
//                }
//                else
//                {
                    
//                    if (counter_time > (time_for_jump) / 2)
//                    {                     
//                        jump_monster += new Vector2(0.0f, -0.025f) * Time.timeScale;
//                        if(shadow.localScale.x < 1.0f)
//                        {
//                            shadow.localScale = new Vector2(shadow.localScale.x + 0.01f, shadow.localScale.y);
//                            shadow.localPosition = new Vector2(anim_slime.localPosition.x, shadow.localPosition.y+0.025f);

//                        }
//                    }
//                    else
//                    {
//                        jump_monster += new Vector2(0.0f, 0.025f) * Time.timeScale;
//                        if (shadow.localScale.x > 0.1f)
//                        {
//                            shadow.localScale = new Vector2(shadow.localScale.x - 0.01f, shadow.localScale.y);
//                            shadow.localPosition = new Vector2(anim_slime.localPosition.x, shadow.localPosition.y -0.025f);

//                        }
//                    }
//                    move();
//                }
//                anim_slime.transform.localPosition = jump_monster;
//                break;
//            case State.HURT:
                
//                if (counter_time > time_for_hurt)
//                {

//                    StateMonster = State.WAIT;
//                    anim_slime.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
//                }
//                else
//                {
//                    anim_slime.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
//                    move_after_hurt();
//                }
//                break;

//        }
//        this.gameObject.GetComponent<Rigidbody2D>().velocity = move_monster;


//    }

//    List<Node> Path;
//    Vector3 dir;
//    BFS bfs;

//    private void move()
//    {
//        if ((target.transform.position - transform.position).sqrMagnitude < FIELD_Of_VIEW)
//        {
//            if (bfs == null)
//            {
//                bfs = new BFS(GameObject.Find("GameManager").GetComponent<GameManager>().getColumns(), GameObject.Find("GameManager").GetComponent<GameManager>().getRows());
                
//            }
//            Path = bfs.CalculateBFS(GameObject.Find("GameManager").GetComponent<Grid>(), target.transform.position, transform.position);
//            if ((transform.position - new Vector3((int)Path[0].Position.x, (int)Path[0].Position.y)).sqrMagnitude <= DISTANCE_MIN_NODE)
//            {
//                Path.RemoveAt(0);
//            }
//            move_monster = (new Vector3((int)Path[0].Position.x, (int)Path[0].Position.y) - transform.position).normalized;
//            this.gameObject.GetComponent<Rigidbody2D>().velocity = (speed) * dir;
//        }


//        //if (transform.position.x < target.position.x + 0.25 && transform.position.x > target.position.x - 0.25
//        //            || transform.position.y < target.position.y + 0.25 && transform.position.y > target.position.y - 0.25)
//        //{
//        //    if (transform.position.x < target.position.x + 0.25 && transform.position.x > target.position.x - 0.25)
//        //    {
//        //        if (transform.position.y < target.position.y)
//        //        {
//        //            move_monster += new Vector2(0.0f, +(move_velocity*2f));
//        //        }
//        //        if (transform.position.y > target.position.y)
//        //        {
//        //            move_monster += new Vector2(0.0f, -(move_velocity*2f));
//        //        }
//        //    }
//        //    if (transform.position.y < target.position.y + 0.25 && transform.position.y > target.position.y - 0.25)
//        //    {
//        //        if (transform.position.x < target.position.x)
//        //        {
//        //            move_monster += new Vector2(+(move_velocity * 2), 0.0f);
//        //        }
//        //        if (transform.position.x > target.position.x)
//        //        {
//        //            move_monster += new Vector2(-(move_velocity * 2), 0.0f);
//        //        }
//        //    }
//        //}
//        //else
//        //{
//        //    if (transform.position.x > target.position.x && transform.position.y > target.position.y)
//        //    {
//        //        move_monster += new Vector2(-move_velocity, -move_velocity);
//        //    }

//        //    if (transform.position.x < target.position.x && transform.position.y < target.position.y)
//        //    {
//        //        move_monster += new Vector2(+move_velocity, +move_velocity);
//        //    }

//        //    if (transform.position.x > target.position.x && transform.position.y < target.position.y)
//        //    {
//        //        move_monster += new Vector2(-move_velocity, +move_velocity);
//        //    }

//        //    if (transform.position.x < target.position.x && transform.position.y > target.position.y)
//        //    {
//        //        move_monster += new Vector2(+move_velocity, -move_velocity);
//        //    }
//        //}
//    }
//    private void move_after_hurt()
//    {
//        if (transform.position.x < target.position.x + 0.25 && transform.position.x > target.position.x - 0.25
//            || transform.position.y < target.position.y + 0.25 && transform.position.y > target.position.y - 0.25)
//        {
//            if (transform.position.x < target.position.x + 0.25 && transform.position.x > target.position.x - 0.25)
//            {
//                if (transform.position.y < target.position.y)
//                {
//                    move_monster += new Vector2(0.0f, -10.0f);
//                }
//                if (gameObject.transform.position.y > target.position.y)
//                {
//                    move_monster += new Vector2(0.0f, +10.0f);
//                }
//            }
//            if (transform.position.y < target.position.y + 0.25 && transform.position.y > target.position.y - 0.25)
//            {
//                if (transform.position.x < target.position.x)
//                {
//                    move_monster += new Vector2(-10.0f, 0.0f);
//                }
//                if (transform.position.x > target.position.x)
//                {
//                    move_monster += new Vector2(+10.0f, 0.0f);
//                }
//            }
//        }
//        else
//        {
//            if (transform.position.x > target.position.x && transform.position.y > target.position.y)
//            {
//                move_monster += new Vector2(+8.0f, +8.0f);
//            }

//            if (transform.position.x < target.position.x && transform.position.y < target.position.y)
//            {
//                move_monster += new Vector2(-8.0f, -8.0f);
//            }

//            if (transform.position.x > target.position.x && transform.position.y < target.position.y)
//            {
//                move_monster += new Vector2(+8.0f, -8.0f);
//            }

//            if (transform.position.x < target.position.x && transform.position.y > target.position.y)
//            {
//                move_monster += new Vector2(-8.0f, +8.0f);
//            }
//        }
//    }

//    public void take_damage()
//    {
//        healt_point--;
//        shadow.localPosition = new Vector2(anim_slime.localPosition.x, anim_slime.localPosition.y - 0.5f);
//        anim_monster.SetBool("Is_Jump", false);
//        anim_slime.localScale = new Vector2(1, 1);
//        counter_time = 0.0f;
//        StateMonster = State.HURT;
//        if (healt_point <= 0)
//        {
//            destroy_monster();
//        }
//    }

//    private void destroy_monster()
//    {
//        Debug.Log("DIE!!!!!!!!");
//        Destroy(this.gameObject,1);
//    }

//    private IEnumerator WaitAndSearch(float waitTime)
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(waitTime);
//            bfs = new BFS(GameObject.Find("GameManager").GetComponent<GameManager>().getColumns(), GameObject.Find("GameManager").GetComponent<GameManager>().getRows());
//        }
//    }
//}
