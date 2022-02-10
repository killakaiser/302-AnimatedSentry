using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed = 5;

    public Camera cam;

    CharacterController pawn;

    void Start()
    {
        pawn = GetComponent<CharacterController>();
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        bool playerWantsToMove = (v != 0 || h != 0); 

        if (cam && playerWantsToMove)
        {//turn the player to match the camera

            float playerYaw = transform.eulerAngles.y;
            float camYaw = cam.transform.eulerAngles.y;

           // while (camYaw > playerYaw + 180) camYaw -= 360;
           // while (camYaw < playerYaw - 180) camYaw += 360;


            Quaternion targetRotation = Quaternion.Euler(0, camYaw, 0);
            transform.rotation = AnimMath.Ease(transform.rotation, targetRotation, .01f);
        }
        Vector3 moveDir = transform.forward * v + transform.right * h;
        if (moveDir.sqrMagnitude > 1) moveDir.Normalize();


        pawn.SimpleMove(moveDir * walkSpeed);

    }
}
