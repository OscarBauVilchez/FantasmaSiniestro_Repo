using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invulnerabilitySeconds = 0.25f;

    [Header("Damage Sources (Tags)")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string projectileTag = "Projectile";

    public int CurrentHealth { get; private set; }

    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action OnDied;

    private bool invulnerable;

    private void Awake()
    {
        CurrentHealth = Mathf.Max(1, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryTakeDamageFrom(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryTakeDamageFrom(other.gameObject);
    }

    private void TryTakeDamageFrom(GameObject other)
    {
        if (invulnerable) return;

        if (other.CompareTag(enemyTag) || other.CompareTag(projectileTag))
        {
            TakeDamage(1);

            // Si quieres destruir el proyectil al tocar:
            if (other.CompareTag(projectileTag))
                Destroy(other);
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || invulnerable) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (invulnerabilitySeconds > 0f)
                StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnerabilitySeconds);
        invulnerable = false;
    }

    private void Die()
    {
        OnDied?.Invoke();

        // “Morir” básico: desactivar al player.
        // Puedes cambiarlo por animación, ragdoll, respawn, etc.
        gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        CurrentHealth = Mathf.Max(1, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        invulnerable = false;
        gameObject.SetActive(true);
    }
}
