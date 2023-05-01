using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockCage : MonoBehaviour
{
    private bool canUnlock;
    public GameObject message;

    public CharacterSelector[] charSelects;
    private CharacterSelector playerToUnlock;

    public SpriteRenderer cageSR;

    private void Start()
    {
        playerToUnlock = charSelects[Random.Range(0, charSelects.Length)];

        cageSR.sprite = playerToUnlock.playerToSpawn.bodySR.sprite;
    }

    private void Update()
    {
        if (canUnlock)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);

                Instantiate(playerToUnlock, transform.position, transform.rotation);

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
