using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float maxLife = 40f;
    [SerializeField] private float currentLife;

    private SpriteRenderer sprite;

    void Start()
    {
        currentLife = maxLife;
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        currentLife -= damage;
        StartCoroutine(Flash());
        if (currentLife <= 0f) Die();
    }

    IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white; 
    }

    void Die()
    {
        Destroy(gameObject);
    }
}