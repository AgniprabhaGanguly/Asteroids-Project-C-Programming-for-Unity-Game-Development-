using UnityEngine;
using TMPro;
using System;
public class HealthScript : MonoBehaviour
{
    public static HealthScript Instance;
    
    int health = 100;
    public TextMeshProUGUI healthText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public float Health
    {
        get { return health; }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) health = 0;
        healthText.text = $"HP: {health}";
    }
}
