using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] int maxHealth = 5;
    [SerializeField] int currentHealth;

    [Header("Attack")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletSpeed = 10f;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");
        gameObject.SetActive(false);
    }

    // ===================
    // Player Attack
    // ===================
    public void OnAttack()
    {
        anim.SetTrigger("Attack");
        Shoot();
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Flip bullet horizontally seg�n direcci�n del player
            Vector3 scale = bullet.transform.localScale;
            scale.x = transform.localScale.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            bullet.transform.localScale = scale;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.up * bulletSpeed;
            }

            BulletBehavior bb = bullet.AddComponent<BulletBehavior>();
            bb.damage = 9999; 
        }
    }
}