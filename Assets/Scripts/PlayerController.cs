using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movSpeed;
    [SerializeField] private float maxLife = 100f;

    [SerializeField] private float speedX, speedY;

    [SerializeField] private float attackRadius = 1.5f;
    Rigidbody2D rb;

    public int bullets;
    public int lifes;

    public float currentLife;
    private GameController gameController;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentLife = maxLife;
        gameController = FindObjectOfType<GameController>();
        gameController.LifeSlider.maxValue = maxLife;
        gameController.LifeSlider.value = currentLife;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && lifes > 0 && currentLife < maxLife)
        {
            print("Healing");
            lifes -= 1;
            currentLife += gameController.LifeRecover;
            if (currentLife > maxLife) currentLife = maxLife;
        }
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) && bullets > 0)
        {
            print("Firing");
            bullets -= 1;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
            foreach (var hit in hits)
            {
                EnemyController enemy = hit.GetComponent<EnemyController>();
                if (enemy != null) enemy.TakeDamage(gameController.DamageCaused);
            }
        }

        gameController.UpdateUI(lifes, bullets, currentLife);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            currentLife -= gameController.DamageSuffered;
            StartCoroutine(Flash());
            if (currentLife < 0f) currentLife = 0f;
        }

        Collectible collectible = other.GetComponent<Collectible>();
        if (collectible == null) return;

        if (collectible.type == Collectible.Type.Bullets) bullets += 1;
        if (collectible.type == Collectible.Type.Life) lifes += 1;

        Destroy(other.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
