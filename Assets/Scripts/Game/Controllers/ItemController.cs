using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public enum Type { Heal, Score, Shot };


    public float moveSpeed;
    [SerializeField] public Type type;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] ParticleSystem pc;
    StatsConfigSO cfg;


    void Start()
    {
        cfg = ConfigManager.Instance.StatsConfigSO;
    }

    public void Init(float moveSpeed, int t)
    {
        this.moveSpeed = moveSpeed;
        type = (Type)t;
        var trailController = pc.GetComponent<ParticleTrailController>();
        switch (type)
        {
            case Type.Heal:
                sr.color = Color.red;
                trailController.SetTrailColor(Color.red, 2.0f);
                break;
            case Type.Score:
                sr.color = Color.yellow;
                trailController.SetTrailColor(Color.yellow, 2.0f);
                break;
            case Type.Shot:
                sr.color = Color.green;
                trailController.SetTrailColor(Color.green, 2.0f);
                break;
        }
    }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }

        void ReturnToPool()
        {
            ObjectPool.Instance.ReturnItem(gameObject);
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
                    switch (type)
                    {
                        case Type.Heal:
                            player.Heal(cfg.itemHeal);
                            break;
                        case Type.Score:
                            ScoreManager.Instance.Score += cfg.itemScore;
                            break;
                        case Type.Shot:
                            player.fireInterval -= cfg.itemShotSpeed;
                            break;
                    }
                    ReturnToPool();
                }
            }
        }
    }
