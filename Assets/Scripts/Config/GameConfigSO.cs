using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameConfig")]
public class GameConfigSO : ScriptableObject
{
    public int playerHP = 3;
    public float playerMoveSpeed = 6f;
    public float fireInterval = 0.2f;

    public float bulletSpeed = 10f;
    public int bulletDamage = 3;

    public int enemyNormalHP = 10;
    public int enemyEliteHP = 30;
    public float enemySpeed = 2f;
    public float enemySpawnInterval = 1.5f;
    public float enemyInvicibleTime = 0.5f;

    public int scoreNormalEnemy = 100;
    public int scoreEliteEnemy = 300;
}