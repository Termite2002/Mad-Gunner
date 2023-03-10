using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 7.5f;
    public Rigidbody2D theRB;

    public GameObject impactEffect;
    public GameObject hurtEnemyEffect;

    public int damageToGive = 50;

    void Update()
    {
        theRB.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Environment")
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(4);

        if (other.tag == "Enemy")
        {
            Instantiate(hurtEnemyEffect, transform.position, transform.rotation);
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }
    }
    private void OnBecameInvisible()    
    {
        Destroy(gameObject);
    }
}
