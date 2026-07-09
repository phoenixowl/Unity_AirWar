using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class HitFlashController : MonoBehaviour
{
    public float flashDuration = 0.08f;

    private SpriteRenderer sr;
    private MaterialPropertyBlock block;
    private Coroutine flashRoutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
        if (sr == null) return;

        // 停止可能存在的闪烁协程（防止 Lerp 继续写值）
        StopAllCoroutines();

        // 取 → 改 → 还
        sr.GetPropertyBlock(block);
        block.SetFloat("_FlashAmount", 0f);
        sr.SetPropertyBlock(block);
    }

    void SetFlashAmount(float value)
    {
        sr.GetPropertyBlock(block);
        block.SetFloat("_FlashAmount", value);
        sr.SetPropertyBlock(block);
    }
    void OnDisable()
    {
        ResetFlash();
    }
}