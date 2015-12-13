using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public Image healthBar;
    private float maxHealth;

    public void Init(float maxHealth) {
        this.maxHealth = maxHealth;
    }
    public void UpdateHealthBar(float currentHealth) {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

}
