using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(NavMeshAgent))]
public class Monsterai : MonoBehaviour
{
    public enum State { Patrol, Chase, Grabbing, Stunned }

    [Header("Referencias")]
    public Transform player;
    public Transform[] patrolPoints;
    public Quicktimeevent qte;

    [Header("Velocidades")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4.5f;

    [Header("Detección")]
    public float sightRange = 15f;
    public float hearRange = 5f;
    [Range(0, 180)] public float fieldOfView = 100f;
    public float loseSightTime = 4f;
    public LayerMask obstacleMask;

    [Header("Ataque / Aturdimiento")]
    public float grabDistance = 1.6f;
    public float stunDuration = 6f;

    public State current = State.Patrol;

    NavMeshAgent agent;
    int patrolIndex;
    float timeSinceSeen;
    float stunTimer;

    void Awake() => agent = GetComponent<NavMeshAgent>();

    void Start()
    {
        agent.speed = patrolSpeed;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        switch (current)
        {
            case State.Patrol: UpdatePatrol(); break;
            case State.Chase: UpdateChase(); break;
            case State.Stunned: UpdateStunned(); break;
        }
    }

    // ---------- PATRULLA ----------
    void UpdatePatrol()
    {
        if (CanDetectPlayer()) { StartChase(); return; }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextPatrolPoint();
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    // ---------- PERSECUCIÓN ----------
    void StartChase()
    {
        current = State.Chase;
        agent.speed = chaseSpeed;
        timeSinceSeen = 0f;
    }

    void UpdateChase()
    {
        // Si el jugador está en una zona de luz, el monstruo se detiene y retrocede
        if (Lightzone.PlayerIsSafe)
        {
            Stun(stunDuration * 0.5f);
            return;
        }

        agent.SetDestination(player.position);

        if (CanDetectPlayer()) timeSinceSeen = 0f;
        else timeSinceSeen += Time.deltaTime;

        if (timeSinceSeen > loseSightTime)
        {
            current = State.Patrol;
            agent.speed = patrolSpeed;
            GoToNextPatrolPoint();
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= grabDistance)
            StartGrab();
    }

    // ---------- AGARRAR + QTE ----------
    void StartGrab()
    {
        current = State.Grabbing;
        agent.isStopped = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        qte.Begin(OnQTEResult);
    }

    void OnQTEResult(bool survived)
    {
        if (survived) Stun(stunDuration);
        else Gamemanager.PlayerDied(); // <-- conecta aquí tu pantalla de Game Over
    }

    // ---------- ATURDIDO ----------
    void Stun(float duration)
    {
        current = State.Stunned;
        stunTimer = duration;
        agent.isStopped = false;
        agent.speed = chaseSpeed;

        // Huye alejándose del jugador
        Vector3 away = (transform.position - player.position).normalized * 15f;
        if (NavMesh.SamplePosition(transform.position + away, out var hit, 10f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }

    void UpdateStunned()
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            current = State.Patrol;
            agent.speed = patrolSpeed;
            GoToNextPatrolPoint();
        }
    }

    // ---------- DETECCIÓN ----------
    bool CanDetectPlayer()
    {
        if (Lightzone.PlayerIsSafe) return false;

        Vector3 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;

        // Oído: lo detecta cerca aunque esté a su espalda
        if (dist <= hearRange) return true;

        if (dist > sightRange) return false;
        if (Vector3.Angle(transform.forward, toPlayer) > fieldOfView * 0.5f) return false;

        // Línea de visión
        Vector3 eye = transform.position + Vector3.up * 1.6f;
        Vector3 target = player.position + Vector3.up * 1.0f;
        return !Physics.Linecast(eye, target, obstacleMask);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, hearRange);
    }
}
