using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// References:
/// https://www.youtube.com/watch?v=mZfyt03LDH4&t=8s
///  https://www.youtube.com/watch?v=mZfyt03LDH4&t=8s (coding the actual A* alg)
/// </summary>

//this is where the real A* algorithm is applied
public class Pathfinding : MonoBehaviour
{
    public Transform player;    //player object
    public Transform target;    //the target Tile/object

    Grid grid;  //Grid object containing the grid layout of the game

    public List<Tile_Online> starPath = new List<Tile_Online>();

    //renders items and instantiates objects at the beginning of a scene
    private void Awake()
    {
        //starPath
        grid = GetComponent<Grid>();
            //gets the Grid script attached to this object (the A* object)
    }

    //moves the object along the A* Path by calling the other methods
    private void Update()
    {
        FindPath(player.position, target.position);
    }

    //finds the shortest path along a grid 
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Tile_Online startPoint = grid.TilefromWorldPoint(startPos);    //grid point/tile where the player starts moving from
        Tile_Online targetPoint = grid.TilefromWorldPoint(targetPos);  //goal point

        List<Tile_Online> unCalcTiles = new List<Tile_Online>();         
            //list of tiles with F = G + H values that have not been calculated
        HashSet<Tile_Online> calcTiles = new HashSet<Tile_Online>();
            //list of tiles that have been traversed or have calculated F = G + H values

        unCalcTiles.Add(startPoint);    //first tile to be calculated

        //Debug.Log("Open Nodes " + unCalcTiles);

        //while the list of uncalculated tiles is not empty:...
        while(unCalcTiles.Count > 0)
        {
            //find the tile in "unCalcTiles" with the lowest F-cost 
            Tile_Online current = unCalcTiles[0];

            //traverses through the list "unCalcTiles"
                //need to change this so the alg. is efficient
            for(int i = 1; i<unCalcTiles.Count; i++)
            {
                /*if the current tile's F(x) val is less than the next 
                 * "unCalcTiles'" value; or the current tile's and "unCalcTiles[i]'s" 
                 * F(X) equal each other and "unCalcTiles[i]'s" H(x) is less than the
                 * current tile's H(x), make current tile "unCalcTile"
                 */
                if (unCalcTiles[i].FCost() < current.FCost() || 
                    unCalcTiles[i].FCost() == current.FCost() && 
                    unCalcTiles[i].H < current.H)
                {
                    Debug.Log("appear");
                    current = unCalcTiles[i];
                }
            }

            unCalcTiles.Remove(current);    //remove the selected tile/tile we chose to move across
            calcTiles.Add(current);     //add the tile we are using to the calculated Tile hash set

            //if the current tile is the target, exit the loop
            if(current == targetPoint)
            {
                StepBack(startPoint, targetPoint);
                return;
            }

            //for each Tile object in "grid" (a tile list), check if the tile "neighbor" is walkable or not
            foreach (Tile_Online neighbor in grid.GetNeighbors(current))
            {
                /* if the tile/current tile in the loop (not the current tile for the player) is not walkable
                 * or already calculated, skip a run and move to the next part of the loop*/
                if(!neighbor.walkable || calcTiles.Contains(neighbor))
                {
                    continue;
                }

                int moveCostNeigh = current.G + GetDist(current, neighbor);  
                    //the cost of moving from the current tile to the neighboring tile
                    //this is the new/current G-cost from the neighbor/when when we move to the neighbor node

                /* if the calculated move cost is less than the current neighbor's g-cost or the neighbor has 
                 * not been traversed/has not had its values calculated yet, ...
                 */
                if(moveCostNeigh < neighbor.G || !unCalcTiles.Contains(neighbor))
                {
                    neighbor.G = moveCostNeigh; //new G-cost of the neighbor tile
                    neighbor.H = GetDist(neighbor, targetPoint);    //used to get the new H-cost of the neighbor tile

                    neighbor.parent = current;

                    Debug.Log("current node: " + current);
                    Debug.Log("parent of neighbor: " + neighbor.parent);

                        //the parent of the neighboring tile is the current node we're on
                        //this is b/c we have not traversed/moved the player yet

                    /* if we have not calculated the distances of the neighbor tile yet, calculate it and mark it has 
                     * having been calculated*/
                    if (!unCalcTiles.Contains(neighbor))
                    {
                        unCalcTiles.Add(neighbor);
                    }
                }
            }
        }
    }

    //"StepBack" retraces the path the player follows from the final node to the start node
    void StepBack(Tile_Online startTile, Tile_Online endTile)
    {
        List <Tile_Online> path = new List<Tile_Online>();
        Tile_Online current = endTile;

        /* while the current tile is not the starting tile, add the current tile to the list 
         * and set "current" value to its parent, tracing the path backwards and putting it 
         * into the list
         */
        while(current != startTile)
        {
            path.Add(current);
            Debug.Log(path);
            if (path.Contains(current))
            {
                Debug.Log("insert successful");
            }
            current = current.parent; 
        }

        path.Reverse(); //reverses the list to get the correct order of the path to traverse/traversed

        grid.AStarPath = path;   //creates a gizmo to show where the path exists
        starPath = path;
    }


    //method gets the distance b/w the start and end nodes
    int GetDist(Tile_Online startTile, Tile_Online endTile)
    {
        int xDist = Mathf.Abs(startTile.gridx - endTile.gridx); //absolute val. of x-distance
        int yDist = Mathf.Abs(startTile.gridy - endTile.gridy); //absolute val. of y-distance

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
}
