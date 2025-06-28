using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private MapManager mapManager;

    public List<Vector2Int> openSetList = new();

    public PathFinder(MapManager map)
    {
        this.mapManager = map;
    }

    private static readonly Vector2Int[] DIR =
    {
        Vector2Int.up,    
        Vector2Int.down,
        Vector2Int.right, 
        Vector2Int.left   
    };

    /// <summary>
    /// スタートからゴールまでのパスを取得（経路がなければ null）
    /// </summary>
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        // 開いているノード
        var openSet = new PriorityQueue<Vector2Int>();
        openSet.Enqueue(start, 0);

        // どのノードから来たか
        Dictionary<Vector2Int, Vector2Int> cameFrom = new();

        // スタート地点からのコスト
        Dictionary<Vector2Int, int> costSoFar = new();
        costSoFar[start] = 0;

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();
            
            if (current == goal)
            {
                Debug.Log("道！あったでええ！！！");
                return ReconstructPath(cameFrom, start, goal);
            }

            foreach (Vector2Int neighbor in GetNeighbours(current))
            {
                int newCost = costSoFar[current] + 1; // 全ての移動コストは1とする
                

                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    int priority = newCost + CalcCost(neighbor, goal);
                    openSet.Enqueue(neighbor, priority);
                    cameFrom[neighbor] = current;
                }
            }
        }

        Debug.Log("道見つかれへんかったんやけど!");
        return null; // 経路が見つからなかった場合
    }
    

    /// <summary>
    /// 経路復元
    /// </summary>
    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> path = new();
        Vector2Int current = goal;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }

    /// <summary>
    /// 隣接マスの取得
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private List<Vector2Int> GetNeighbours(Vector2Int pos)
    {
        var list = new List<Vector2Int>(4);
        foreach (Vector2Int d in DIR)
        {
            int nx = pos.x + d.x;
            int ny = pos.y + d.y;
            var data = mapManager.GetBlockData(nx, ny);
            if (data != null && data.isWalkable) list.Add(new Vector2Int(nx, ny));
        }
        return list;
    }

    /// <summary>
    /// ゴールまでの距離
    /// </summary>
    private int CalcCost(Vector2Int neighbor, Vector2Int goal)
    {

        int cost = Mathf.Abs(neighbor.x - goal.x) + Mathf.Abs(neighbor.y - goal.y);
        return cost;
    }
}