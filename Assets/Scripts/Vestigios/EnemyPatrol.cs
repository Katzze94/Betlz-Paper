using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public enum PatrolAxis { Vertical, Horizontal }

    [Header("Patrol Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool startFromPointB = false;
    [SerializeField] private PatrolAxis patrolAxis = PatrolAxis.Vertical;

    private Transform target;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Coroutine patrolRoutine;

    

    void OnEnable(){
        PlayerStates.OnDeath += HandlePlayerDeath;

        if (patrolRoutine != null) StopCoroutine(patrolRoutine);

        if (startFromPointB)
        {
            startPosition = pointB.position;
            target = pointA;
        }
        else
        {
            startPosition = pointA.position;
            target = pointB;
        }

        startRotation = transform.rotation;
        transform.position = startPosition;

        patrolRoutine = StartCoroutine(PatrolRoutine());
    }

    void OnDisable(){
        PlayerStates.OnDeath -= HandlePlayerDeath;
        if (patrolRoutine != null) StopCoroutine(patrolRoutine);
    }


    private void HandlePlayerDeath(int screen, bool death)
    {
        if (!death) return;

        if (patrolRoutine != null) StopCoroutine(patrolRoutine);
        transform.position = startPosition;
        transform.rotation = startRotation;
        target = startFromPointB ? pointA : pointB;
        patrolRoutine = StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            while (Vector3.Distance(transform.position, target.position) > 0.05f)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                float step = speed * Time.deltaTime;
                Vector3 nextPosition = transform.position + direction * step;

                // Si el siguiente paso va a pasar el objetivo, ajusta la posición directamente
                if (Vector3.Distance(nextPosition, target.position) < step)
                {
                    transform.position = target.position;
                    break;
                }

                transform.position = nextPosition;

                // Rotación solo en eje relevante
                Vector3 lookDir = direction;
                if (patrolAxis == PatrolAxis.Horizontal)
                    lookDir = new Vector3(direction.x, 0, 0);
                else if (patrolAxis == PatrolAxis.Vertical)
                    lookDir = new Vector3(0, direction.y, 0);

                if (lookDir != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(lookDir, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                }

                yield return null;
            }

            // Asegúrate de que el enemigo esté exactamente en el objetivo antes de esperar
            transform.position = target.position;
            yield return new WaitForSeconds(waitTime);
            
            // Cambia al siguiente objetivo
            target = target == pointA ? pointB : pointA;
        }
    }
}
