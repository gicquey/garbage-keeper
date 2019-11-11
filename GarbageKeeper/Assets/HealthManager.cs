using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private Quaternion initialRotation;

    public Ennemi enemy;
    public Image healthBar;

    private float startHealth;
    private float currentHealth;

    void Start()
    {
        initialRotation = transform.rotation;
        enemy = GetComponentInParent<Ennemi>();
        startHealth = (int)Settings.Instance.baseEnnemyMaxLife * enemy.maxLifeMultiplier;

    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = enemy._currentLife;
        healthBar.fillAmount = currentHealth / startHealth;
        transform.rotation = initialRotation;
    }
}
