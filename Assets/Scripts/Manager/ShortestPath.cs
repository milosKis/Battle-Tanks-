using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{
    class Node
    {
        public int row, column, distance;
        Node parent;

        public Node(int row, int column, int dist, Node parent)
        {
            this.row = row;
            this.column = column;
            this.distance = dist;
            this.parent = parent;
        }

        public void AddToPath(List<Vector2> path)
        {
            if (parent != null)
            {
                parent.AddToPath(path);
            }

            path.Add(new Vector2(row, column));
        }
    };

    private int[] row = {-1, 0, 0, 1};
    private int[] col = {0, -1, 1, 0};

    private bool isValid(bool[,] hasObstacle, bool[,] visited, int row, int col)
    {
        return (row >= 0) && (row < Constants.NumOfRows) && (col >= 0) && (col < Constants.NumOfColums)
               && !hasObstacle[row, col] && !visited[row, col];
    }

    public List<Vector2> FindShortestPath(bool[,] hasObstacle, int sourceRow, int sourceColumn, int destinationRow, int destinationColumn)
    {
        bool[,] visited = new bool[Constants.NumOfRows, Constants.NumOfColums];
        Queue<Node> queue = new Queue<Node>();

        visited[sourceRow, sourceColumn] = true;
        queue.Enqueue(new Node(sourceRow, sourceColumn, 0, null));
        int minDist = Constants.MaxDistance;
        Node node = null;

        while (queue.Count > 0)
        {
            node = queue.Dequeue();
            int currentRow = node.row;
            int currentColumn = node.column;
            int dist = node.distance;

            if (currentRow == destinationRow && currentColumn == destinationColumn)
            {
                minDist = dist;
                break;
            }

            for (int k = 0; k < 4; k++)
            {
                if (isValid(hasObstacle, visited, currentRow + row[k], currentColumn + col[k]))
                {
                    visited[currentRow + row[k], currentColumn + col[k]] = true;
                    queue.Enqueue(new Node(currentRow + row[k], currentColumn + col[k], dist + 1, node));
                }
            }
        }

        List<Vector2> path = new List<Vector2>();
        if (minDist < Constants.MaxDistance)
        {
            node.AddToPath(path);
        }

        return path;
    }
}
