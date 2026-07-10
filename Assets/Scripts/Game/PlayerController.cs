using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    public int health = 5;
    public float moveSpeed = 5f; // 移动速度
    public float fireInterval = 1.0f;//发射速度

    public int damage = 1;
    public float bulletSpeed = 10.0f;

    public float nextFireTime;
    public float invicibleTime = 1.0f;
    public float invicibleTimer = 0f;

    [SerializeField] BulletShooter bulletShooter;
    [SerializeField] Camera mainCamera;
    HitFlashController hitFlashController;
    SpriteRenderer spriteRenderer;

    public bool useMouseControl = true;

    [Tooltip("当距离鼠标X小于此值时，直接贴合到鼠标位置")]
    public float mouseSnapThreshold = 0.05f;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        hitFlashController = GetComponent<HitFlashController>();
    }
    void Start()
    {

        var cfg = ConfigManager.Instance.StatsConfigSO;
        health = cfg.playerHP;
        moveSpeed = cfg.playerMoveSpeed;
        damage = cfg.playerBulletDamage;
        bulletSpeed = cfg.playerBulletSpeed;
        fireInterval = cfg.playerFireInterval;
        invicibleTime = cfg.playerInvicibleTime;
    }

    void Update()
    {
        if (GameManager.Instance.isGamePausing) return;

        if (useMouseControl)
        {
            MoveWithMouse();
        }
        else
        {
            // 原有键盘控制
            float input = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * input * moveSpeed * Time.deltaTime);
        }

        ClampPositionToScreen();
        //攻击
        TryShoot();

        //无敌帧
        if (invicibleTimer > 0f)
        {
            invicibleTimer -= Time.deltaTime;
        }
    }

    void MoveWithMouse()
    {
        if (mainCamera == null || Time.timeScale == 0f)
            return;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        float targetX = mouseWorldPos.x;
        float currentX = transform.position.x;
        float deltaX = targetX - currentX;

        // ✅ 只用速度，不用瞬移
        float direction = Mathf.Sign(deltaX);
        float step = moveSpeed * Time.deltaTime;

        // ✅ 限制最大步长，防止过冲
        step = Mathf.Min(step, Mathf.Abs(deltaX));

        transform.Translate(Vector3.right * direction * step);
    }


    // 改进：更精确的屏幕边界限制
    void ClampPositionToScreen()
    {
        if (mainCamera == null || spriteRenderer == null)
            return;

        // 获取飞机的世界坐标
        Vector3 worldPos = transform.position;

        // 将世界坐标转换为视口坐标
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(worldPos);

        // 考虑精灵宽度的一半（在视口坐标中）
        float halfWidthInViewport = spriteRenderer.bounds.extents.x / (mainCamera.orthographicSize * 2 * mainCamera.aspect);

        // 限制X轴在屏幕内
        viewportPos.x = Mathf.Clamp(viewportPos.x, halfWidthInViewport, 1f - halfWidthInViewport);

        // 转回世界坐标
        Vector3 clampedWorldPos = mainCamera.ViewportToWorldPoint(viewportPos);
        clampedWorldPos.y = worldPos.y; // 保持Y轴不变
        clampedWorldPos.z = worldPos.z; // 保持Z轴不变

        transform.position = clampedWorldPos;
    }

    void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                nextFireTime = Time.time + fireInterval;

                bulletShooter.Shoot(true, damage, bulletSpeed);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (invicibleTimer > 0) return;

        health -= damage;
        EventBus.Emit(new PlayerHitEvent(health));

        invicibleTimer = invicibleTime;

        hitFlashController.flashDuration = invicibleTime;
        hitFlashController.Flash();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        print("u died");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var ec = other.GetComponent<EnemyController>();
            if (ec != null)
            {
                TakeDamage(ec.touchDamage);
            }
            else
            {
                TakeDamage(0);
            }
        }
    }
}
