using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    private const int DELTA_SIZE = 10;
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        WALL = 0,
        FLOOR = 1
    }


    public LayerMask WallMask;
    private Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;
    private BoardCreator bord_creator;

    Node[,] grid;
    public List<Node> FinalPath;

     public Node[,] GetGride()
     {
        return grid;
     }

    float nodeDiameter;
    private int gridSizeX, gridSizeY;

    public void Init(BoardCreator _bord_creator, int columns, int rows)
    {
        gridWorldSize = new Vector2(columns, rows);
        bord_creator = _bord_creator;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * distance - Vector3.up * distance;
        for(int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + distance) + Vector3.up * (y * nodeDiameter + distance);
                bool Wall = true;
                int[,] tiles = new int[(int)gridWorldSize.x, (int)gridWorldSize.y];
                tiles = bord_creator.Tiles;
                if(tiles[x, y] != (int)TileType.WALL)
                {
                    Wall = false;
                }

                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, nodeRadius);
        
        if(grid != null)
        {
            foreach(Node n in grid)
            {
                if (n.IsWall)
                {
                    Gizmos.color = Color.black;
                }
                else 
                {
                    Gizmos.color = Color.yellow;
                }

                if (n.isVisited)
                {
                    Gizmos.color = Color.cyan;
                }

                if(FinalPath != null)
                {
                    if (FinalPath.Contains(n))
                    {
                        Gizmos.color = Color.red;
                    }
                }

                Gizmos.DrawSphere(n.Position, nodeRadius / DELTA_SIZE);
            }
        }

    }

}
