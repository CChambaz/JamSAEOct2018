using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRage : MonoBehaviour
{
    [Header("Rage attribut")]
    [SerializeField] int energyUseRate;
    [SerializeField] int rageMultiplier; 

    public int activeRageMultiplier;

    int actualEnergy;

    // Start is called before the first frame update
    void Start()
    {
        actualEnergy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
            EnableRage();

        if (activeRageMultiplier > 0)
            CheckRageStatus();
    }

    public void AddEnergy(int energyAdded)
    {
        actualEnergy += energyAdded;
    }

    void EnableRage()
    {
        // Disable rage if active
        if (activeRageMultiplier > 0)
        {
            activeRageMultiplier = 0;
            return;
        }
        else if (actualEnergy > 0)
        {
            activeRageMultiplier = rageMultiplier;
        }
    }

    void CheckRageStatus()
    {
        if (actualEnergy <= 0)
            activeRageMultiplier = 0;

        if (actualEnergy > 0)
            actualEnergy -= energyUseRate;        
    }
}
