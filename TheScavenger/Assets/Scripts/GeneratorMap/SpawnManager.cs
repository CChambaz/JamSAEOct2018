using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class SpawnManager: MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    private float countWolf = 5;
    Room[] rooms;
    
    Vector2[] positionSpawns;

    private int counter_enemy = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnEnemies(Grid _grid)
    {
        Node[,] grid = _grid.GetGride();


        while (counter_enemy < countWolf)
        {
                if (counter_enemy < countWolf)
                {

                     System.Random pseudoRandom = new System.Random(DateTime.Now.Ticks.GetHashCode());
                     int x= pseudoRandom.Next(20);
                     System.Random pseudoRandom2 = new System.Random(DateTime.Now.Ticks.GetHashCode());
                     int y = pseudoRandom.Next(20);

                    if (!grid[x, y].IsWall)
                    {
                        GameObject tmp = Instantiate(enemyPrefab, new Vector2(x, y), Quaternion.identity);
                        counter_enemy++;
                }
                    //GameObject tmp = Instantiate(enemyPrefab, new Vector2(room.xPos + (float)Random.Range(0, room.roomWidth - 1), room.yPos + (float)Random.Range(0, room.roomHeight - 1)) - new Vector2(0.5f, 0.5f), Quaternion.identity);
                    //  tmp.GetComponent<Slime>().SetPositionsPatroller(pos_patrol);
                   
                }
        }
    }
}
