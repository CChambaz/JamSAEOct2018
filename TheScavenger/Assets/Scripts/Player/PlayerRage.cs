using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRage : MonoBehaviour
{
    [Header("Rage attribut")]
    [SerializeField] int energyUseRate;
    [SerializeField] int rageMultiplier;
    [SerializeField] public int maxEnergy;

    public int activeRageMultiplier = 1;

    public int actualEnergy;

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

        if (activeRageMultiplier > 1)
            CheckRageStatus();
    }

    public void AddEnergy(int energyAdded)
    {
        actualEnergy += energyAdded;
    }

    void EnableRage()
    {
        // Disable rage if active
        if (activeRageMultiplier > 1)
        {
            activeRageMultiplier = 1;
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
            activeRageMultiplier = 1;

        if (actualEnergy > 0)
            actualEnergy -= energyUseRate;        
    }
}
