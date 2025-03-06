using UnityEngine;

public class Drop : MonoBehaviour
{
    public float healAmount = 10f;
    public float baseMoveSpeed = 10f;
    public float maxMoveSpeed = 100f;
    public float speedIncreaseRate = 6f;
    public float attractDistance = 4f;
    public float countdownTime = 20f;

    private Transform player;
    private float countdownTimer;
    private bool isCountingDown = false;
    private PlayerHealthBar playerHealthBar;
    private float currentMoveSpeed;
    private float timeInRange = 0f;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
        playerHealthBar = playerObject.GetComponentInChildren<PlayerHealthBar>();

        countdownTimer = countdownTime;
        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attractDistance)
        {
            timeInRange += Time.deltaTime;
            currentMoveSpeed = Mathf.Min(baseMoveSpeed * Mathf.Pow(speedIncreaseRate, timeInRange), maxMoveSpeed);

            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * currentMoveSpeed * Time.deltaTime);

            isCountingDown = false;
            countdownTimer = countdownTime;
        }
        else
        {
            currentMoveSpeed = baseMoveSpeed;
            timeInRange = 0f;

            if (!isCountingDown)
            {
                isCountingDown = true;
            }

            if (isCountingDown)
            {
                countdownTimer -= Time.deltaTime;

                if (countdownTimer <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player picked up drop");
            playerHealthBar.Heal(healAmount, 1);
            Destroy(gameObject);
        }
    }
}
