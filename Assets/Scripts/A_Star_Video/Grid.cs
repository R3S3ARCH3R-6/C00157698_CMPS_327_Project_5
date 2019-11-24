using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// References:
/// https://www.youtube.com/watch?v=nhiFx28e7JY (starting the code/creating the grid)
/// https://www.youtube.com/watch?v=mZfyt03LDH4&t=8s (coding the actual A* alg)
/// </summary>

//this script creates the grid an A* alg. will look at and follow
//the A* game object contains the grid script
public class Grid : MonoBehaviour
{
    //public Transform player;    //player object
    //public Transform target;    //the target Tile/object

    public LayerMask noWalkMask;    //public layer mask for the grid
    public Vector2 gridSize;        //contains a 2x2 grid size for coordinates
    public float tileRadius;        //gets the radius of a tile on the grid
    public List<Tile_Online> AStarPath;    //the path taken by the A* object

    Tile_Online[,] grid;       //contains a 2x2 array of "Tile.cs" objects/nodes

    float tileDiameter; //the diameter of a tile
    int gridX;  //contains the x-val of a grid position/the x-val length of a grid
    int gridY;  //contains the y-val of a grid position/the y-val length of a grid
        //"Y" is more like "Z" in 3D space; we're just looking top-down 

    
    // Start is called before the first frame update
    void Start()
    {
        AStarPath = GetComponent<Pathfinding>().starPath;

        tileDiameter = tileRadius * 2;  //b/c diameter is x2 radius
        gridX = Mathf.RoundToInt(gridSize.x/tileDiameter);  //returns the size of the x-axis of the grid
        gridY = Mathf.RoundToInt(gridSize.y/tileDiameter);  //returns the size of the y-axis (z-axis actually) of the grid

        CreateGrid();   //creates the grid in Unity 3D
    }


    //this method enables us to draw gizmos on the object containing this script
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));
            //draws a wire/grid on the object with this script
            //this will visually display a grid on the plane in the Edit window
            //the grid's dimensions are set in the call

        if(grid != null)    //if the array "grid" is not empty:...
        {
            //Tile playerPoint = TilefromWorldPoint(player.position);
                //a tile/node object is made from the player object's position (for the player)

            foreach(Tile_Online t in grid)   //foreach "Tile" object "t" in "grid": ...
            {
                Gizmos.color = (t.walkable) ? Color.white:Color.red;
                //the gizmo cubes'/tiles' gizmos' colors are determined by if the player can move across a tile or not
                //if walkable, white; if not walkable, red

                /*if (playerPoint == t)   //if the detected tile/node is the player, change it to blue
                {
                    Gizmos.color = Color.blue;

                }*/


                //if an AStarPath exists: ...
                if(AStarPath != null)
                {
                    //if the list/our AStarPath contains the current node being called, "t", paint the tiles black
                    if (AStarPath.Contains(t))
                    {
                        Debug.Log(AStarPath[0]);
                        Gizmos.color = Color.black;
                    }
                }
                else
                {
                    //Debug.Log("Empty path");
                    Gizmos.color = Color.magenta;
                }


                Gizmos.DrawCube(t.worldPos, Vector3.one * (tileDiameter - 0.1f));
                    //the line above draws a cube relative to the tile's world position and 
            }
        }
    }

    void CreateGrid()
    {
        grid = new Tile_Online[gridX, gridY];  //"grid" is a new array of size gridX by gridY
            //this will store the nodes and test for "walkability"

        Vector3 bLCorner = transform.position - Vector3.right * gridSize.x/2 - Vector3.forward * gridSize.y/2;
            //The above code gets us the bottom-left corner of our tile/node

        //this is how we get the correct node (this is a VERY taxing alg.)
        for(int x = 0; x < gridX; x++)
        {
            for(int y = 0; y < gridY; y++)
            {
                Vector3 worldPoint = bLCorner + Vector3.right * (x * tileDiameter + tileRadius) 
                    + Vector3.forward * (y * tileDiameter + tileRadius);
                    //"worldPoint" is the space a node will occupy relative to the world
                
                bool walkable = !(Physics.CheckSphere(worldPoint, tileRadius, noWalkMask));
                    //checks to see if the area nearby is walkable

                /*if(y >= 1 && x >= 1)
                {
                    grid[x, y] = new Tile(walkable, worldPoint, x, y, grid[x-1, y-1]);
                }else if(x )*/

                //below code creates a new node/tile object with values
                grid[x, y] = new Tile_Online(walkable, worldPoint, x, y, null);
                    //"walkable" and "worldPoint" are inserted into the constructor found in "Tile"
            }
        }
    }

    //we want to calc. how far away the point is from the grid (possibly the target position) as a percentage
    public Tile_Online TilefromWorldPoint(Vector3 worldPos)
    {
        //far left: 0%
        //middle: 0.5% (or 0.5 = 50%)
        //far right: 1% (pr 1 = 100%)

        float x_percent = (worldPos.x + gridSize.x/2) / gridSize.x;
            //percentage along the x-axis
            //this will give us a half depending on our position
        float y_percent = (worldPos.z + gridSize.y / 2) / gridSize.y;
            //percentage along the y-axis (z-axis from 3D perspective)

        x_percent = Mathf.Clamp01(x_percent); //"Clamp01" "clamps" a val b/w 0 and 1
        y_percent = Mathf.Clamp01(y_percent);
        //above is used b/c if char/worldPos is outside the grid, we don't get an invalid position number

        int x = Mathf.RoundToInt((gridX-1) * x_percent);  //x-index of the grid array
        int y = Mathf.RoundToInt((gridY - 1) * y_percent);  //y-index of the grid array

        return grid[x, y];  //returns the Tile from the grid array (specifically, its x-y coordinates)
    }


    //the following method comes from the 2nd site
    //the following method is used to ...
    public List<Tile_Online> GetNeighbors(Tile_Online tile)
    {
        List<Tile_Online> neighbors = new List<Tile_Online>();    //makes a list of nodes/the neighboring tiles

        //the code MUST be optimized
        //we are searching in a 3x3 block around a tile for smaller F(x) and H(x) values
        for(int x  = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                //if x and y are 0, we're in the center of the tile/the current tile and not a neighbor
                if(x == 0 && y == 0)
                {
                    continue;   //this skips the iteration
                }

                //checkX & checkY are the node's x-y values + the value of x and y
                int checkX = tile.gridx + x;
                int checkY = tile.gridy + y;

                //if the checks are on the grid, add the tile to the list "neighbors"
                if((checkX == 0 && checkX < gridSize.x) && (checkY == 0 && checkY < gridSize.y))
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        
        return neighbors;
    }

}
