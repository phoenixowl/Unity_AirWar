using UnityEngine;

public class ExplosionParticle : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();

        // 关键设置：强制粒子系统停止时触发回调
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    // 当粒子系统完全停止且没有存活粒子时，Unity 会自动调用这个内置函数
    void OnParticleSystemStopped()
    {
        ReturnToPool();
    }

    void ReturnToPool()
    {
        ObjectPool.Instance.ReturnExplosionParticle(gameObject);
    }
}