using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    public static List<TileData> GetBFS(TileData start, Grid grid, float range)
    {
        HashSet<TileData> visited = new HashSet<TileData>();
        Queue<TileData> queue = new Queue<TileData>();
        queue.Enqueue(start);
        var startVec = new Vector2(start.x, start.y);

        while (queue.Count > 0)
        {
            TileData tile = queue.Dequeue();
            if (visited.Contains(tile))
                continue;
            var currentVec = new Vector2(tile.x, tile.y);
            if (Vector2.Distance(currentVec, startVec) < range)
                visited.Add(tile);


            foreach(TileData neighbor in grid.GetNeighbors(tile))
            {
                currentVec = new Vector2(neighbor.x, neighbor.y);
                if (!visited.Contains(neighbor) && Vector2.Distance(currentVec, startVec) < range)
                    queue.Enqueue(neighbor);
            }
        }
        return visited.ToList();
    }
}
