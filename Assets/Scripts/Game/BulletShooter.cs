using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(bool isPlayerBullet, int damage, float bulletSpeed)
    {
        GameObject bullet = ObjectPool.Instance.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        bullet.GetComponent<BulletController>().Init(damage, bulletSpeed, isPlayerBullet);
    }
}
