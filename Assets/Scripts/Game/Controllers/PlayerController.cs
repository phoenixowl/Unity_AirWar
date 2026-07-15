using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    public int health = 5;
    public float moveSpeed = 5f;
    public float fireInterval = 1.0f;
    public int damage = 1;
    public float bulletSpeed = 10.0f;
    public float nextFireTime;
    public float invicibleTime = 1.0f;
    public float invicibleTimer = 0f;

    [SerializeField] BoxCollider2D movementBoundsArea;
    [SerializeField] BulletShooter bulletShooter;
    [SerializeField] Camera mainCamera;
    HitFlashController hitFlashController;
    SpriteRenderer spriteRenderer;
    Collider2D playerCollider; // 缓存玩家自身的碰撞体，用于精确计算边界

    public bool useMouseControl = true;
    public bool useAutoFire = true;
    [Tooltip("当距离鼠标X小于此值时，直接贴合到鼠标位置")]
    public float mouseSnapThreshold = 0.05f;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); // 获取玩家自身碰撞体
        hitFlashController = GetComponent<HitFlashController>();

        // 未赋值移动区域Collider的警告
        if (movementBoundsArea == null)
            Debug.LogWarning("PlayerController: 未赋值移动区域Collider");
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
        useMouseControl = ConfigManager.Instance.SettingConfigSO.useMouseControl;
        useAutoFire = ConfigManager.Instance.SettingConfigSO.useAutoFire;
    }

    void Update()
    {
        if (GameManager.Instance.isGamePausing) return;

        // 移动逻辑（二选一，根据useMouseControl切换）
        if (useMouseControl)
        {
            MoveWithMouse();
        }
        else
        {
            MoveWithKeyboard(); // 键盘上下左右移动
        }

        // 核心：用移动区域限制位置（替代原来的屏幕边界限制）
        ClampPositionToBounds();

        // 攻击
        TryShoot();

        // 无敌帧倒计时
        if (invicibleTimer > 0f)
        {
            invicibleTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 键盘控制：支持WASD/方向键上下左右移动
    /// </summary>
    void MoveWithKeyboard()
    {
        float horizontal = Input.GetAxis("Horizontal"); // 左右轴
        float vertical = Input.GetAxis("Vertical");     // 上下轴（新增）

        Vector2 moveDir = new Vector2(horizontal, vertical);
        // 归一化方向，防止斜向移动速度比单向快√2倍
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 鼠标控制：平滑跟随鼠标位置（支持XY轴）
    /// </summary>
    void MoveWithMouse()
    {
        if (mainCamera == null || Time.timeScale == 0f)
            return;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        // X轴移动（原有逻辑保留）
        float deltaX = mouseWorldPos.x - transform.position.x;
        float stepX = Mathf.Min(Mathf.Abs(deltaX), moveSpeed * Time.deltaTime) * Mathf.Sign(deltaX);

        // Y轴移动（新增，和X轴逻辑完全一致，保证手感统一）
        float deltaY = mouseWorldPos.y - transform.position.y;
        float stepY = Mathf.Min(Mathf.Abs(deltaY), moveSpeed * Time.deltaTime) * Mathf.Sign(deltaY);

        transform.Translate(new Vector3(stepX, stepY, 0f));
    }

    /// <summary>
    /// 用场景中的BoxCollider2D限制玩家位置，确保玩家完全在区域内
    /// </summary>
    void ClampPositionToBounds()
    {
        // 未赋值移动区域Collider时，回退到原来的屏幕边界限制
        if (movementBoundsArea == null)
        {
            return;
        }

        // 获取移动区域的边界（世界坐标，每帧获取以支持动态移动的区域）
        Bounds bounds = movementBoundsArea.bounds;

        // 获取玩家自身的半尺寸（优先用碰撞体，没有则用渲染体，确保边界精确）
        Vector2 playerHalfSize = Vector2.zero;
        if (playerCollider != null)
        {
            playerHalfSize = playerCollider.bounds.extents; // extents是一半的尺寸
        }
        else if (spriteRenderer != null)
        {
            playerHalfSize = spriteRenderer.bounds.extents;
        }

        // 计算可移动的有效范围（减去玩家半尺寸，防止玩家半边身子伸出边界）
        float minX = bounds.min.x + playerHalfSize.x;
        float maxX = bounds.max.x - playerHalfSize.x;
        float minY = bounds.min.y + playerHalfSize.y;
        float maxY = bounds.max.y - playerHalfSize.y;

        // 限制玩家位置
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
    void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
            if (Input.GetKey(KeyCode.Space) || useAutoFire)
            {
                nextFireTime = Time.time + Mathf.Clamp(fireInterval, 0.01f, 10.0f);
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

    public void Heal(int amount)
    {
        health += amount;
        if (health > 10) health = 10;
        EventBus.Emit(new PlayerHealEvent(health));
    }

    void Die()
    {
        EventBus.Emit(new GameOverEvent(false));
    }
}