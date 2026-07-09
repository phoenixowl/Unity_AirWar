using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] int damage = 3;

    public void Init()
    {
        var cfg = ConfigManager.Instance.GameConfig;
        speed = cfg.bulletSpeed;
        damage = cfg.bulletDamage;
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // ·É³öÆÁÄ»
        if (transform.position.y > 6f)
        {
            ObjectPool.Instance.ReturnBullet(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
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
}