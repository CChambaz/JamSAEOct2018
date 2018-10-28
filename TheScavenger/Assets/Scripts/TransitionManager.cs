using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public enum GameState
    {
        MAINMENU,
        INGAME,
        INTERLEVEL,
        DEATH
    }

    InterLevelMenu interMenu;
    PlayerLife playerLife;
    MainMenu mainMenu;
    SpawnManager spawnManager;

    public int activeEnemyCount = 0;

    public GameState gameState = GameState.MAINMENU;
    GameState previousGameState = GameState.MAINMENU;

    // Start is called before the first frame update
    void Start()
    {
        interMenu = FindObjectOfType<InterLevelMenu>();
        playerLife = FindObjectOfType<PlayerLife>();
        mainMenu = FindObjectOfType<MainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousGameState != gameState)
            ApplyChangeState();

        if (playerLife.activeLife <= 0)
            gameState = GameState.DEATH;
    }

    void ApplyChangeState()
    {
        previousGameState = gameState;

        switch (gameState)
        {
            case GameState.MAINMENU:
                mainMenu.FadeMenuUI(true);
                break;
            case GameState.INGAME:
                break;
            case GameState.INTERLEVEL:
                spawnManager.increaseWolfCount();
                interMenu.FadeInterLevelUI(true);
                break;
            case GameState.DEATH:
                mainMenu.FadeMenuUI(true);
                break;
        }
    }
}
