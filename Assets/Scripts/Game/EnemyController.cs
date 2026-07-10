using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject outlineChildItem;
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
            outlineChildItem.SetActive(true);
        }
        else
        {
            outlineChildItem.SetActive(false);
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

        ReturnToPool();
    }
    void ReturnToPool()
    {
        if (gameObject.name.Contains("Elite"))
            ObjectPool.Instance.ReturnEnemyElite(gameObject);
        else
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