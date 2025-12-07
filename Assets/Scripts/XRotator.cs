using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRotator : MonoBehaviour
{
    [SerializeField] private Transform[] objectsToRotate;
    [SerializeField] private float rotationSpeed = 50f;

    void Update()
    {
        foreach (Transform obj in objectsToRotate)
        {
            obj.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
            obj.Rotate(Vector3.up * Time.deltaTime * rotationSpeed / 2);            
        }
    }
}
