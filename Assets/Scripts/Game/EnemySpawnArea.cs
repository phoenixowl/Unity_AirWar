using UnityEngine;

public class EnemySpawnArea : MonoBehaviour
{
    private BoxCollider2D box;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }

    /// 在区域内随机返回一个世界坐标
    public Vector3 GetRandomPoint()
    {
        Bounds bounds = box.bounds;

        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0f);
    }
}