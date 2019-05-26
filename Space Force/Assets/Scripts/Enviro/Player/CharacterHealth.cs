using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
    public GameObject body;

    public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = 30f;
        CurrentHealth = MaxHealth;


        healthBar.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // tells healthbar to be damaged by given value
    public void DealDamage(float damageValue)
    {
        CurrentHealth -= damageValue;
        healthBar.value = CalculateHealth();

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else if (healthBar.value <= 0.25f)
        {
            Debug.Log("Health is below 1/4, playing alert sound");
            print(healthBar.value);

            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }

        }
        
    }
    // death of player
    void Die()
    {
        CurrentHealth = 0;
        
        Debug.Log("Player is dead and has no health");
        body.GetComponent<Body>().Reset();
        CurrentHealth = MaxHealth;
        healthBar.value = CalculateHealth();
        if (GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }
}
