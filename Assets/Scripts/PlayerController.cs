using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movSpeed;
    [SerializeField] private float maxLife = 100f;

    [SerializeField] private float speedX, speedY;

    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private SpriteRenderer circleAttack;
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
            StartCoroutine(FlashHealing());
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
            StartCoroutine(FlashAttack());
        }
        gameController.UpdateUI(lifes, bullets, currentLife);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Collectible collectible = other.GetComponent<Collectible>();
        if (collectible == null) return;

        if (collectible.type == Collectible.Type.Bullets) bullets += 1;
        if (collectible.type == Collectible.Type.Life) lifes += 1;

        StartCoroutine(CollectibleFlashDestroy(collectible));
    }

    void OnTriggerStay2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            currentLife -= gameController.DamageSuffered * Time.deltaTime;
            StartCoroutine(Flash());
            if (currentLife < 0f) currentLife = 0f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }


    #region corrotinas
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

    IEnumerator FlashAttack()
    {
        for (int i = 0; i < 2; i++)
        {
            circleAttack.color = new Color(1f, 0f, 0f, 0.5f);
            yield return new WaitForSeconds(0.1f);

            circleAttack.color = new Color(1f, 1f, 1f, 0.05f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FlashHealing()
    {
        for (int i = 0; i < 2; i++)
        {
            circleAttack.color = new Color(0f, 1f, 0f, 0.25f);
            yield return new WaitForSeconds(0.15f);

            circleAttack.color = new Color(1f, 1f, 1f, 0.05f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator CollectibleFlashDestroy(Collectible collectible)
    {
        SpriteRenderer spriteRenderer = collectible.GetComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = collectible.GetComponent<BoxCollider2D>();

        if (boxCollider != null) boxCollider.enabled = false;

        if (spriteRenderer != null)
        {
            for (int i = 0; i < 2; i++)
            {
                Color color = spriteRenderer.color;

                color.a = 0f;
                spriteRenderer.color = color;
                yield return new WaitForSeconds(0.05f);

                color.a = 1f;
                spriteRenderer.color = color;
                yield return new WaitForSeconds(0.05f);
            }
        }

        Destroy(collectible.gameObject);
    }
    #endregion

}
