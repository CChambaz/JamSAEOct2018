using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] CanvasGroup uiCanvasStatic;
    [SerializeField] CanvasGroup uiCanvasDynamic;
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
    float previousEnergy = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerLife>();
        playerRage = GetComponent<PlayerRage>();
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
        for (int i = 0; i < playerStats.maxTotalArmor; i++)
        {
            if (i >= playerStats.activeArmor)
                StartCoroutine(Fill(shieldList[i], 0));
            else
                StartCoroutine(Fill(shieldList[i], 1));
        }

        previousShield = playerStats.activeArmor;
    }

    void UpdateEnergy()
    {
        float actualEnergy = playerRage.actualEnergy;
        float totalEnergy = playerRage.maxEnergy;

        StartCoroutine(Fill(imgEnergy, actualEnergy / totalEnergy));

        previousEnergy = actualEnergy;
    }

    IEnumerator Fill(Image imageToFill, float fillGoal)
    {
        while (imageToFill.fillAmount != fillGoal)
        {
            imageToFill.fillAmount = Mathf.Lerp(imageToFill.fillAmount, fillGoal, fillSpeed);

            if(imageToFill.fillAmount < 0)
                imageToFill.fillAmount = 0;
            else if(imageToFill.fillAmount > 1)
                imageToFill.fillAmount = 1;

            yield return new WaitForEndOfFrame();
        }
    }
}
