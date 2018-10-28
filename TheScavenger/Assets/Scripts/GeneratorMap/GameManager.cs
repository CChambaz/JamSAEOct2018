using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class GameManager : MonoBehaviour {
    [SerializeField] private int columns = 20;                                 // The number of columns on the board (how wide it will be).
    [SerializeField] private int rows = 20;

    [SerializeField] Grid grid;
    [SerializeField] BoardCreator board_creator;
    [SerializeField] SpawnManager spawn_manager;

    [SerializeField] Text Level;

    public int getColumns()
    {
        return columns;
    }

    public int getRows()
    {
        return rows;
    }

    private void Start()
    {
        board_creator.Init(columns, rows);
       // grid.Init(board_creator, columns, rows);
      //  spawn_manager.SpawnEnemies(board_creator);

        //UI
      //  Level.text = Level.text + GameObject.Find("Scoring").GetComponent<ScoringManger>().GetLevel().ToString();
    }
}