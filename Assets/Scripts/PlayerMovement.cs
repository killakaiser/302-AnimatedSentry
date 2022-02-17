using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform boneLegLeft;
    public Transform boneLegRight;

    public float walkSpeed = 5;

    public Camera cam;

    CharacterController pawn;

    private Vector3 inputDir;

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

            while (camYaw > playerYaw + 180) camYaw -= 360;
            while (camYaw < playerYaw - 180) camYaw += 360;

            Quaternion playerRotation = Quaternion.Euler(0, playerYaw, 0);
            Quaternion targetRotation = Quaternion.Euler(0, camYaw, 0);


            transform.rotation = AnimMath.Ease(playerRotation, targetRotation, .01f);
        }
        Vector3 inputDir = transform.forward * v + transform.right * h;
        if (inputDir.sqrMagnitude > 1) inputDir.Normalize();

        pawn.SimpleMove(inputDir * walkSpeed);

        WalkAnimation();
    }

    void WalkAnimation()
    {



        Vector3 inputDirLocal = transform.InverseTransformDirection(inputDir);
        Vector3 axis = Vector3.Cross(Vector3.up, inputDirLocal);

        float alignment = Vector3.Dot(inputDirLocal, Vector3.forward);

        alignment = Mathf.Abs(alignment);

        float degrees = AnimMath.Lerp(10, 40, alignment);
        float speed = 10;
        float wave = Mathf.Sin(Time.time * speed) * degrees;

        boneLegLeft.localRotation = Quaternion.AngleAxis(wave, axis);
        boneLegRight.localRotation = Quaternion.AngleAxis(-wave, axis);


    }




}
