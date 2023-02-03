using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

public static class Pathfinding
{
    private static World _world;

    public static void Init(World world)
    {
        _world = world;
    }

    public static Task<List<Tile>> GetPathAsync(Vector2Int start, Vector2Int end, int maxDist)
    {
        var task = new Task<List<Tile>>(() => {
            return GetPath(start, end, maxDist);
        });
        task.Start();

        return task;
    }

    public static List<Tile> GetPath(Vector2Int start, Vector2Int end, int maxDist)
    {
        try
        {
            if (_world == null)
            {
                Debug.LogError("Pathfinding not initialized");
                return null;
            }

            if (!_world.IsInBounds(start) || !_world.IsInBounds(end))
            {
                Debug.LogError("Start or end point out of bounds");
                return null;
            }

            List<Tile> path = new List<Tile>();
            HashSet<Vector2Int> closed = new HashSet<Vector2Int>();
            List<Vector2Int> open = new List<Vector2Int>();
            List<Vector2Int> newOpen = new List<Vector2Int>();
            Dictionary<Vector2Int, float> costs = new Dictionary<Vector2Int, float>();

            open.Add(start);
            costs.Add(start, 0);


            var endReached = false;

            while(open.Count > 0)
            {
                foreach(var curTilePos in open)
                {
                    var curTile = _world.GetTile(curTilePos);

                    var neighborIndex = -1;

                    foreach (var neighborTile in _world.GetNeighbours(curTile))
                    {
                        neighborIndex++;

                        if (neighborTile.Position == end)
                        {
                            path.Add(neighborTile);
                            endReached = true;
                            Debug.Log("End reached: " + neighborTile.Position);
                            break;
                        }

                        if (neighborTile.Def == null || !neighborTile.Def.IsWalkable || neighborTile.entities.FirstOrDefault(e => e.Blocking) != null) continue;

                        var cost = costs[curTilePos] + (neighborIndex == 0 || neighborIndex == 2 || neighborIndex == 5 || neighborIndex == 7 ? 1.4f : 1f);
                        
                        if (costs.ContainsKey(neighborTile.Position))
                        {
                            if (costs[neighborTile.Position] > cost)
                            {
                                costs[neighborTile.Position] = cost;
                            }
                        }
                        else
                        {
                            newOpen.Add(neighborTile.Position);
                            costs.Add(neighborTile.Position, cost);
                        }

                        //Thread.Sleep(1);
                        //Debug.DrawLine(new Vector2(curTilePos.x, curTilePos.y), new Vector2(neighborTile.Position.x, neighborTile.Position.y), Color.cyan, 5);
                    }

                    if (endReached) break;
                }

                if (endReached) break;

                open.ForEach(p => closed.Add(p));
                open.Clear();
                newOpen.ForEach(p => open.Add(p));

                newOpen.Clear();

            }

            // Reconstruct path
            var curPos = end;
            while (curPos != start)
            {
                var curTile = _world.GetTile(curPos);
                var neighbours = _world.GetNeighbours(curTile);

                var minCost = float.MaxValue;
                var minCostTile = curTile;

                foreach (var neighbour in neighbours)
                {
                    if (!costs.ContainsKey(neighbour.Position)) continue;

                    if (costs[neighbour.Position] < minCost)
                    {
                        minCost = costs[neighbour.Position];
                        minCostTile = neighbour;
                    }
                }

                //Debug.DrawLine(new Vector2(curPos.x, curPos.y), new Vector2(minCostTile.Position.x, minCostTile.Position.y), Color.yellow, 10);
                path.Add(minCostTile);
                curPos = minCostTile.Position;
            }
            
            path.Reverse();
            return path;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message + e.StackTrace);
            return null;
        }
    }

    static int CostEstimate(Vector2Int start, Vector2Int end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }
}
