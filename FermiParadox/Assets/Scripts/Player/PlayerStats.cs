using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public float currentHealth;
    public float maxHealth = 100;
    public Slider sliderHealth;
    float healthPercentage;

	// Use this for initialization
	void Start () {
        //sliderHealth.value = health;
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        //sliderHealth.value = health;
	}

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthPercentage = currentHealth / maxHealth;
        sliderHealth.value = healthPercentage;
    }
    /*
    public void UpdateHealth(int health)
    {
        health += health;
        sliderHealth.value = health;
    }
    */
}
