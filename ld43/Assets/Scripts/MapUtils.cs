using UnityEngine;

public static class MapUtils
{
    public static int GetManhattanDistance(Vector2Int p1, Vector2Int p2)
    {
        return Mathf.Abs(p2.x - p1.x) + Mathf.Abs(p2.y - p1.y);
    }

    public static int GetChebyshevDistance(Vector2Int p1, Vector2Int p2)
    {
        return Mathf.Max(Mathf.Abs(p2.x - p1.x), Mathf.Abs(p2.y - p1.y));
    }
}