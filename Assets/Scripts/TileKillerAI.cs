using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class TileKillerAI : MonoBehaviour
{
    [SerializeField]
    public NavMeshAgent navmeshAgent;

    [SerializeField]
    private float patrolRange = 3.0f;

    private float patrolTimer = 0.0f;

    [SerializeField]
    float waitTimer = 2.0f;

    private Transform targetInstance;

    private List<GameObject> tilesInGame;

    private float targetRange = 2.0f;

    private const float COROUTINE_DELAY = 0.5f;

    private const int AREA_MASK = -1;

    [SerializeField]
    private AudioClip tileKillerOnlineSound;

    private void Start()
    {
        AudioManager.Instance.PlaySound(tileKillerOnlineSound);
    }

    void Update()
    {
        // entire logic is based around finding nearest tile to destroy, else AI will randomly patrol

        StartCoroutine(FindClosestTarget());

        if (targetInstance == null)
        {
            return;
        }
        
        if (Vector3.Distance(this.transform.position, targetInstance.transform.position) < targetRange)
        {
            navmeshAgent.SetDestination(targetInstance.transform.position);

            if (navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
            {
                GameManager.Instance.UpdateScore();

                GameManager.Instance.tileList.Remove(targetInstance.gameObject);
                Destroy(targetInstance.gameObject);
            }
        }
        else
        {
            if (navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
            {
                patrolTimer += Time.deltaTime; // timer begins

                if (patrolTimer >= waitTimer)
                {
                    Vector3 point = RandomPoint();

                    navmeshAgent.SetDestination(point);

                    patrolTimer = 0.0f; // reset timer after each destination
                }
            }
            else
            {
                patrolTimer = 0.0f;
            }
        }
    }
  
    // obtain random point around AI origin

    public Vector3 RandomPoint()
    {
        Vector3 RandomPoint = (Random.onUnitSphere * patrolRange) + this.transform.position;
        NavMeshHit Hit;
        NavMesh.SamplePosition(RandomPoint, out Hit, patrolRange, AREA_MASK);
        return new Vector3(Hit.position.x, this.transform.position.y, Hit.position.z);
    }

    // find the closest Tile to destroy

    IEnumerator FindClosestTarget()
    {
        yield return new WaitForSeconds(COROUTINE_DELAY);

        tilesInGame = GameManager.Instance.tileList;

        Transform ClosestTarget = null;
        float MaximumDistance = Mathf.Infinity;

        for (int I = 0; I < tilesInGame.Count; I++)
        {
            if (tilesInGame[I] != null)
            {
                float TargetDistance = Vector3.Distance(this.transform.position, tilesInGame[I].transform.position);

                if (TargetDistance < MaximumDistance)
                {
                    ClosestTarget = tilesInGame[I].transform;
                    MaximumDistance = TargetDistance;
                }
            }
        }

        targetInstance = ClosestTarget;
    }
}
