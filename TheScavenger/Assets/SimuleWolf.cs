using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimuleWolf : MonoBehaviour
{
    private CameraManager cameraManager;

    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (cameraManager == null)
            {
                cameraManager = Camera.main.GetComponent<CameraManager>();
                cameraManager.AddMonsterIsAttacking(transform);
            }
        }
        else
        {
            if (cameraManager != null)
            {
                cameraManager.RemoveMonsterIsAttacking(transform);
                cameraManager = null;
            }
        }
    }
}
