using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    [SerializeField]
    public NavMeshAgent navmeshAgent;

    private Transform playerTarget;

    private const float MINIMUM_DISTANCE_TO_PLAYER = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // set minion AI destination to play immediately 

        navmeshAgent.SetDestination(playerTarget.transform.position);

        if (Vector3.Distance(this.transform.position, playerTarget.transform.position) < MINIMUM_DISTANCE_TO_PLAYER)
        {
            if (playerTarget != null)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.InstantDeath();
                }
            }
        }
    }
}
