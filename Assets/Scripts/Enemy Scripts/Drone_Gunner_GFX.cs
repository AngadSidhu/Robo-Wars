using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Gunner_GFX : MonoBehaviour
{

    public AIPath aiPath;

    // Update is called once per frame
    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1.3617f, -1.5351f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1.3617f, 1.5351f, 1f);
        }
    }
}
