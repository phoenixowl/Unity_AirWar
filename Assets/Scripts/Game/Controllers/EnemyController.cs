using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public int score = 1;

    public int hp = 10;
    public float moveSpeed = 2.0f;
    public float fireInterval = 1000;
    public float nextFireTime = 0;
    public int touchDamage = 1;

    public int bulletDamage = 1;
    public float bulletSpeed = 5.0f;

    public float invicibleTime = 1.0f;
    public float invicibleTimer = 0f;

    [SerializeField] BulletShooter bulletShooter;
    [SerializeField] ParticleSystem deathExplosion;
    [SerializeField] Sprite normalEnemySprite;
    [SerializeField] Sprite eliteEnemySprite;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] BoxCollider2D collider;
    HitFlashController hitFlashController;

    public void Init(int score,int hp, float moveSpeed, float fireInterval, int touchDamage, int bulletDamage, float bulletSpeed, float invicibleTime, bool isElite)
    {
        this.score = score;
        this.hp = hp;
        this.fireInterval = fireInterval;
        this.moveSpeed = moveSpeed;
        this.touchDamage = touchDamage;
        this.bulletDamage = bulletDamage;
        this.invicibleTime = invicibleTime;
        if (isElite)
        {
            sr.sprite = eliteEnemySprite;
            transform.localScale = Vector3.one;
            collider.size = new Vector2(0.77f, 0.98f);
        }
        else
        {
            sr.sprite= normalEnemySprite;
            transform.localScale = Vector3.one * 1.25f;
            collider.size = new Vector2(0.64f, 0.45f);
        }
        nextFireTime = Time.time + this.fireInterval;
    }

    void Awake()
    {
        hitFlashController = GetComponent<HitFlashController>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        TryShoot();

        if (invicibleTimer > 0f)
        {
            invicibleTimer -= Time.deltaTime;
        }
    }

    void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
           nextFireTime = Time.time + fireInterval;

           bulletShooter.Shoot(false, bulletDamage, bulletSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        if (invicibleTimer > 0) return;
        hp -= damage;

        invicibleTimer = invicibleTime;

        hitFlashController.flashDuration = invicibleTime;
        hitFlashController.Flash();

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        EventBus.Emit(new EnemyDiedEvent(score));

        GameObject explosion = ObjectPool.Instance.GetExplosionParticle();

        if (explosion != null)
        {
            // 2. 将爆炸的位置设置在敌人死亡的坐标
            explosion.transform.position = transform.position;

            // 如果需要，也可以同步旋转
            explosion.transform.rotation = transform.rotation;

            explosion.SetActive(true);

            // 3. 播放特效
            var ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
        }

        ReturnToPool();
    }
    void ReturnToPool()
    {
         ObjectPool.Instance.ReturnEnemyNormal(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DownWall"))
        {
            ReturnToPool();
        }
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(touchDamage);
            }
        }
    }
}  