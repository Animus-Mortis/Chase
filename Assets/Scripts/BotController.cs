using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    [Range(0, 360)] [SerializeField] float viewAngle = 60f;
    [SerializeField] float viewDistance = 15f;
    [SerializeField] Transform enemyEye;
    [SerializeField] Transform player;
    [SerializeField] Transform[] positions;

    NavMeshAgent agent;
    Transform nowPlace;
    Transform move;
    Animator animator;
    bool sawPlayer;
    bool gameOver;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetNewPath();
    }
    private void Update()
    {
        if(Vector3.Distance(player.position, agent.transform.position) < 1 && !gameOver)
        {
            GameController.instance.GameOver();
            gameOver = true;
            return;
        }
        if (IsInView())
        {
            sawPlayer = true;
        }
        if (sawPlayer)
        {
            agent.SetDestination(player.position);
            animator.SetBool("Walk Forward", false);
            animator.SetBool("Run Forward", true);
            agent.speed = 3.5f;
        }
        DrawViewState();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dots"))
        {
            SetNewPath();
        }
    }
    void SetNewPath()
    {
        animator.SetBool("Walk Forward", true);
        agent.speed = 2.5f;
        move = positions[Random.Range(0, positions.Length)];
        if (nowPlace != null && nowPlace.position == move.position)
        {
            SetNewPath();
            return;
        }
        nowPlace = move;
        agent.SetDestination(move.position);
    }
    private bool IsInView()
    {
        float realAngle = Vector3.Angle(enemyEye.forward, player.position - enemyEye.position);
        RaycastHit hit;
        if (Physics.Raycast(enemyEye.position, player.position - enemyEye.position, out hit, viewDistance))
        {
            if (realAngle < viewAngle / 2 && Vector3.Distance(enemyEye.position, player.position) <= viewDistance && hit.transform == player.transform)
            {
                return true;
            }
        }
        return false;
    }
    void DrawViewState()
    {
        Vector3 left = enemyEye.position + Quaternion.Euler(new Vector3(0, viewAngle / 2, 0)) * (enemyEye.forward * viewDistance);
        Vector3 right = enemyEye.position + Quaternion.Euler(-new Vector3(0, viewAngle / 2, 0)) * (enemyEye.forward * viewDistance);
        Debug.DrawLine(enemyEye.position, left, Color.red);
        Debug.DrawLine(enemyEye.position, right, Color.red);
    }
}
