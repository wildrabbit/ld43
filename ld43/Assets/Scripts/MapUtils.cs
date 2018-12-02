using UnityEngine;

public static class MapUtils
{
    public static Vector2Int[] ManhattanNeighbours = new Vector2Int[]
    {
        new Vector2Int(-1,0),
        new Vector2Int(1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
    };

    public static Vector2Int[] ChebyshevNeighbours = new Vector2Int[]
    {
        new Vector2Int(-1,-1),
        new Vector2Int(-1,0),
        new Vector2Int(-1,1),
        new Vector2Int(0,-1),
        new Vector2Int(0,1),
        new Vector2Int(1,-1),
        new Vector2Int(1,0),
        new Vector2Int(1,1)
    };

    public static Vector2Int[] GetNeighbourDeltas(DistanceStrategy strategy, bool includeNeutral = false)
    {
        Vector2Int[] neighbours = (strategy == DistanceStrategy.Manhattan ? ManhattanNeighbours : ChebyshevNeighbours);
        if (!includeNeutral)
        {
            return neighbours;
        }
        else
        {
            Vector2Int[] extended = new Vector2Int[1 + neighbours.Length];
            extended[0] = Vector2Int.zero;
            neighbours.CopyTo(extended, 1);
            return extended;
        }
    }

    public static int GetManhattanDistance(Vector2Int p1, Vector2Int p2)
    {
        return Mathf.Abs(p2.x - p1.x) + Mathf.Abs(p2.y - p1.y);
    }

    public static int GetChebyshevDistance(Vector2Int p1, Vector2Int p2)
    {
        return Mathf.Max(Mathf.Abs(p2.x - p1.x), Mathf.Abs(p2.y - p1.y));
    }
}