using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [Header("Attribut")]
    [SerializeField] int baseLife;
    [SerializeField] int baseArmor;
    [SerializeField] public int maxTotalArmor;

    public int maxLife;
    public int activeLife;

    public int maxArmor;
    public int activeArmor;

    // Start is called before the first frame update
    void Start()
    {
        maxLife = baseLife;
        activeLife = maxLife;

        maxArmor = baseArmor;
        activeArmor = maxArmor;
    }

    public void ChangeLife(int lifeAdded)
    {
        // Take damage
        if (lifeAdded < 0)
        {
            // Damage on the armor
            if(activeArmor > 0)
            {
                activeArmor--;
                return;
            }

            // Damage on life
            activeLife += lifeAdded;
            return;
        }

        // Increase health
        if (activeLife == maxLife)
        {
            // Increase max life
            maxLife = activeLife + lifeAdded;
            activeLife = maxLife;
        }
        else
            activeLife += lifeAdded;

        if (activeLife > maxLife)
            activeLife = maxLife;
    }

    public void IncreaseArmor(int armorAdded, bool canIncreaseMaxArmor)
    {
        if(activeArmor >= maxArmor && maxArmor < maxTotalArmor && canIncreaseMaxArmor)
        {
            maxArmor += armorAdded;
            activeArmor = maxArmor;
            return;
        }

        activeArmor += armorAdded;
    }
}
