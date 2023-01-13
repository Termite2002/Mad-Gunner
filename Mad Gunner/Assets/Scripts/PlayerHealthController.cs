using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamagePlayer()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            PlayerController.instance.gameObject.SetActive(false);
        }
    }
}
