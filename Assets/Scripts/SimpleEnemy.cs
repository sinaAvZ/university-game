using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleEnemy : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2.5f;

    private Vector3 target;
    private const float ArrivalDistance = 0.08f;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            enabled = false;
            return;
        }

        target = pointB.position;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) <= ArrivalDistance)
        {
            target = Vector3.Distance(target, pointA.position) <= ArrivalDistance ? pointB.position : pointA.position;
        }
    }

    public void Configure(Transform a, Transform b, float moveSpeed)
    {
        pointA = a;
        pointB = b;
        speed = moveSpeed;
        target = pointB != null ? pointB.position : transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null && GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA.position, pointB.position);
        Gizmos.DrawWireSphere(pointA.position, 0.2f);
        Gizmos.DrawWireSphere(pointB.position, 0.2f);
    }
}
