using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu components")]
    [SerializeField] public CanvasGroup mainMenuCanvas;
    [SerializeField] Text deathText;
    [SerializeField] Text startText;

    [Header("Fade attributs")]
    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeApproximation;

    TransitionManager transitionManager;

    private void Start()
    {
        transitionManager = FindObjectOfType<TransitionManager>();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void FadeMenuUI(bool show)
    {
        if (show)
            StartCoroutine(Fade(1));
        else
            StartCoroutine(Fade(0));
    }

    IEnumerator Fade(float fadeGoal)
    {
        while (mainMenuCanvas.alpha != fadeGoal)
        {
            mainMenuCanvas.alpha = Mathf.Lerp(mainMenuCanvas.alpha, fadeGoal, fadeSpeed);

            if (fadeGoal == 1 && mainMenuCanvas.alpha + fadeApproximation >= fadeGoal)
                mainMenuCanvas.alpha = fadeGoal;
            else if (fadeGoal == 0 && mainMenuCanvas.alpha - fadeApproximation <= fadeGoal)
                mainMenuCanvas.alpha = fadeGoal;

            yield return new WaitForEndOfFrame();
        }

        if (fadeGoal == 0)
        {
            mainMenuCanvas.interactable = false;
            transitionManager.gameState = TransitionManager.GameState.INGAME;
        }
        else
        {
            mainMenuCanvas.interactable = true;
            if(transitionManager.gameState == TransitionManager.GameState.DEATH)
            {
                deathText.text = "You died !";
                startText.text = "Retry";
            }
            else
            {
                deathText.text = "";
                startText.text = "Start";
            }
        }
    }
}
