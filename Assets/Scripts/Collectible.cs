using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private bool isCollected;

    private void Awake()
    {
        Collider2D itemCollider = GetComponent<Collider2D>();
        itemCollider.isTrigger = true;
    }

    public void Collect()
    {
        if (isCollected)
        {
            return;
        }

        isCollected = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(value);
        }

        Destroy(gameObject);
    }
}
