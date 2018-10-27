





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollectable : MonoBehaviour
{
   
    public Sprite[] sprites=new Sprite[3
        
        ];
    ObjectClass objet;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        int randomInt = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 2f));

        switch (randomInt)
        {
            case 0: objet.gainType = ObjectClass.GainType.MONEY;
                gameObject.tag = "MONEY";
                break;
            case 1:
                objet.gainType = ObjectClass.GainType.ARMOR;
                gameObject.tag = "ARMOR";
                break;
            case 2:
                objet.gainType = ObjectClass.GainType.ENERGY;
                gameObject.tag = "ENERGY";
                break;
           
        }
       
        GetComponent<SpriteRenderer>().sprite = sprites[randomInt];
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       int gainMoney = Mathf.RoundToInt(UnityEngine.Random.Range(5f, 15f));
        float gainEnergy = UnityEngine.Random.Range(1f, 3f);
        if(collision.tag=="Player")
        {
            switch (objet.gainType)
            {
                case ObjectClass.GainType.MONEY:
                    PlayerMoney pm = collision.gameObject.GetComponent<PlayerMoney>();
                    pm.AddMoney(gainMoney);
                    break;
                case ObjectClass.GainType.ENERGY:
                    PlayerRage pr = collision.gameObject.GetComponent<PlayerRage>();
                    pr.AddEnergy(gainEnergy);
                    break;
                case ObjectClass.GainType.ARMOR:
                    PlayerUI pui = collision.gameObject.GetComponent<PlayerUI>();
                    pui.previousShield=0;
                    break;
                default:
                    break;
            }
        }
    }
}
