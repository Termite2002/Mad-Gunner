using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPiecies;
    public int maxPieces = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerController.instance.dashCounter > 0)
            {
                Destroy(gameObject);

                int piecesToDrop = Random.Range(0, maxPieces);

                for (int i = 1; i <= piecesToDrop; i++)
                {
                    int randomPiece = Random.Range(0, brokenPiecies.Length);
                    Instantiate(brokenPiecies[randomPiece], transform.position, transform.rotation);
                }
            }
              
        }
    }
}
