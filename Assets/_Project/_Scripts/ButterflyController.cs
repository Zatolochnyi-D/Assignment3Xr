using System.Threading;
using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    [SerializeField] private float spawnInTime = 1f;
    [SerializeField] private Vector2 speedRange = new(2f, 5f);
    [SerializeField] private Vector2 changeRange = new(3f, 5f);
    [SerializeField] private Transform rightWing;
    [SerializeField] private Transform leftWing;
    [SerializeField] private float swingSpeed = 80f;
    [SerializeField] private float rightLimitation = 70f;
    [SerializeField] private float leftLimitation = -70f;

    private float currentMovementSpeed;
    private bool wingDirection = false;

    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    void Start()
    {
        Spawn(destroyCancellationToken);
        Shuffle(destroyCancellationToken);
        Move(destroyCancellationToken);
        AnimateWings(destroyCancellationToken);
    }

    private async void Spawn(CancellationToken token)
    {
        for (var elapsedTime = 0f; elapsedTime < spawnInTime; elapsedTime += Time.deltaTime)
        {
            if (token.IsCancellationRequested)
                return;
            transform.localScale = Mathf.Min(elapsedTime / spawnInTime, 1f) * Vector3.one;
            await Awaitable.NextFrameAsync();
        }
    }

    private async void Shuffle(CancellationToken token)
    {
        while (true)
        {
            if (token.IsCancellationRequested)
                return;
            var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            transform.forward = direction;
            currentMovementSpeed = Random.Range(speedRange.x, speedRange.y);
            await Awaitable.WaitForSecondsAsync(Random.Range(changeRange.x, changeRange.y));
        }
    }

    private async void Move(CancellationToken token)
    {
        while (true)
        {
            if (token.IsCancellationRequested)
                return;
            transform.position += Time.deltaTime * currentMovementSpeed * transform.forward;
            await Awaitable.NextFrameAsync();
        }
    }

    private async void AnimateWings(CancellationToken token)
    {
        while (true)
        {
            if (token.IsCancellationRequested)
                return;
            var right = Mathf.Min(Time.deltaTime * swingSpeed * (wingDirection ? -1 : 1), rightLimitation);
            if (right == rightLimitation)
                wingDirection = !wingDirection;
            var left = Mathf.Max(Time.deltaTime * swingSpeed * (wingDirection ? 1 : -1), leftLimitation);
            rightWing.localEulerAngles += new Vector3(0f, right, 0f);
            leftWing.localEulerAngles += new Vector3(0f, left, 0f);
            await Awaitable.NextFrameAsync();
        }
    }
}
