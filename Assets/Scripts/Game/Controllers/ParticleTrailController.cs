using UnityEngine;

public class ParticleTrailController : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// 一键改变拖尾颜色
    /// </summary>
    /// <param name="targetColor">目标基本颜色</param>
    /// <param name="glowIntensity">发光强度（配合 HDR/Bloom 使用，1为默认，数值越大越亮）</param>
    public void SetTrailColor(Color targetColor, float glowIntensity = 1.0f)
    {
        if (ps == null) return;

        // 计算带高光的 HDR 颜色
        Color hdrColor = targetColor * glowIntensity;

        // ==========================================
        // 1. 控制 Start Color (基础模块)
        // ==========================================
        var mainModule = ps.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(hdrColor);

        // ==========================================
        // 2. 控制 Color over Lifetime (生命周期颜色模块)
        // ==========================================
        var colorModule = ps.colorOverLifetime;

        // 确保该模块已启用
        colorModule.enabled = true;

        // 用代码创建一个全新的渐变色对象
        Gradient gradient = new Gradient();

        // 设置颜色关键帧（Color Keys）：从头到尾都保持目标颜色
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(targetColor, 0.0f); // 头部
        colorKeys[1] = new GradientColorKey(targetColor, 1.0f); // 尾部

        // 设置透明度关键帧（Alpha Keys）：复刻 80% 处极速消失的“鲜明感”
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];
        alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);  // 0% 处完全不透明
        alphaKeys[1] = new GradientAlphaKey(1.0f, 0.8f);  // 到了 80% 依然完全不透明（保持实体感）
        alphaKeys[2] = new GradientAlphaKey(0.0f, 0.85f); // 到了 85% 瞬间透明度归零（极速截断）

        // 将键位分派给渐变色
        gradient.SetKeys(colorKeys, alphaKeys);

        // 将写好的渐变色赋给模块
        colorModule.color = new ParticleSystem.MinMaxGradient(gradient);
    }
}