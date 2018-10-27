using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Inter-Level components")]
    [SerializeField] public CanvasGroup pausePanel;
    [SerializeField] Image increaseLife;
    [SerializeField] Image increaseArmor;

    [Header("Cost values")]
    [SerializeField] int addArmorCost;
    [SerializeField] int addLifeCost;

    [Header("Fade attributs")]
    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeApproximation;
   
    PlayerLife playerLife;
    MainMenu mainMenu;
    InterLevelMenu interMenu;

    private void Start()
    {
        playerLife = FindObjectOfType<PlayerLife>();
        mainMenu = FindObjectOfType<MainMenu>();
        interMenu = FindObjectOfType<InterLevelMenu>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenu.mainMenuCanvas.interactable && !interMenu.interCanvas.interactable)
            SetPause();
    }

    public void SetPause()
    {
        FadePauseMenu(!pausePanel.interactable);
    }

    // Call this function to show or hide the inter-level menu
    public void FadePauseMenu(bool show)
    {
        if (show)
        {
            StartCoroutine(Fade(1));
        }
        else
            StartCoroutine(Fade(0));
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 1;

        mainMenu.FadeMenuUI(true);

        FadePauseMenu(false);
    }

    // Insert in these three function the scrap cost
    public void AddArmor(int amount)
    {
        playerLife.IncreaseArmor(amount, false);
    }

    public void AddLife(int amount)
    {
        playerLife.ChangeLife(amount);
    }

    IEnumerator Fade(float fadeGoal)
    {
        while (pausePanel.alpha != fadeGoal)
        {
            pausePanel.alpha = Mathf.Lerp(pausePanel.alpha, fadeGoal, fadeSpeed);

            if (fadeGoal == 1 && pausePanel.alpha + fadeApproximation >= fadeGoal)
                pausePanel.alpha = fadeGoal;
            else if (fadeGoal == 0 && pausePanel.alpha - fadeApproximation <= fadeGoal)
                pausePanel.alpha = fadeGoal;

            yield return new WaitForEndOfFrame();
        }

        if (fadeGoal == 0)
        {
            pausePanel.interactable = false;
            Time.timeScale = 1;
        }
        else
        {
            pausePanel.interactable = true;
            Time.timeScale = 0;
        }
    }
}
