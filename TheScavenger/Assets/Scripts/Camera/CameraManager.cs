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
            Vector2 positionTarget = target.position;
            transform.position = new Vector3(positionTarget.x, positionTarget.y, -10);
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
                    Debug.Log((newPositionCamera - (Vector2)position).magnitude);
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

    private void OnDrawGizmos()
    {
        if (newPositionCamera != Vector2.zero)
        {
            Gizmos.DrawSphere(newPositionCamera, 0.2f);
        }
    }
}

