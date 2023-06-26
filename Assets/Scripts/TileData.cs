using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileData
{
    public int x, y; //Location on the Grid
    public Grid GridContainer; //The grid this tile is in

    //Eventually move this out of the tile and into the unit, since move cost will change depending on unit type
    private int moveCost;
    public int MoveCost { get => moveCost; }
    public Dictionary<string, float> noise = new Dictionary<string, float>(); //Name of layer and value at tile

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetMoveCost(int moveCost)
    {
        this.moveCost = moveCost;
    }
}