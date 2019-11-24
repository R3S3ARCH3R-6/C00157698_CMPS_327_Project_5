using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MapGen;

/// <summary>
/// References:
/// https://www.youtube.com/watch?v=mZfyt03LDH4&t=8s
/// https://www.youtube.com/watch?v=mZfyt03LDH4&t=8s (coding the actual A* alg)
/// </summary>

public class Node
{
    public Node cameFrom = null; //parent node
    public double priority = 0; // F-cost value
    public double costSoFar = 0; // G-cost Value
    public double HCost = 0;    //H-cost (I added this var)
    public Tile tile;

    public Node(Tile _tile, double _priority, Node _cameFrom, double _costSoFar, double heuristic)
    {
        cameFrom = _cameFrom;
        priority = _priority; 
        costSoFar = _costSoFar;
        tile = _tile;

        HCost = heuristic;  //added HCost
    }
}

public class PathFinder
{
    List<Tile> open = new List<Tile>();
    List<Tile> closed = new List<Tile>();

    List<Node> TODOList = new List<Node>();
    List<Node> DoneList = new List<Node>();
    Tile goalTile = null;

    public PathFinder()
    {
    }

    // TODO: Find the path based on A-Star Algorithm
    public Queue<Tile> FindPathAStar(Tile start, Tile goal)
    {
        //double G = HeuristicsDistance(start, goal); //G of endNode
        //double H = HeuristicsDistance(start, goal); //H of endNode
        //double F = G + H;

        Node startNode = new Node(start, 0, null, 0, 0);   //F, G, and H aren't on the start node
        Tile test = startNode.tile;

        List<Tile> paths = start.Adjacents;
        //Node endNode = new Node(goal, F, null, G, H);  //may be wrong design
        Debug.Log(paths.ToString());

        TODOList.Add(startNode);
            //gets the node component of the "start" Tile object

        while(TODOList.Count > 0)
        {
            Node current = TODOList[0];

            for(int i = 1; i<TODOList.Count; i++)
            {
                if(TODOList[i].priority < current.priority || 
                    TODOList[i].priority == current.priority && 
                    HeuristicsDistance(current.tile, goal) < current.HCost)
                {
                    current = TODOList[i];

                }
            }

            TODOList.Remove(current);
            DoneList.Add(current);

            if (current.tile == goal)
            {
                return RetracePath(startNode);
            }

            foreach(Node neighbor in NeighborNodes(startNode))
            {
                //neighbor.tile.Adjacents();
            }

        }


        //return path;
        return new Queue<Tile>(); // Returns an empty Path
    }

    // TODO: Find the path based on A-Star Algorithm
    // In this case avoid a path passing near an enemy tile
    public Queue<Tile> FindPathAStarEvadeEnemy(Tile start, Tile goal)
    {



       return new Queue<Tile>(); // Returns an empty Path
    }


    List<Node> NeighborNodes(Node node)
    {
        List<Node> neighborNodes = new List<Node>();
        //Tile found = new Tile();
        List<MapTile> map = neighborNodes[0].tile.mapTile.Adjacents;
        List<Tile> tempTile = neighborNodes[0].tile.Adjacents;

        List<Tile> tileList = node.tile.Adjacents;

        for(int j = 0; j < tileList.Count; j++)
        {
            //double gCost = 

            //Node newNode = new Node(tileList[j], Facebook, )
            //neighborNodes.Add();
        }

        //the code MUST be optimized
        //we are searching in a 3x3 block around a tile for smaller F(x) and H(x) values
        /*for (int x = -1; x <=1; x++)
        {
            for(int y = -1; y <=1; y++)
            {
                //if x and y are 0, we're in the center of the tile/the current tile and not a neighbor
                if (x == 0 && y == 0)
                {
                    continue;   //this skips the iteration
                }

                int checkX = node.tile.indexX + x;
                int checkY = node.tile.indexY + y;

                if ((checkX == 0 && checkX < node.tile.mapTile.X) && (checkY == 0 && checkY < node.tile.mapTile.Y))
                {
                    //Tile tileTemp = node.tile.Adjacents.;
                }
            }
        }*/

        return neighborNodes;
    }

    int GetDist(Tile startTile, Tile endTile)
    {
        int xDist = Mathf.Abs(startTile.mapTile.X - endTile.mapTile.X); //absolute val. of x-distance
        int yDist = Mathf.Abs(startTile.mapTile.Y - endTile.mapTile.Y); //absolute val. of y-distance

        //this calculates the distance (x and y) b/w the end tile and the start tile
        /*if(xDist > yDist)
        {
            return 14 * yDist + 10 * (xDist - yDist);
        }
        else
        {
            return 14 * xDist + 10 * (yDist - xDist);
        }*/

        if (xDist > yDist)
        {
            return 14 * yDist + 10 * (xDist - yDist);
        }
        return 14 * xDist + 10 * (yDist - xDist);
    }

    // Manhattan Distance with horizontal/vertical cost of 10
    double HeuristicsDistance(Tile currentTile, Tile goalTile)
    {
        int xdist = Math.Abs(goalTile.indexX - currentTile.indexX);
        int ydist = Math.Abs(goalTile.indexY - currentTile.indexY);
        // Assuming cost to move horizontally and vertically is 10
        //return manhattan distance
        return (xdist * 10 + ydist * 10);
    }

    // Retrace path from a given Node back to the start Node
    Queue<Tile> RetracePath(Node node)
    {
        List<Tile> tileList = new List<Tile>();
        Node nodeIterator = node;
        while (nodeIterator.cameFrom != null)
        {
            tileList.Insert(0, nodeIterator.tile);
            nodeIterator = nodeIterator.cameFrom;
        }
        return new Queue<Tile>(tileList);
    }

    // Generate a Random Path. Used for enemies
    public Queue<Tile> RandomPath(Tile start, int stepNumber)
    {
        List<Tile> tileList = new List<Tile>();
        Tile currentTile = start;
        for (int i = 0; i < stepNumber; i++)
        {
            Tile nextTile;
            //find random adjacent tile different from last one if there's more than one choice
            if (currentTile.Adjacents.Count < 0)
            {
                break;
            }
            else if (currentTile.Adjacents.Count == 1)
            {
                nextTile = currentTile.Adjacents[0];
            }
            else
            {
                nextTile = null;
                List<Tile> adjacentList = new List<Tile>(currentTile.Adjacents);
                ShuffleTiles<Tile>(adjacentList);
                if (tileList.Count <= 0) nextTile = adjacentList[0];
                else
                {
                    foreach (Tile tile in adjacentList)
                    {
                        if (tile != tileList[tileList.Count - 1])
                        {
                            nextTile = tile;
                            break;
                        }
                    }
                }
            }
            tileList.Add(currentTile);
            currentTile = nextTile;
        }
        return new Queue<Tile>(tileList);
    }

    private void ShuffleTiles<T>(List<T> list)
    {
        // Knuth shuffle algorithm :: 
        // courtesy of Wikipedia :) -> https://forum.unity.com/threads/randomize-array-in-c.86871/
        for (int t = 0; t < list.Count; t++)
        {
            T tmp = list[t];
            int r = UnityEngine.Random.Range(t, list.Count);
            list[t] = list[r];
            list[r] = tmp;
        }
    }
}
