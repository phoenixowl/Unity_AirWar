using System.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameConfig")]
public class StatsConfigSO : ScriptableObject
{
    public int playerHP = 3;
    public float playerMoveSpeed = 6f;
    public float playerFireInterval = 0.2f;
    public float playerInvicibleTime = 0.5f;
    public int playerBulletDamage = 3;
    public float playerBulletSpeed = 10.0f;

    public int enemyHP = 10;
    public float enemyMoveSpeed = 2f;
    public float enemyFireInterval = 9999f;
    public int enemyTouchDamage = 1;
    public float enemySpawnInterval = 1.5f;
    public float enemyInvicibleTime = 0.2f;
    public int enemyBulletDamage = 1;
    public float enemyBulletSpeed = 3.0f;

    public int scoreEnemy = 100;

    public float eliteEnemyRate = 0.05f;

    public float expectedGameTime = 180f;
}