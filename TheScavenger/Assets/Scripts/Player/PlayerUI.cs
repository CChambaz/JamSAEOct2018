using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] Canvas globalCanvas;
    [SerializeField] Image imgHealth;
    [SerializeField] Image imgEnergy;
    [SerializeField] List<Image> shieldList;

    [Header("Other attributs")]
    [SerializeField] float fadeDuration;
    [SerializeField] float fillSpeed;
    [SerializeField] float shieldOffset;

    GameObject player;
    PlayerLife playerStats;
    PlayerRage playerRage;

    int previousLife = 0;
    int previousShield = 0;
    int previousEnergy = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerStats = player.GetComponent<PlayerLife>();
        playerRage = player.GetComponent<PlayerRage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousLife != playerStats.activeLife)
            UpdateLife();

        if (previousShield != playerStats.activeArmor)
            UpdateShield();

        if (previousEnergy != playerRage.actualEnergy)
            UpdateEnergy();
    }

    void UpdateLife()
    {
        float actualLife = (float)playerStats.activeLife;
        float totalLife= (float)playerStats.maxLife;

        StartCoroutine(Fill(imgHealth, actualLife / totalLife));

        previousLife = (int)actualLife;
    }

    void UpdateShield()
    {
        if (previousShield < playerStats.activeArmor)
        {
            for (int i = 0; i < playerStats.activeArmor; i++)
            {
                StartCoroutine(Fill(shieldList[i], 1));
            }
        }
        else
        {
            for (int i = 0; i < playerStats.maxArmor; i++)
            {
                if (i > playerStats.activeArmor)
                    StartCoroutine(Fill(shieldList[i], 0));
            }
        }

        previousShield = playerStats.activeArmor;
    }

    void UpdateEnergy()
    {
        float actualEnergy = (float)playerRage.actualEnergy;
        float totalEnergy = (float)playerRage.maxEnergy;

        StartCoroutine(Fill(imgEnergy, actualEnergy / totalEnergy));

        previousEnergy = (int)actualEnergy;
    }

    IEnumerator Fill(Image imageToFill, float fillGoal)
    {
        while (imageToFill.fillAmount != fillGoal)
        {
            imageToFill.fillAmount = Mathf.Lerp(imageToFill.fillAmount, fillGoal, fillSpeed);

            yield return new WaitForEndOfFrame();
        }
    }
}
