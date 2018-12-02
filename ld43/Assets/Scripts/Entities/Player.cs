using UnityEngine;

public class Player : Entity
{
    Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
        Camera.SetupCurrent(_camera);
    }

    public override void AddTime(float timeUnits, ref PlayContext playContext)
    {
        // Handle regen.
    }
}
