// ===================
// Bullet Behavior
// ===================
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // mata al enemigo
            Destroy(gameObject); // destruye la bala
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject); // destruye la bala al chocar con muro/suelo
        }
    }
}