using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {

        int step = 0;
        List<Node> Grid = new List<Node>();
        Node[,] gridArray = new Node[grid.GetLength(0), grid.GetLength(1)];


        List<Node> OpenList = new List<Node>();
        List<Node> ClosedList = new List<Node>();
        List<Node> Pathlist = new List<Node>();

        List<Vector2Int> path = new List<Vector2Int>();
        //List<Node> neighbours = new List<Node>();


        Node startNode = new Node(startPos, null, 0, nodeDistance(startPos, endPos));

        Debug.Log(" StartPos: " + startPos + " EndPos: " + endPos + "Grid Count: " + gridArray.Length);

        //1. Set Nodes + HScore
        foreach (Cell node in grid)
        {
            Grid.Add(new Node(node.gridPosition, null, 0, nodeDistance(node.gridPosition,endPos)));

            gridArray[node.gridPosition.x, node.gridPosition.y] = new Node(node.gridPosition, null, 0, nodeDistance(node.gridPosition, endPos));
        }
        //Set Start Node
        OpenList.Add(startNode);

        

        while (OpenList.Count != 0)
        {
            
            //3. Get lowest Fscore
            List<float> FScore = new List<float>();
            for (int i = 0; i < OpenList.Count; i++)
            {
                FScore.Add(OpenList[i].FScore);
            }

            float minFScore = FScore.Min();
            int FScoreIndex = FScore.ToList().IndexOf(minFScore);

            Node current = OpenList[FScoreIndex];
            

            if (current.position == endPos)//Check if Node is EndPos - Return path if so
            {

                Pathlist.Add(current);

                while (path.Count == 0)
                {

                    if (Pathlist[Pathlist.Count - 1].parent != null)
                    {
                        Pathlist.Add(Pathlist[Pathlist.Count - 1].parent);
                    }
                    else
                    {
                        foreach (Node node in Pathlist)
                        {
                            path.Add(node.position);
                        }
                    }
                }


                path.Reverse();
                Debug.Log("OpenList Count: " + OpenList.Count + " Steps:" + step + " Current: " + current.position + " End: " + endPos);
                return path;
            }
            step += 1;

            OpenList.Remove(current);
            ClosedList.Add(current);

            OpenList = CheckNeighbours(GetNeighbours(current, gridArray), OpenList, ClosedList, current, startNode, grid);


        }
        Debug.Log("No path Availible. Steps:" + step);
        return null;
        
    }
    
    public List<Node> GetNeighbours(Node current, Node[,] grid)
    {
        List<Node> neighbours = new List<Node>();

        if (inBoundsX(current, 1, grid) == true) { neighbours.Add(grid[current.position.x + 1, current.position.y]); }
        if (inBoundsX(current, -1, grid) == true) { neighbours.Add(grid[current.position.x - 1, current.position.y]); }
        if (inBoundsY(current, 1, grid) == true) { neighbours.Add(grid[current.position.x, current.position.y + 1]); }
        if (inBoundsY(current, -1, grid) == true) { neighbours.Add(grid[current.position.x, current.position.y - 1]); }

        return neighbours;
    }

    private bool inBoundsX(Node current,int val, Node[,] grid)
    {
        if((current.position.x + val) < 0 || (current.position.x + val) >= grid.GetLength(0))
        {
            return false;
        }
        return true;
    }
    private bool inBoundsY(Node current, int val, Node[,] grid)
    {
        if ((current.position.y + val) < 0 || (current.position.y + val) >= grid.GetLength(1))
        {
            return false;
        }
        return true;
    }


    public List<Node> CheckNeighbours(List<Node> nodeList, List<Node> openList, List<Node> closedList, Node currNode, Node startPos,Cell[,] grid)
    {
        List<Node> returnList = new List<Node>();
        foreach (Node node in nodeList)//Add neigbours to List IF there is no wall around currNode Node
        {
            Debug.Log("Node Pos:"+ node.position);
            Debug.Log(" CurrNode: " + currNode.position);
            Debug.Log(" NeighbourCount: " + nodeList.Count);

            if (node.position == (currNode.position + new Vector2Int(1, 0)))//Right Neighbour
            {
                if (!grid[currNode.position.x, currNode.position.y].walls.HasFlag(Wall.RIGHT))
                {
                    if (!closedList.Contains(node))
                    {
                        returnList.Add(node);
                    }
                }
            }
            if (node.position == (currNode.position - new Vector2Int(1, 0)))//Left Neighbour
            {
                if (!grid[currNode.position.x, currNode.position.y].walls.HasFlag(Wall.LEFT))
                {
                    if (!closedList.Contains(node))
                    {
                        returnList.Add(node);
                    }
                }
            }
            if (node.position == (currNode.position + new Vector2Int(0, 1)))//UP Neighbour
            {
                if (!grid[currNode.position.x, currNode.position.y].walls.HasFlag(Wall.UP))
                {
                    if (!closedList.Contains(node))
                    {
                        returnList.Add(node);
                    }
                }
            }

            if (node.position == (currNode.position - new Vector2Int(0, 1)))//DOWN Neighbour
            {
                if (!grid[currNode.position.x, currNode.position.y].walls.HasFlag(Wall.DOWN))
                {
                    if (!closedList.Contains(node))
                    {
                        returnList.Add(node);
                    }
                }
            }
        }
        foreach (Node node in returnList)//Calculate GTentative (Gscore + Distance currNode)
        {
            node.GScore = nodeDistance(node.position, startPos.position);

            float GTentative = 0;

            if (currNode.GScore != 0)
            {
                GTentative = (currNode.GScore + nodeDistance(currNode.position, node.position));
            }
            if (GTentative < node.GScore)
            {
                node.parent = currNode;
                node.GScore = GTentative;
                openList.Add(node);
            }
        }
        //Debug.Log("CurrNode = Position: " + currNode.position + " Score: " + currNode.GScore + "," + currNode.HScore + "," + currNode.FScore + " Neigbours: " + returnList.Count);

        return openList;
    }
    

    private int nodeDistance(Vector2 startPos, Vector2 endPos)
    {
        return Mathf.RoundToInt(Mathf.Sqrt((Mathf.Pow((startPos.x - endPos.x), 2) + Mathf.Pow((startPos.y - endPos.y), 2))) * 10);
    }
    

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
