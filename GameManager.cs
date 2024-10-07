using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button buttonStart;
    public Button buttonRestart;
    public GameObject road1;
    public GameObject road2;
    public GameObject gameOver;
    public float speed = 5f;

    public PlayerCarController playerCarController;
    public CarSpawner carSpawner;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI carsAvoidedText;
    public TextMeshProUGUI gameOverText;

    private int carsAvoided = 0;
    private int level = 1;
    private bool isGameStart = false;
    private bool isGameOver = false;
    private bool hasWon = false;
    private float roadHeight;
    private float speedIncrement = 3f;
    private float spawnIntervalDecrement = 0.2f;
    private float minSpawnInterval = 0.5f;

    void Start()
    {
        roadHeight = road1.GetComponent<SpriteRenderer>().bounds.size.y;

        Time.timeScale = 1f;
        buttonStart.gameObject.SetActive(true);
        buttonRestart.gameObject.SetActive(false);
        gameOver.SetActive(false);
        UpdateCarsAvoidedUI();
        UpdateLevelUI();
    }

    void Update()
    {
        if (isGameStart && !isGameOver)
        {
            road1.transform.Translate(Vector2.down * speed * Time.deltaTime);
            road2.transform.Translate(Vector2.down * speed * Time.deltaTime);

            if (road1.transform.position.y <= -roadHeight)
            {
                Vector2 newPosition = new Vector2(road1.transform.position.x, road2.transform.position.y + roadHeight);
                road1.transform.position = newPosition;
            }

            if (road2.transform.position.y <= -roadHeight)
            {
                Vector2 newPosition = new Vector2(road2.transform.position.x, road1.transform.position.y + roadHeight);
                road2.transform.position = newPosition;
            }
        }

        Time.timeScale = isGameOver ? 0f : 1f;
    }

    public void StartGame()
    {
        isGameStart = true;
        isGameOver = false;
        hasWon = false;
        buttonStart.gameObject.SetActive(false);
        buttonRestart.gameObject.SetActive(false);
        gameOver.SetActive(false);

        carsAvoided = 0;
        level = 1;
        speed = 5f;
        carSpawner.AdjustSpawningSpeed(2f, 3f);
        UpdateCarsAvoidedUI();
        UpdateLevelUI();

        playerCarController.StartGame();
        carSpawner.StartSpawning();
    }

    public void GameOver()
    {
        isGameStart = false;
        isGameOver = true;

        Time.timeScale = 0f;
        buttonStart.gameObject.SetActive(false);
        buttonRestart.gameObject.SetActive(true);
        gameOver.SetActive(true);
        gameOverText.text = "Game Over!";
    }

    public void RestartGame()
    {
        isGameStart = true;
        isGameOver = false;
        hasWon = false;
        Time.timeScale = 1f;

        road1.transform.position = new Vector2(0, 0);
        road2.transform.position = new Vector2(0, road1.transform.position.y + roadHeight);

        playerCarController.ResetCar();
        carSpawner.StopSpawning();
        carSpawner.RemoveAllCars();
        carSpawner.AdjustSpawningSpeed(2f, 3f);
        carSpawner.StartSpawning();

        carsAvoided = 0;
        level = 1;
        speed = 5f;
        UpdateCarsAvoidedUI();
        UpdateLevelUI();

        gameOver.SetActive(false);
    }


    public void CarAvoided()
    {
        if (hasWon) return;

        carsAvoided++;
        UpdateCarsAvoidedUI();

        if (carsAvoided % 10 == 0)
        {
            level++;
            speed += speedIncrement;

            float newSpawnInterval = Mathf.Max(minSpawnInterval, carSpawner.spawnInterval - spawnIntervalDecrement);
            float newCarSpeed = carSpawner.carSpeed + speedIncrement;
            carSpawner.AdjustSpawningSpeed(newSpawnInterval, newCarSpeed);

            UpdateLevelUI();
        }

        if (carsAvoided > 29)
        {
            YouWon();
        }
    }

    private void YouWon()
    {
        isGameStart = false;
        isGameOver = true;
        hasWon = true;

        Time.timeScale = 0f;
        gameOver.SetActive(true);
        gameOverText.text = "You Won!"; 
        buttonRestart.gameObject.SetActive(true); 
    }

    void UpdateCarsAvoidedUI()
    {
        carsAvoidedText.text = carsAvoided.ToString();
    }

    void UpdateLevelUI()
    {
        levelText.text = level.ToString();
    }
}
