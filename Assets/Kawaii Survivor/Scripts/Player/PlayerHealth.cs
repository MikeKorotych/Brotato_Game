using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{


    [Header(" Settings ")]
    [SerializeField] private int maxHealth;
    private int health;


    [Header(" Elements ")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image sliderImage;
    [SerializeField] private TextMeshProUGUI healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;

        UpdateHealth();
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        if (health <= 0)
        {
            PassAway();
        }

        UpdateHealth();
        ChangeSliderColor();
    }

    private void UpdateHealth()
    {
        healthSlider.value = (float)health / maxHealth;
        healthText.text = health + " / " + maxHealth;
    }

    private void ChangeSliderColor()
    {
        float healthPercentage = (float)health / maxHealth;

        // H меняется от 0 (красный) до 120 (зелёный)
        float hue = healthPercentage * 120f;
        float saturation = 1f; // Полная насыщенность
        float value = 1f; // Полная яркость

        // Преобразование HSV в RGB
        Color color = Color.HSVToRGB(hue / 360f, saturation, value);
        sliderImage.color = color;
    }

    private void PassAway()
    {
        Debug.Log("--- dead ---");
        SceneManager.LoadScene(0);
    }
}
