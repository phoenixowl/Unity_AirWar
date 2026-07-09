using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 5;
    public float moveSpeed = 5f; // 移动速度
    public float fireInterval = 1.0f;//发射速度
    float nextFireTime;
    [SerializeField] GameObject bulletPrefab;   // ← Inspector 把 BulletPlayer prefab 拖这里
    [SerializeField] Transform firePoint;
    [SerializeField] private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        var cfg = ConfigManager.Instance.GameConfig;
        health = cfg.playerHP;
        moveSpeed = cfg.playerMoveSpeed;
        fireInterval = cfg.fireInterval;
    }

    void Update()
    {
        // 获取输入（-1 ~ 1）
        float input = Input.GetAxis("Horizontal");

        // 水平移动
        transform.Translate(Vector3.right * input * moveSpeed * Time.deltaTime);

        // 屏幕边界限制
        ClampPositionToScreen();
        //攻击
        if (Time.time >= nextFireTime)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                nextFireTime = Time.time + fireInterval;

                GameObject bullet = ObjectPool.Instance.GetBullet();
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = Quaternion.identity;

                bullet.GetComponent<BulletController>().Init();
            }
        }
    }

    void ClampPositionToScreen()
    {
        if (mainCamera == null || spriteRenderer == null)
            return;

        // 将世界坐标转换为屏幕坐标
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));

        // 考虑精灵宽度的一半
        float halfWidth = spriteRenderer.bounds.extents.x;

        // 计算可移动范围
        float minX = leftEdge.x + halfWidth;
        float maxX = rightEdge.x - halfWidth;

        // 限制位置
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        transform.position = clampedPos;
    }
}
