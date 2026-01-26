using UnityEngine;

// =========================
// ENEMY AI + ATTACK
// =========================
public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 2f;
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float attackRange = 1.2f;

    [Header("Attack")]
    [SerializeField] int damage = 1;
    [SerializeField] float attackCooldown = 1.5f;

    [Header("Collision")]
    [SerializeField] LayerMask obstacleLayer = 1 << 6; // Layer 6 = Ground
    [SerializeField] float wallCheckDistance = 0.5f;

    Rigidbody2D rb;
    Animator anim;
    Transform player;

    float lastAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
            if (dist > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            Patrol();
        }
    }

    void ChasePlayer()
    {
        anim.SetBool("Walking", true);

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);

        if (dir.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void Patrol()
    {
        anim.SetBool("Walking", true);

        Vector2 origin = transform.position;
        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, wallCheckDistance, obstacleLayer);

        if (hit.collider != null)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            dir *= -1;
        }

        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);
    }

    void AttackPlayer()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("Attack");

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
    }
}
