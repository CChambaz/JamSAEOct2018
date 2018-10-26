using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRage : MonoBehaviour
{
    [Header("Rage attribut")]
    [SerializeField] int energyRequired;
    [SerializeField] int energyUseRate;
    [SerializeField] int activeRageMultiplier;

    public int rageMultiplier;

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

        if (rageMultiplier > 0)
            CheckRageStatus();
    }

    public void AddEnergy(int energyAdded)
    {
        actualEnergy += energyAdded;
    }

    void EnableRage()
    {
        // Disable rage if active
        if (rageMultiplier > 0)
        {
            rageMultiplier = 0;
            return;
        }
        else if (actualEnergy > 0)
        {
            rageMultiplier = activeRageMultiplier;
        }
    }

    void CheckRageStatus()
    {
        if (actualEnergy <= 0)
            rageMultiplier = 0;

        if (actualEnergy > 0)
            actualEnergy -= energyUseRate;        
    }
}
