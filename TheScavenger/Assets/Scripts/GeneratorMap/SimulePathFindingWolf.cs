using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulePathFindingWolf : MonoBehaviour
{
    private const int FIELD_OF_VIEW = 50;
    private const float DISTANCE_MIN_NODE = 0.25f;

    private Transform target;
    private Vector2 move_monster;
    private float speed = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            move_monster = new Vector2(0.0f, 0.0f);
            if ((target.position - transform.position).magnitude > 1.5f)
            {
                move();
                this.gameObject.transform.position += (Vector3) move_monster * Time.deltaTime;
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }

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
}
