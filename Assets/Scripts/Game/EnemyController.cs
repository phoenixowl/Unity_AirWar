using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    int hp = 10;
    int score = 1;
    float moveSpeed = 2.0f;
    float invicibleTime = 1.0f;
    float invicibleTimer = 0f;

    HitFlashController hitFlashController;

    public void Init(int hp, int score, float speed, float invicibleTime)
    {
        this.hp = hp;
        this.score = score;
        this.moveSpeed = speed;
        this.invicibleTime = invicibleTime;
    }

    void Awake()
    {
        hitFlashController = GetComponent<HitFlashController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (invicibleTimer > 0f)
        {
            invicibleTimer -= Time.deltaTime;
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
        ScoreManager.Instance.Score += score;

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
    }
}  