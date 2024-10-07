using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public GameManager gameManager;
    public float speed = 3f;

    void Update()
    {
        if (gameManager == null)
        {
            return;
        }

        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < -6f) 
        {
            gameManager.CarAvoided();
            Destroy(gameObject);
        }
    }
}
