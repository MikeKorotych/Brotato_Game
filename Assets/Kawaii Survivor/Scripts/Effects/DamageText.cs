using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro damageText;
    [SerializeField] private int fontSize;

    public void PlayAnimation(string damage, bool isCriticalHit)
    {
        damageText.text = damage.ToString();
        damageText.color = isCriticalHit ? Color.yellow : Color.white;

        // Увеличиваем размер шрифта на 30% при критическом ударе
        float originalFontSize = damageText.fontSize;
        originalFontSize = fontSize;
        damageText.fontSize = isCriticalHit ? (int)(originalFontSize * Random.Range(1.5f, 2f)) : originalFontSize;

        animator.Play("AnimateText");
    }
}
