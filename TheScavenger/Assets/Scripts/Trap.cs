using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    TransitionManager transitionManager;
    Animator animator;

    bool mapClear;

    // Start is called before the first frame update
    void Start()
    {
        transitionManager = FindObjectOfType<TransitionManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (transitionManager.activeEnemyCount <= 0)
        {
            mapClear = true;

            if (!animator.GetBool("isOpen"))
                animator.SetBool("isOpen", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && mapClear)
            transitionManager.gameState = TransitionManager.GameState.INTERLEVEL;
    }
}
