using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;

<<<<<<< HEAD
    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
=======
    public Node(bool _walkable, Vector3 _worldPos)
>>>>>>> parent of 7afab92 (A* Algo with pathfinding)
    {
        walkable = _walkable;
        worldPos = _worldPos;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
