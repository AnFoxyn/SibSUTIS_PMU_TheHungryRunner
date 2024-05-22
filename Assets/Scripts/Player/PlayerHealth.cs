using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private Animator animator;
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private int maxHealth = 3;
    private float cooldouwn = 0f;
    private int currentHealth;

    public GameController gameController;
    private bool isDead = false;

    void Start()
    {
        ResetHealth();
        GameController.OnReset += ResetHealth;
    }

    void Update()
    {
        cooldouwn -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Spike spike =  collision.GetComponent<Spike>();
        if (spike && spike.damage > 0)
            TakeDamage(spike.damage);
    }

    void ResetHealth()
    {
        isDead = false;
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }

    private void TakeDamage(int damage)
    {
        if (cooldouwn <= 0)
        {
            currentHealth -= damage;
            healthUI.UpdateHearts(currentHealth);

            StartCoroutine(getDamage());
        }

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            gameController.GameOverScreen();
        }    
    }

    private IEnumerator getDamage()
    {
        cooldouwn = 3f;
        animator.SetTrigger("getDamage");
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
    }
}
