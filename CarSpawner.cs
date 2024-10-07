using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject randomCar;
    public TextMeshProUGUI speedText;
    public float spawnInterval = 2f;
    public float minX = -2f;
    public float maxX = 2f;
    public float spawnY = 5f;
    public float carSpeed = 3f;

    private bool isGameStarted = false;
    private List<GameObject> activeCars = new List<GameObject>();

    IEnumerator SpawnCars()
    {
        while (!isGameStarted)
        {
            yield return null;
        }

        while (isGameStarted)
        {
            SpawnCar();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCar()
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, spawnY);
        GameObject car = Instantiate(randomCar, spawnPosition, Quaternion.identity);

        CarMovement carMovement = car.AddComponent<CarMovement>();
        carMovement.speed = carSpeed;
        carMovement.gameManager = FindObjectOfType<GameManager>();

        activeCars.Add(car);

        speedText.text = carMovement.speed.ToString();
    }

    public void RemoveAllCars()
    {
        foreach (GameObject car in activeCars)
        {
            if (car != null)
            {
                Destroy(car);
            }
        }
        activeCars.Clear();
    }

    public void StartSpawning()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            StartCoroutine(SpawnCars());
        }
    }

    public void StopSpawning()
    {
        isGameStarted = false;
        StopAllCoroutines();
    }

    public void AdjustSpawningSpeed(float newSpawnInterval, float newCarSpeed)
    {
        spawnInterval = newSpawnInterval;
        carSpeed = newCarSpeed;
    }
}
