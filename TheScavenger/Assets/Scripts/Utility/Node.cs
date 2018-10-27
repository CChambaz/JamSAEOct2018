using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int gridX;
    public int gridY;

    public bool IsWall;
    public bool isVisited;
    public Vector3 Position;

    public Node Parent;

    public int gCost;
    public int hCost;

    public int FCost { get { return gCost + hCost; } }

    public Node(bool a_isWall, Vector3 a_pos, int a_gridX, int a_gridY)
    {
        IsWall = a_isWall;
        Position = a_pos;
        gridX = a_gridX;
        gridY = a_gridY;
        isVisited = false;
    }

    public Node(Vector3 a_pos)
    {
        IsWall = false;
        Position = a_pos;
        isVisited = false;
    }

    public static Node operator -(Node p1, Node p2)
    {
        return new Node(new Vector3(p1.gridX - p2.gridX, p1.gridY - p2.gridY));
    }
    public static Node operator +(Node p1, Node p2)
    {
        return new Node(new Vector3(p1.gridX + p2.gridX, p1.gridY + p2.gridY));
    }

    public static bool operator ==(Node p1, Node p2)
    {
        return p1.gridX == p2.gridX && p1.gridY == p2.gridY;
    }

    public static bool operator !=(Node p1, Node p2)
    {
        return !(p1 == p2);
    }

}
