using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterLevelMenu : MonoBehaviour
{
    [Header("Inter-Level components")]
    [SerializeField] public CanvasGroup interCanvas;
    [SerializeField] Image increaseLife;
    [SerializeField] Image increaseArmor;
    [SerializeField] Image increaseDamage;

    [Header("Cost values")]
    [SerializeField] int addArmorCost;
    [SerializeField] int addLifeCost;
    [SerializeField] int addDamageCost;

    [Header("Fade attributs")]
    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeApproximation;

    PlayerController playerController;
    PlayerLife playerLife;

    private void Start()
    {
        playerLife = FindObjectOfType<PlayerLife>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Call this function to show or hide the inter-level menu
    public void FadeInterLevelUI(bool show)
    {
        if (show)
        {
            StartCoroutine(Fade(1));
        }
        else
            StartCoroutine(Fade(0));
    }

    // Insert in these three function the scrap cost
    public void AddArmor(int amount)
    {
        playerLife.IncreaseArmor(amount, true);
    }

    public void AddLife(int amount)
    {
        playerLife.ChangeLife(amount);
    }

    public void AddDamage(int amount)
    {
        playerController.IncreaseDamage(amount);
    }

    IEnumerator Fade(float fadeGoal)
    {
        while(interCanvas.alpha != fadeGoal)
        {
            interCanvas.alpha = Mathf.Lerp(interCanvas.alpha, fadeGoal, fadeSpeed);

            if (fadeGoal == 1 && interCanvas.alpha + fadeApproximation >= fadeGoal)
                interCanvas.alpha = fadeGoal;
            else if (fadeGoal == 0 && interCanvas.alpha - fadeApproximation <= fadeGoal)
                interCanvas.alpha = fadeGoal;

            yield return new WaitForEndOfFrame();
        }

        if (fadeGoal == 0)
            interCanvas.interactable = false;
        else
            interCanvas.interactable = true;
    }
}
