using StandTogether;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int height, width, seed;
    public TileData[,] Tiles;
    public float[,] elevations;
    public List<NoiseLayerWithWeight> Weights;

    private void Awake()
    {
        //CreateMap(); Commented out for testing building maps with ui only
    }

    [System.Serializable]
    public struct NoiseLayerWithWeight
    {
        public NoiseLayer layer;
        public float weight;
    }

    public void CreateMap()
    {
        Tiles = new TileData[height, width];
        elevations = new float[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Tiles[i, j] = new TileData();
                Tiles[i, j].x = i;
                Tiles[i, j].y = j;
                Tiles[i, j].GridContainer = this;
                float elevation = 0;
                float maxWeight = 0;
                foreach (NoiseLayerWithWeight weight in Weights)
                {
                    if (weight.layer.subtractive)
                    {
                        elevation -= Mathf.PerlinNoise((i + (weight.layer.noiseOffsetY + seed)) * weight.layer.noiseScale,
                        (j + (weight.layer.noiseOffsetY + seed)) * weight.layer.noiseScale) * weight.weight;

                        maxWeight -= weight.weight;
                    }
                    else
                    {
                        elevation += Mathf.PerlinNoise((i + (weight.layer.noiseOffsetY + seed)) * weight.layer.noiseScale,
                            (j + (weight.layer.noiseOffsetY + seed)) * weight.layer.noiseScale) * weight.weight;
                        maxWeight += weight.weight;
                    }
                    
                }
                Tiles[i, j].elevationNoise = Mathf.InverseLerp(0, maxWeight, elevation);
                elevations[i, j] = Tiles[i, j].elevationNoise;
            }
        }
        //Temporary for testing purposes
        GetComponent<GridRender>().CreateRenderers();
    }

    public List<TileData> GetNeighbors(TileData source)
    {
        //Return a list of tiles north, east, south, and west of the source tile
        List<TileData> foundTiles = new List<TileData>();
        if (source.x - 1 >= 0) //Only add west tile if it is a positive integer
            foundTiles.Add(Tiles[source.x - 1, source.y]);
        if (source.x + 1 <= width)
            foundTiles.Add(Tiles[source.x +1, source.y]);
        if (source.y - 1 >= 0)
            foundTiles.Add(Tiles[source.x, source.y - 1]);
        if (source.y + 1 <= height)
            foundTiles.Add(Tiles[source.x, source.y + 1]);

        return foundTiles;
    }

}
