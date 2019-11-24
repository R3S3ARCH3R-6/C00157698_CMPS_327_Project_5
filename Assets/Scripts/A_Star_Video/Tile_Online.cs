using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// References:
/// https://www.youtube.com/watch?v=nhiFx28e7JY (starting the code/creating the grid)
/// https://www.youtube.com/watch?v=mZfyt03LDH4&t=8s (A* alg.)
/// </summary>

//this script contains the values used in a tile for A* algorithms, the "F", "G", and "H" functions
public class Tile_Online 
{
    public bool walkable;   //tells the alg. if a tile/node can be walked on or not
    public Vector3 worldPos;  //tells the position of node/tile in the world space

    //the variables below are from the 2nd site
    public int G = 0;   //G(x), or G-cost value, the value a tile is away from the start point
    public int H = 0;   //H(x), or H-cost value, the value a tile is away from the target point

    public int gridx;   //stores the value of object/tile's x-coord. on the grid
    public int gridy;   //stores the value of object/tile's y-coord. on the grid

    public Tile_Online parent; //public parent Tile of the current tile
        //"parent" contains the previos Tile the player was on

    //constructor for the tile object/script
    //sets the values of the public variables above
    //3rd and 4th var and space are from the 2nd site 
    public Tile_Online(bool walk, Vector3 pos, int grid_xVal, int grid_yVal, Tile_Online _parent)
    {
        walkable = walk;
        worldPos = pos;

        //from 2nd site
        gridx = grid_xVal;
        gridy = grid_yVal;
        parent = _parent;
    }

    //gets the value of the F-cost, or F(x)
        //this comes from the 2nd link
    public int FCost()
    {
        return G + H;
    }
}
