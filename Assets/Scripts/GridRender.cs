using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRender : MonoBehaviour
{

    public GameObject mapParent;
    public GameObject tilePrefab;
    public Grid Grid;
    
    public List<NoiseToSpriteGroup> sprites = new List<NoiseToSpriteGroup>();
    public Dictionary<string, RenderPool> layerRenderers = new Dictionary<string, RenderPool>(); //Match a layer name to a list of renderers

    private int lastWidth, lastHeight;

    [System.Serializable]
    public struct NoiseToSprite
    {
        public float noiseMin, noiseMax;
        public Color color;
        public Sprite sprite;
    }

    [System.Serializable]
    public class NoiseToSpriteGroup
    {
        public string noiseLayer;
        public NoiseToSprite[] noiseToSprites;
        public bool useSprite;
        public int spriteLayer;
    }

    public void CreateRenderers()
    {
        if (Grid.Tiles == null)
            throw new System.Exception("Error: Attempted to create renderers with no tile data in grid");

        for (int height = 0;  height < Grid.height; height++)
        {
            for (int width = 0; width < Grid.width; width++)
            {
                TileData tile = Grid.Tiles[height, width];
                foreach (string key in tile.noise.Keys)
                { //Check our layerRenderers to see if we have a pool for that layer
                    RenderPool pool;
                    NoiseToSpriteGroup spriteGroup = null;
                    if (layerRenderers.ContainsKey(key))
                    {
                        pool = layerRenderers[key];
                    }
                    else
                    {
                        pool = new RenderPool();
                        layerRenderers.Add(key, pool);
                    }
                    pool.Reset();

                    //Next lets find the weights to sprite group
                    for (int i = 0; i < sprites.Count; i++)
                    {
                        if (sprites[i].noiseLayer == key)
                        {
                            spriteGroup = sprites[i];
                            break; //Early exit
                        }
                    }
                    if (spriteGroup == null)
                        continue; //If we didn't find rules to render this layer we should skip to the next layer

                    //Otherwise we now have all the info to create the tile for this layer
                    foreach (NoiseToSprite weights in spriteGroup.noiseToSprites)
                    {
                        if (tile.noise[key] >= weights.noiseMin && tile.noise[key] <= weights.noiseMax)
                        { //We can create the tile now
                            GameObject renderer = pool.GetRenderer(tilePrefab, mapParent.transform);
                            renderer.transform.position = new Vector2(height, width);
                            SpriteRenderer sprite = renderer.GetComponent<SpriteRenderer>();
                            if (spriteGroup.useSprite)
                                sprite.sprite = weights.sprite; //Change the square sprite to the one specified
                            sprite.color = weights.color;
                            sprite.sortingOrder = spriteGroup.spriteLayer;
                        }
                    }
                }
            }
        }
        foreach (RenderPool pool in layerRenderers.Values)
        {
            pool.Trim(); //Have each pool deactivate any unused renderers.
        }
    }
}
