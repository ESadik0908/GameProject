using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
<<<<<<< HEAD

    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
=======
    [SerializeField] private Transform player;
    public LayerMask terrainMask;
>>>>>>> parent of 7afab92 (A* Algo with pathfinding)
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;

    private int gridSizeX;
    private int gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

<<<<<<< HEAD
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
=======
    private void CreateGrid()
>>>>>>> parent of 7afab92 (A* Algo with pathfinding)
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius)) + (Vector3.up * ( y * nodeDiameter + nodeRadius));
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, terrainMask));
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridSizeX / 2) / gridWorldSize.x;
        float percentY = (worldPos.y + gridSizeY / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

<<<<<<< HEAD
   
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        if (grid != null && displayGridGizmos)
=======
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if(grid != null)
>>>>>>> parent of 7afab92 (A* Algo with pathfinding)
        {
            Node playerNode = NodeFromWorldPoint(player.position);
            foreach (Node n in grid)
            {
<<<<<<< HEAD
                Gizmos.color = (n.walkable) ? Color.white : Color.red;    
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
=======
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (playerNode == n)
                {
                    Debug.Log(playerNode.worldPos);
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
>>>>>>> parent of 7afab92 (A* Algo with pathfinding)
            }
        }
    }
}
<<<<<<< HEAD
=======

>>>>>>> parent of 7afab92 (A* Algo with pathfinding)
