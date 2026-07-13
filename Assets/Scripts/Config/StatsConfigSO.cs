using System.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameConfig")]
public class StatsConfigSO : ScriptableObject
{
    public int playerHP = 3;
    public float playerMoveSpeed = 6f;
    public float playerFireInterval = 0.5f;
    public float playerInvicibleTime = 0.5f;
    public int playerBulletDamage = 3;
    public float playerBulletSpeed = 10.0f;

    public int enemyHP = 10;
    public float enemyMoveSpeed = 2f;
    public float enemyFireInterval = 9999f;
    public int enemyTouchDamage = 1;
    public int enemyBulletDamage = 1;
    public float enemyBulletSpeed = 3.0f;

    public int eliteEnemyHP = 15;
    public float eliteEnemyMoveSpeed = 1f;
    public float eliteEnemyFireInterval = 3.0f;
    public int eliteEnemyTouchDamage = 1;
    public int eliteEnemyBulletDamage = 1;
    public float eliteEnemyBulletSpeed = 3.0f;

    public float itemMoveSpeed = 5f;
    public int itemHeal = 1;
    public int itemScore = 300;
    public float itemShotSpeed = 0.05f;

    public int scoreEnemy = 100;
    public int scoreEliteEnemy = 300;

    public float eliteEnemyRate = 0.05f;
    public float enemySpawnInterval = 1.5f;
    public float enemyInvicibleTime = 0.1f;
    public float itemSpawnInterval = 10f;

    public float expectedGameTime = 180f;
}