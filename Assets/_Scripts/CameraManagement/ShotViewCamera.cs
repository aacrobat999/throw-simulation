using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotViewCamera : MonoBehaviour
{
    [SerializeField] ProjectileThrow projectileThrow;
    [SerializeField] float distance = 20f;
    [SerializeField] float height = 10f;

    Transform cannonball;

    private void Update()
    {
        cannonball = projectileThrow.thrownObject.transform;

        if (cannonball != null)
        {
            Vector3 targetPosition = cannonball.position - cannonball.forward * distance + Vector3.up * height;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);

            transform.LookAt(cannonball.position);
        }
    }
}