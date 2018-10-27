using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{

    int money = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void AddMoney(int metal)
    {
        money += metal;
    }

    public int GetMetal()
    {
        return money;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
