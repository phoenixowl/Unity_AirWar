using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 3;
    bool isPlayerBullet = true;

    public void Init(int damage, float speed, bool isPlayerBullet)
    {
        this.damage = damage;
        this.speed = speed;
        this.isPlayerBullet = isPlayerBullet;
        if(!isPlayerBullet)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // ·É³öÆÁÄ»
        if (transform.position.y > 6f || transform.position.y < -6f)
        {
            ObjectPool.Instance.ReturnBullet(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerBullet)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                ObjectPool.Instance.ReturnBullet(gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
                ObjectPool.Instance.ReturnBullet(gameObject);
            }
        }
    }
}