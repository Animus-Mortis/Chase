using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] Vector3 vector3;

    Animator animator;
    CharacterController controller;
    Vector3 moveDirection;
    float speed = 4f;
    float jumpPower = 7;
    float speedRotation = 0.2f;
    float gravityForce;
    bool die;
    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (!die)
        {
            Animation();
            Move();
        }
        Gravity();
    }
    private void Animation()
    {
        if (joystick.Horizontal() == 0 && joystick.Vertical() == 0)
        {
            animator.SetBool("Walk Forward", false);
            animator.SetBool("Run Forward", false);
        }
        else if ((joystick.Horizontal() >= 0.5f || joystick.Horizontal() <= -0.5f) || (joystick.Vertical() >= 0.5f || joystick.Vertical() <= -0.5f))
        {
            animator.SetBool("Run Forward", true);
            animator.SetBool("Walk Forward", false);
        }
        else
        {
            animator.SetBool("Walk Forward", true);
            animator.SetBool("Run Forward", false);
        }
    }
    private void Move()
    {
        moveDirection = new Vector3(joystick.Horizontal(), 0, joystick.Vertical());
        moveDirection *= speed;

        if (Vector3.Angle(Vector3.forward, moveDirection) > 1f || Vector3.Angle(Vector3.forward, moveDirection) == 0)
        {
            Vector3 direct = Vector3.RotateTowards(transform.forward, moveDirection, speedRotation, 0.0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }
        moveDirection.y = gravityForce;
        controller.Move(moveDirection * Time.deltaTime);
    }
    private void Gravity()
    {
        if (!controller.isGrounded)
        {
            gravityForce -= 20f * Time.deltaTime;
            return;
        }
        if (joystick.setLoose)
        {
            animator.SetBool("Jump", true);
            gravityForce = jumpPower;
            joystick.setLoose = false;
            return;
        }
        animator.SetBool("Jump", false);
        gravityForce = -1f;
    }

    public void Die()
    {
        die = true;
        animator.SetTrigger("Die");
        animator.SetBool("Walk Forward", false);
        animator.SetBool("Run Forward", false);
        animator.SetBool("Jump", false);
    }
}
