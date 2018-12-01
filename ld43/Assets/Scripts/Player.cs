using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IScheduledEntity
{
    PlayerConfig _config;
    Map _map;

    public float Speed => _config.DefaultSpeed;

    public Vector2Int Coords { get; private set; }

    public void Setup(PlayerConfig cfg, Map map)
    {
        _config = cfg;
        _map = map;
        Coords = Vector2Int.zero;
    }

    public void AddTime(float timeUnits, ref PlayContext playContext)
    {
    }

    public void SetGridPos(Vector2Int pos)
    {
        Coords = pos;
        transform.position = _map.WorldFromGrid(pos);
    }

    public void StartGame()
    {
        SetGridPos(_map.PlayerStartCoords);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
