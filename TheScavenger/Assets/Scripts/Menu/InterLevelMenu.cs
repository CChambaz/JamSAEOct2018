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
    TransitionManager transitionManager;
    BoardCreator boardCreator;
    PlayerMoney playerInventory;

    private void Start()
    {
        playerLife = FindObjectOfType<PlayerLife>();
        playerController = FindObjectOfType<PlayerController>();
        transitionManager = FindObjectOfType<TransitionManager>();
        boardCreator = FindObjectOfType<BoardCreator>();
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
        playerInventory.AddMoney(-addArmorCost);
    }

    public void AddLife(int amount)
    {
        playerLife.ChangeLife(amount);
        playerInventory.AddMoney(-addLifeCost);
    }

    public void AddDamage(int amount)
    {
        playerController.IncreaseDamage(amount);
        playerInventory.AddMoney(-addDamageCost);
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
        {
            //boardCreator.                             // Call map generation here
            interCanvas.interactable = false;
            transitionManager.gameState = TransitionManager.GameState.INGAME;
        }
        else
            interCanvas.interactable = true;
    }
}
