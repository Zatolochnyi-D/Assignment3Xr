using UnityEngine;

public class ButterflySpawner : MonoBehaviour
{
    [SerializeField] private GameObject butterflyPrefab;
    [SerializeField] private Transform spawnpoint;
    [SerializeField] private float delayBeforeSpawning = 2f;
    [SerializeField] private Vector2 delayBetweenSpawning = new(3f, 7f);

    void Start()
    {
        StartSpawning();
    }
    
    private async void StartSpawning()
    {
        await Awaitable.WaitForSecondsAsync(delayBeforeSpawning);
        while (true)
        {
            Instantiate(butterflyPrefab, spawnpoint.position, Quaternion.identity);
            await Awaitable.WaitForSecondsAsync(Random.Range(delayBetweenSpawning.x, delayBetweenSpawning.y));
        }
    }
}