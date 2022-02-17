using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{

    public float visionDistance = 10;

    [Range(1, 20)]
    public int roundsPerSecond = 5;

    public Transform boneShoulderRight;
    public Transform boneShoulderLeft;


    public TargetableObject target { get; private set; }

    public bool playerWantsToAim { get; private set; }
    public bool playerWantsToAttack { get; private set; }

    private List<TargetableObject> validTargets = new List<TargetableObject>();
    private float cooldownScan = 0;
    private float cooldownPickTarget = 0;
    private float cooldownAttack = 0;

    void Update()
    {
        playerWantsToAim = Input.GetButton("Fire2");

        cooldownScan -= Time.deltaTime;
        cooldownPickTarget -= Time.deltaTime;
        cooldownAttack -= Time.deltaTime;

        if (playerWantsToAim)
        {
            if(target != null)
            {
                if (!CanSeeThing(target))
                {
                    target = null;
                }
            }

            if (cooldownScan <= 0) ScanForTargets();
            if (cooldownPickTarget <= 0) PickAtTarget();
        }
        else
        {
            target = null;
        }
        DoAttack();
    }

    void DoAttack() {
        if (cooldownAttack > 0) return;
        if (!playerWantsToAim) return;
        if (!playerWantsToAttack) return;
        if (target = null) return;
        if (!CanSeeThing(target)) return;

        cooldownAttack = 1f / roundsPerSecond;

        // spawn projectiles...
        // or take health away form target 

        boneShoulderLeft.localEulerAngles += new Vector3(-30, 0, 0);
        boneShoulderRight.localEulerAngles += new Vector3(-30, 0, 0);

    }

    void ScanForTargets()
    {
        cooldownScan = .5f;

        validtargets.Clear();

        TargetableObject[] things = GameObject.FindObjectOfType<TargetableObject>();
        foreach(TargetableObject thing in things)
        {
            if(CanSeeThing(thing))
            {
                validTargets.Add(thing);
            }
        }
    }

    private bool CanSeeThing(TargetableObject thing)
    {
        Vector3 vToThing = thing.transform.position - transform.position;

        //is too far to see
        if (vToThing.sqrMagnitude > visionDistance * visionDistance) return false;

        //how much in front of player?
        float alignment = Vector3.Dot(transform.forward, vToThing.normalized);

        //is whithin so many degrees of forwrds direction?
        if (alignment < .4f) return false;

        return true;
    }
    void PickAtTarget()
    {



    }
}
