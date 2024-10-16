using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IPlayerStatsDependency
{
    [Header(" Settings ")]
    [SerializeField] private int baseMaxHealth;
    [SerializeField] private float maxDamageReductionByArmor;
    [SerializeField] private int maxDodge;
    private float maxHealth;
    private float health;
    private float lifeSteal;
    private float armor;
    private float dodge;
    private float healthRecoverySpeed;
    private float healthRecoveryTimer;
    private float healthRecoveryDuration;

    [Header(" Elements ")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image sliderImage;
    [SerializeField] private TextMeshProUGUI healthText;


    [Header(" Actions ")]
    public static Action<Vector2> onAttackDodged;

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    private void Update()
    {
        if (health < maxHealth)
        {
            RecoverHealth();
        }
    }

    private void RecoverHealth()
    {
        healthRecoveryTimer += Time.deltaTime;

        if (healthRecoveryTimer > healthRecoveryDuration)
        {
            healthRecoveryTimer = 0;

            float healthToAdd =  Mathf.Min(0.1f, maxHealth - health);
            health += healthToAdd;

            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;

    }

    private void EnemyTookDamageCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        if (health >= maxHealth)
            return;

        float lifeStealValue = damage * lifeSteal;
        float healthToAdd = MathF.Min(lifeStealValue, maxHealth - health);

        health += healthToAdd;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (ShouldDodge())
        {
            onAttackDodged?.Invoke(transform.position);
            return;
        }

        armor = Mathf.Clamp(armor, 0, maxDamageReductionByArmor);
        float realDamage = Mathf.Clamp(1 - (armor / 100), 0, 10000);
        realDamage = Mathf.Min(realDamage, health);
        health -= realDamage;


        Debug.Log("--- real damage: " + realDamage + " ---");

        UpdateUI();

        if (health <= 0)
            PassAway();

        ChangeSliderColor();
    }

    private bool ShouldDodge()
    {
        dodge = Mathf.Clamp(dodge, 0, maxDodge);
        return Random.Range(0f, 100f) <= dodge;
    }

    private void UpdateUI()
    {
        healthSlider.value = health / maxHealth;
        healthText.text = (int)health + " / " + maxHealth;
    }

    private void ChangeSliderColor()
    {
        float healthPercentage = health / maxHealth;

        // Цвета в формате RGB
        Color greenColor = new Color(0.541f, 0.690f, 0.376f); // #8ab060
        Color redColor = new Color(0.706f, 0.322f, 0.322f);   // #b45252

        // Линейная интерполяция между зелёным и красным
        Color sliderColor = Color.Lerp(redColor, greenColor, healthPercentage);

        // Применяем цвет к слайдеру
        sliderImage.color = sliderColor;
    }


    private void PassAway()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float addedHealth = playerStatsManager.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth, 1);

        health = maxHealth;
        UpdateUI();

        armor = playerStatsManager.GetStatValue(Stat.Armor);
        lifeSteal = playerStatsManager.GetStatValue(Stat.LifeSteal) / 100;
        dodge = playerStatsManager.GetStatValue(Stat.Dodge);

        healthRecoverySpeed = Mathf.Max(0.0001f, playerStatsManager.GetStatValue(Stat.HpRegenSpeed));
        healthRecoveryDuration = 1f / healthRecoverySpeed;
    }
}
