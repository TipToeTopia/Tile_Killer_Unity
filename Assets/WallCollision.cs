using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    private const string WALLS_TAG = "Walls";

    private void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.tag == WALLS_TAG)
        {
            Destroy(gameObject);
        }
    }
}
