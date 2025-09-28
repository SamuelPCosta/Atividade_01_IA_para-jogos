using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float maxLife = 40f;
    [SerializeField] private float currentLife;
    [SerializeField] private float speed = 3f;
    [SerializeField] private Animator animator;

    private SpriteRenderer sprite;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        currentLife = maxLife;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    [SerializeField] private float minRotationDistance = 0.05f;
    [SerializeField] private float stopDistance = 0.25f;

    void Update()
    {
        if (player == null) return;

        if (rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance > stopDistance)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                rb.velocity = dir * speed;

                if (distance > minRotationDistance)
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }

                if (animator != null) animator.speed = 1f;
            }
            else
            {
                rb.velocity = Vector2.zero;
                if (animator != null) animator.speed = 0f;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            if (animator != null) animator.speed = 0f;
        }
    }



    public void TakeDamage(float damage)
    {
        currentLife -= damage;
        StartCoroutine(Flash());
        if (currentLife <= 0f) Die();
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 2; i++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}