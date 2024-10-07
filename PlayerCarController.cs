using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    public float moveSpeed = 5f;  
    public float minX;             
    public float maxX;             

    private bool isGameStarted = false; 

    public GameManager gameManager; 

    void Update()
    {
        if (isGameStarted)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            Vector2 newPosition = transform.position;
            newPosition.x += horizontalInput * moveSpeed * Time.deltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            transform.position = newPosition;
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D playerCar)
    {
        if (playerCar.gameObject.CompareTag("RandomCar"))
        {
            gameManager.GameOver();
        }
    }

    public void ResetCar()
    {
        transform.position = new Vector2(0, -3f);
    }

}
