using System.Data;

public interface IShooter
{
    void Shoot(bool isPlayerBullet, int damage, float bulletSpeed);
}