using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAt : MonoBehaviour
{
    public bool lockAxisX = false;
    public bool lockAxisY = false;
    public bool lockAxisZ = false;


    private Quaternion startRotation;
    private Quaternion goalRotation;

    private PlayerTargeting playerTargeting;

    void Start()
    {
        playerTargeting = GetComponentInParent<PlayerTargeting>();
        startRotation = transform.localRotation;
    }

    void Update()
    {
        TurnTowardsTarget();
    }


    private void TurnTowardsTarget()
    {
        if(playerTargeting && playerTargeting.target && playerTargeting.playerWantsToAim)
        {
            Vector3 vToTarget = playerTargeting.target.transform.position - transform.position;

            Quaternion worldRot = Quaternion.LookRotation(vToTarget);

            Quaternion localRot = worldRot;

            if (transform.parent)
            {
                localRot = Quaternion.Inverse(transform.parent.rotation) * worldRot;
            }

            Vector3 euler = localRot.eulerAngles;
            if (lockAxisX) euler.x = 0;
            if (lockAxisY) euler.y = 0;
            if (lockAxisZ) euler.z = 0;

            localRot.eulerAngles = euler;

            goalRotation = localRot;

        } else
        {
            goalRotation = startRotation;
        }
        transform.localRotation = AnimMath.Ease(transform.localRotation, goalRotation, .01f);
    }
}
