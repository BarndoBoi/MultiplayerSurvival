using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int height, width, seed;
    public TileData[,] Tiles;
    public List<NoiseLayerGroup> GridWeights = new List<NoiseLayerGroup>();

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

    [System.Serializable]
    public struct NoiseLayerGroup
    {
        public string GroupName;
        public List<NoiseLayerWithWeight> weights;
    }

    public void CreateMap()
    {
        Tiles = new TileData[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Tiles[i, j] = new TileData();
                Tiles[i, j].x = i;
                Tiles[i, j].y = j;
                Tiles[i, j].GridContainer = this;
                
                foreach (NoiseLayerGroup g in GridWeights)
                {
                    Tiles[i, j].noise[g.GroupName] = SumWeights(g.weights, i, j); //Get the weights at this tile for each layer
                }
            }
        }
        //Temporary for testing purposes
        GetComponent<GridRender>().CreateRenderers();
    }

    public float SumWeights(List<NoiseLayerWithWeight> layers, int i, int j)
    {
        float noise = 0;
        float maxWeight = 0;
        foreach (NoiseLayerWithWeight weight in layers)
        {
            
            if (weight.layer.subtractive)
            {
                noise -= Mathf.PerlinNoise((i + (weight.layer.noiseOffsetY + seed)) * weight.layer.noiseScale,
                (j + (weight.layer.noiseOffsetY + seed)) * weight.layer.noiseScale) * weight.weight;

                maxWeight -= weight.weight;
            }
            else
            {
                noise += Mathf.PerlinNoise((i + (weight.layer.noiseOffsetY + seed)) * weight.layer.noiseScale,
                    (j + (weight.layer.noiseOffsetY + seed)) * weight.layer.noiseScale) * weight.weight;
                maxWeight += weight.weight;
            }

        }

        return Mathf.InverseLerp(0, maxWeight, noise);
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
