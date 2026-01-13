using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement & Jump Configuration")]
    [SerializeField] float speed = 6;
    [SerializeField] float jumpForce = 6;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isFacingRight;
    [SerializeField] Transform groundCheck; //Ref a la posición del detector de suelo
    [SerializeField] float groundCheckRadius; //Radio del detector de suelo
    [SerializeField] LayerMask groundLayer; //Ref a la capa que puede tocar el detector de suelo

    //Variables de referencia interna
    Rigidbody2D playerRb;
    Animator anim;
    PlayerInput input;
    Vector2 moveInput;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        isFacingRight = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        AnimationManagement();
        if (moveInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        if (moveInput.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        playerRb.linearVelocity = new Vector2(moveInput.x * speed, playerRb.linearVelocity.y);
    }

    void Flip()
    {
        Vector3 actualScale = transform.localScale;
        actualScale.x *= -1;
        transform.localScale = actualScale;
        isFacingRight = !isFacingRight;
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    void AnimationManagement()
    {
        //Lógica del cambio de animación de salto
        anim.SetBool("Jumping", !isGrounded); //Que tiene el valor contrario a isGrounded
        //Lógica de cambio entre idle-run
        if (moveInput.x != 0) anim.SetBool("Running", true);
        else anim.SetBool("Running", false);
    }




    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded) Jump();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded) anim.SetTrigger("Attack");
    }

    #endregion

}
