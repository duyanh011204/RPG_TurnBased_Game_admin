using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayer3D : MonoBehaviour
{
    void Start()
    {
        // Thân (Body)
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.parent = transform;
        body.transform.localPosition = new Vector3(0, 1, 0);
        body.transform.localScale = new Vector3(1f, 1.5f, 0.5f);

        // Đầu (Head)
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        head.transform.parent = transform;
        head.transform.localPosition = new Vector3(0, 2.25f, 0);
        head.transform.localScale = new Vector3(1f, 1f, 1f);

        // Tay trái (Left Arm)
        GameObject leftArm = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftArm.transform.parent = transform;
        leftArm.transform.localPosition = new Vector3(-0.75f, 1.25f, 0);
        leftArm.transform.localScale = new Vector3(0.5f, 1.25f, 0.5f);

        // Tay phải (Right Arm)
        GameObject rightArm = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightArm.transform.parent = transform;
        rightArm.transform.localPosition = new Vector3(0.75f, 1.25f, 0);
        rightArm.transform.localScale = new Vector3(0.5f, 1.25f, 0.5f);

        // Chân trái (Left Leg)
        GameObject leftLeg = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftLeg.transform.parent = transform;
        leftLeg.transform.localPosition = new Vector3(-0.3f, 0.25f, 0);
        leftLeg.transform.localScale = new Vector3(0.5f, 1f, 0.5f);

        // Chân phải (Right Leg)
        GameObject rightLeg = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightLeg.transform.parent = transform;
        rightLeg.transform.localPosition = new Vector3(0.3f, 0.25f, 0);
        rightLeg.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
    }
}