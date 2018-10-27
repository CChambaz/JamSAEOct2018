using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterLevelMenu : MonoBehaviour
{
    [Header("Inter-Level components")]
    [SerializeField] CanvasGroup interCanvas;
    [SerializeField] Image increaseLife;
    [SerializeField] Image increaseArmor;
    [SerializeField] Image increaseDamage;

    [Header("Fade attributs")]
    [SerializeField] float fadeSpeed;

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

    public void AddArmor(int amount)
    {
        playerLife.IncreaseArmor(amount, true);
    }

    IEnumerator Fade(float fadeGoal)
    {
        if(fadeGoal > 0)
            interCanvas.gameObject.SetActive(true);

        while(interCanvas.alpha != fadeGoal)
        {
            interCanvas.alpha = Mathf.Lerp(interCanvas.alpha, fadeGoal, fadeSpeed);

            yield return new WaitForEndOfFrame();
        }

        if (fadeGoal == 0)
            interCanvas.gameObject.SetActive(false);
    }
}
