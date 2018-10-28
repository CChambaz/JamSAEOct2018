using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum LAYERMASKS
{
    WALL = 8,
    ENEMY = 9,
    KEY = 10,
    PLAYER = 11,
    DOOR = 12
}


public class GameManagerSample : MonoBehaviour {
    [SerializeField] private int columns = 20;                                 // The number of columns on the board (how wide it will be).
    [SerializeField] private int rows = 20;

    [SerializeField] Grid grid;
    [SerializeField] BoardCreator board_creator;
    [SerializeField] SpawnManager spawn_manager;

    [SerializeField] Text Level;

    public int GetColumns()
    {
        return columns;
    }

    public int GetRows()
    {
        return rows;
    }

    private void Start()
    {
        board_creator.Init(columns, rows);
        grid.Init(board_creator, columns, rows);
        //spawn_manager.SpawnEnemies(grid);

        //UI
      //  Level.text = Level.text + GameObject.Find("Scoring").GetComponent<ScoringManger>().GetLevel().ToString();
    }
}