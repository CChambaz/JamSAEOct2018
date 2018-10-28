using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class SpawnManager: MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] int maxWolf;
    [SerializeField] public int countWolf = 3;
    [SerializeField] float wolfCountMultiplier;

    Room[] rooms;
    
    Vector2[] positionSpawns;
    TransitionManager transitionManager;

    // Use this for initialization
    void Start () {
        transitionManager = FindObjectOfType<TransitionManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void increaseWolfCount()
    {
        float tmpCount = countWolf;

        tmpCount *= wolfCountMultiplier;

        countWolf = (int)tmpCount;

        if(countWolf > maxWolf)
            countWolf = maxWolf;
    }

    List<Vector2> positionMonsters;
    public void SpawnEnemies(Grid _grid)
    {
        Node[,] grid = _grid.GetGride();

        int counter_enemy = 0;

        while (counter_enemy < countWolf)
        {

             System.Random pseudoRandom = new System.Random(DateTime.Now.Ticks.ToString().GetHashCode());
             int x= pseudoRandom.Next(30);
            
             System.Random pseudoRandom2 = new System.Random(x);
             int y = pseudoRandom2.Next(30);


            if (!grid[x, y].IsWall)
            {
                if (positionMonsters == null)
                {
                    positionMonsters =  new List<Vector2>();
                    Instantiate(enemyPrefab, new Vector2(x, y), Quaternion.identity);
                    positionMonsters.Add(new Vector2(x, y));
                    
                    counter_enemy++;
                    transitionManager.activeEnemyCount++;
                }
                else
                {
                    foreach (var position in positionMonsters)
                    {

                        if (x  != position.x || y != position.y)
                        {
                            Instantiate(enemyPrefab, new Vector2(x, y), Quaternion.identity);
                            positionMonsters.Add(new Vector2(x, y));
                            counter_enemy++;
                            transitionManager.activeEnemyCount++;
                        }
                    }
                }
            }
        }
    }
}
