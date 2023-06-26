using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPool
{
    public List<GameObject> pool = new List<GameObject>();
    int used, total; //How many of the renderers are being used and how many do we have?
    
    public void Reset()
    {
        used = 0;
    }

    public GameObject GetRenderer(GameObject prefabToUse, Transform parent)
    {
        GameObject found;
        if (used >= pool.Count)
        { //We need to add a new renderer to the pool
            found = GameObject.Instantiate(prefabToUse, parent);
        }
        else
        {
            found = pool[used];
        }
        used++;
        total = pool.Count;
        return found;
    }

    public void Trim()
    {
        if (used < total)
        {
            for (int i = used; i < pool.Count; i++)
            {
                GameObject go = pool[i];
                go.SetActive(false);
            }
        }
    }

}
