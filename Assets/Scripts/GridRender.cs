using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRender : MonoBehaviour
{

    public GameObject mapParent;
    public GameObject tilePrefab;
    public Grid Grid;
    GameObject[,] renderedTiles;
    List<GameObject> renderers = new List<GameObject>();
    public TerrainColors[] colors;

    private int lastWidth, lastHeight;

    [System.Serializable]
    public struct TerrainColors
    {
        public float noiseMax;
        public Color color;
    }

    public void CreateRenderers()
    {
        if (Grid.Tiles == null)
            throw new System.Exception("Error: Attempted to create renderers with no tile data in grid");

        if (renderedTiles == null)
            renderedTiles = new GameObject[Grid.height, Grid.width];

        else if (lastWidth != Grid.width || lastHeight != Grid.height)
        { //The dimensions have changed so we need to resize our array

            renderedTiles = new GameObject[Grid.height, Grid.width];
            
        }

        lastWidth = Grid.width;
        lastHeight = Grid.height;
        int listIndex = 0;
        for (int i = 0; i < Grid.height; i++)
        {
            for (int j = 0; j < Grid.width; j++)
            {
                GameObject tile;
                if (renderedTiles[i, j] == null)
                {
                    if (listIndex < renderers.Count)
                    {
                        tile = renderers[listIndex];
                        tile.SetActive(true);
                        tile.transform.position = new Vector3(i, j);
                    }
                    else
                    {
                        tile = GameObject.Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity, mapParent.transform);
                        renderers.Add(tile);
                    }
                    renderedTiles[i, j] = tile;
                }
                tile = renderedTiles[i, j];
                listIndex++;
                SpriteRenderer rend = tile.GetComponent<SpriteRenderer>();
                
                foreach (TerrainColors terrains in colors)
                {
                    if (Grid.Tiles[i, j].elevationNoise >= terrains.noiseMax)
                        rend.color = terrains.color;
                }
            }
        }
        if (listIndex < renderers.Count)
        { //We have leftover tiles
            for ( int i = listIndex + 1; i < renderers.Count; i++ ) 
            {
                renderers[i].SetActive(false);
            }
        }
    }
}
