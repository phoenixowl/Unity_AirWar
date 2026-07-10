using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class HitFlashController : MonoBehaviour
{
    public float flashDuration = 0.08f;

    private Renderer rend;
    private MaterialPropertyBlock block;
    private Coroutine flashRoutine;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(DoFlash());
    }

    IEnumerator DoFlash()
    {
        SetFlashAmount(1f);

        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float amount = Mathf.Lerp(0f, 1f, ((t % 0.15f) / 0.15f));
            SetFlashAmount(amount);
            yield return null;
        }

        SetFlashAmount(0f);
    }
    public void ResetFlash()
    {
        if (rend == null) return;

        // 停止可能存在的闪烁协程（防止 Lerp 继续写值）
        StopAllCoroutines();

        // 取 → 改 → 还
        rend.GetPropertyBlock(block);
        block.SetFloat("_FlashAmount", 0f);
        rend.SetPropertyBlock(block);
    }

    void SetFlashAmount(float value)
    {
        rend.GetPropertyBlock(block);
        block.SetFloat("_FlashAmount", value);
        rend.SetPropertyBlock(block);
    }
    void OnDisable()
    {
        ResetFlash();
    }
}