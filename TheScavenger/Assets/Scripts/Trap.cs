using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    TransitionManager transitionManager;

    bool mapClear;

    // Start is called before the first frame update
    void Start()
    {
        transitionManager = FindObjectOfType<TransitionManager>();
    }

    private void Update()
    {
        if (transitionManager.activeEnemyCount <= 0)
        {
            mapClear = true;
            return;                                     // Insert here the start of the animation
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && mapClear)
            transitionManager.gameState = TransitionManager.GameState.INTERLEVEL;
    }
}
