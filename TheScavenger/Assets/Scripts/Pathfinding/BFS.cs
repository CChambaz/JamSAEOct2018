using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS
{
    Node[] Path;//List of path direction target
    Node[,] grid;// Map

    Node targetNode;
    Node startingNode;

    public BFS(int columns, int rows)
    {
        grid = new Node[columns, rows];
    }

    public List<Node> CalculateBFS(Grid _grid, Vector3 position, Vector3 start_position)
    {
        int sizeX = 20;//Nombre magique
        int sizeY = 20;//MAGIC!!!!!!!! TADADA!
        grid = _grid.GetGride();

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                grid[i, j].isVisited = false;
            }
        }
        targetNode = new Node(new Vector3((int)Mathf.Round(position.x), (int)Mathf.Round(position.y)));
        //targetNode += new Node(new Vector3(sizeX / 2, sizeY / 2));
        if (targetNode.Position.x < 0 || targetNode.Position.x >= sizeX || targetNode.Position.y < 0 || targetNode.Position.y >= sizeY)
        {
            return null;
        }
        startingNode = new Node(new Vector3((int)Mathf.Round(start_position.x), (int)Mathf.Round(start_position.y)));
        startingNode.isVisited = true;

        Queue<Node> tmpNeighbors = new Queue<Node>();
        tmpNeighbors.Enqueue(startingNode);
        Node currentNode = tmpNeighbors.Dequeue();

        //Search a target with BFS
        while (currentNode.Position != targetNode.Position)
        {

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == dy || dx == -dy)
                    {
                        continue;
                    }

                    Node neighborNode = new Node(new Vector3(currentNode.Position.x + dx, currentNode.Position.y + dy));
                    if (neighborNode.Position.x < 0 || neighborNode.Position.x >= sizeX || neighborNode.Position.y < 0 || neighborNode.Position.y >= sizeY)
                    {
                        continue;
                    }

                    if (grid[(int)neighborNode.Position.x, (int)neighborNode.Position.y].isVisited)
                    {
                        continue;
                    }
                    if (!grid[(int)neighborNode.Position.x, (int)neighborNode.Position.y].IsWall)
                    {
                        tmpNeighbors.Enqueue(neighborNode);
                        grid[(int)neighborNode.Position.x, (int)neighborNode.Position.y].Parent = grid[(int)currentNode.Position.x, (int)currentNode.Position.y];
                    }
                    grid[(int)neighborNode.Position.x, (int)neighborNode.Position.y].isVisited = true;
                }
            }
            currentNode = tmpNeighbors.Dequeue();
        }
        // Create List of path direction a target
        List<Node> path = new List<Node>();
        path.Add(targetNode);
        currentNode = path[0];
        while (grid[(int)startingNode.Position.x, (int)startingNode.Position.y] != currentNode)
        {
            path.Add(grid[(int)currentNode.Position.x, (int)currentNode.Position.y].Parent);
            currentNode = grid[(int)currentNode.Position.x, (int)currentNode.Position.y].Parent;
        }
        path.Reverse();
        return path;

    }
}
