using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Spawner_GFX : MonoBehaviour
{
    public AIPath aiPath;

    // Update is called once per frame
    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1.8f, 1.8f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1.8f, 1.8f, 1f);
        }
    }
}
