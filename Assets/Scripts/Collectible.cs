using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum Type { Bullets, Life }
    public Type type;
}

public class BulletCollectible : Collectible
{
    void Awake()
    {
        type = Type.Bullets;
    }
}

public class LifeCollectible : Collectible
{
    void Awake()
    {
        type = Type.Life;
    }
}