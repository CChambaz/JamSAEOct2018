using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    private Transform transform;
    public List<Transform> monstersIsAttacking;
    public Vector3 Position_BottomLeft;

    public Vector2 sizeAera;
    public Vector2 sizeBox = new Vector2(6, 5);

    private float smoothSpeed = 0.125f;

    // Start is called before the first frame update
    void Start()
    {
        transform = this.gameObject.transform;
        monstersIsAttacking = new List<Transform>();
    }

    public enum stateCamera
    {
        Find,
        MoveTo,
    }

    public stateCamera state = stateCamera.Find;

    Vector2 newPositionCamera = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        if (monstersIsAttacking.Count == 0)
        {
            Vector2 position = transform.position;
            Debug.Log(target.position.y - position.y);
            
            Vector2 positionTarget= Vector2.zero;
            //Limit camera X
            if ((position.x - sizeBox.x >= Position_BottomLeft.x || (target.position.x - position.x) > 0) && (position.x + sizeBox.x <= Position_BottomLeft.x + sizeAera.x || (target.position.x - position.x) < 0))
            {
                positionTarget = new Vector2(target.position.x, 0);
                position = new Vector2(positionTarget.x, position.y);
                transform.position = (Vector3)position - Vector3.forward * 10;

            }
            //Limit camera Y
            if ((position.y - sizeBox.y>= Position_BottomLeft.y || (target.position.y - position.y) > 0) && (position.y + sizeBox.y <= Position_BottomLeft.y + sizeAera.y || (target.position.y - position.y) < 0))
            {
                positionTarget = new Vector2(0, target.position.y);
                transform.position = new Vector3(position.x, positionTarget.y, -10);
            }
           
        }
        else
        {
            switch (state)
            {
                case stateCamera.Find:

                    newPositionCamera = Vector2.zero;
                    int i = 0;

                    foreach (var monsterIsAttacking in monstersIsAttacking)
                    {
                        newPositionCamera += Vector2.Lerp(target.position, monsterIsAttacking.position, 0.5f);
                        i++;
                    }

                    newPositionCamera /= i;
                    state = stateCamera.MoveTo;
                    break;

                case stateCamera.MoveTo:
                    Vector3 position = transform.position;
                    transform.position = Vector3.Lerp(position, (Vector3)newPositionCamera - (Vector3.forward * 10), smoothSpeed);
                    //Debug.Log((newPositionCamera - (Vector2)position).magnitude);
                    if ((newPositionCamera - (Vector2)position).magnitude <= smoothSpeed)
                    {
                        state = stateCamera.Find;
                    }
                    break;
            }
        }
    }

    public void AddMonsterIsAttacking(Transform monster)
    {
        monstersIsAttacking.Add(monster);
    }

    public void RemoveMonsterIsAttacking(Transform monster)
    {
        for (int i= 0; i < monstersIsAttacking.Count; i++)
        {
            if (monstersIsAttacking[i] == monster)
            {
                monstersIsAttacking.RemoveAt(i);
            }
        }
    }

    public void AddPlayer(Transform player)
    {
        target = player;
    }

    private void OnDrawGizmos()
    {
        if (newPositionCamera != Vector2.zero)
        {
            Gizmos.DrawSphere(newPositionCamera, 0.2f);
        }

        if (target != null)
        {
            if (transform != null)
            {
                Vector3 position = transform.position;
                //Show Box area piu
                Gizmos.color = Color.red;

                Gizmos.DrawLine(new Vector3(position.x - sizeBox.x, position.y + sizeBox.y, position.z),
                    new Vector3(position.x - sizeBox.x, position.y - sizeBox.y, position.z));
                Gizmos.DrawLine(new Vector3(position.x + sizeBox.x, position.y - sizeBox.y, position.z),
                    new Vector3(position.x + sizeBox.x, position.y + sizeBox.y, position.z));
                if ((position.x - sizeBox.x >= Position_BottomLeft.x || (target.position.x - position.x) > 0) &&
                    (position.x + sizeBox.x <= Position_BottomLeft.x + sizeAera.x ||
                     (target.position.x - position.x) < 0))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }


                Gizmos.DrawLine(new Vector3(position.x, position.y - sizeBox.y / 2, position.z),
                    new Vector3(position.x, position.y + sizeBox.y / 2, position.z));

                if ((position.y - sizeBox.y >= Position_BottomLeft.y || (target.position.y - position.y) > 0) &&
                    (position.y + sizeBox.y <= Position_BottomLeft.y + sizeAera.y ||
                     (target.position.y - position.y) < 0))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawLine(new Vector3(position.x - sizeBox.x/2, position.y, position.z),
                    new Vector3(position.x + sizeBox.x / 2, position.y, position.z));
            }
        }

        //Size Box Aera Camera
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Position_BottomLeft, new Vector3(Position_BottomLeft.x + sizeAera.x, Position_BottomLeft.y, Position_BottomLeft.z));
        Gizmos.DrawLine(Position_BottomLeft, new Vector3(Position_BottomLeft.x, Position_BottomLeft.y + sizeAera.y, Position_BottomLeft.z));
        Gizmos.DrawLine(new Vector3(Position_BottomLeft.x, Position_BottomLeft.y + sizeAera.y, Position_BottomLeft.z), new Vector3(Position_BottomLeft.x + sizeAera.x, Position_BottomLeft.y + sizeAera.y, Position_BottomLeft.z));
        Gizmos.DrawLine(new Vector3(Position_BottomLeft.x + sizeAera.x, Position_BottomLeft.y + sizeAera.y, Position_BottomLeft.z), new Vector3(Position_BottomLeft.x + sizeAera.x, Position_BottomLeft.y, Position_BottomLeft.z));
    }
}

