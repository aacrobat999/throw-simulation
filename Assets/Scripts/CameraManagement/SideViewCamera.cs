using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideViewCamera : MonoBehaviour
{
    [SerializeField] ProjectileThrow projectileThrow;
    [SerializeField] TrajectoryPredictor trajectoryPredictor;

    [SerializeField] float heightAboveTrajectory = 10f;
    [SerializeField] float distanceFromTrajectory = 20f;
    [SerializeField] float offsetFromTrajectory = 30f;
    [SerializeField] float minCameraHeight = 10f;

    void Update()
    {
        if (projectileThrow != null && trajectoryPredictor != null)
        {
            List<Vector3> trajectoryBallPositions = trajectoryPredictor.GetTrajectoryBallPositions();

            if (trajectoryBallPositions.Count >= 2)
            {
                Vector3 trajectoryDirection = (trajectoryBallPositions[1] - trajectoryBallPositions[0]).normalized;

                Vector3 averagePosition = Vector3.zero;
                foreach (Vector3 position in trajectoryBallPositions)
                {
                    averagePosition += position;
                }
                averagePosition /= trajectoryBallPositions.Count;

                Vector3 perpendicularDirection = Vector3.Cross(trajectoryDirection, Vector3.up);

                Vector3 targetPosition = averagePosition - perpendicularDirection * offsetFromTrajectory;
                targetPosition.y = Mathf.Max(averagePosition.y + heightAboveTrajectory, minCameraHeight);

                transform.position = targetPosition;

                transform.LookAt(averagePosition);
            }
        }
    }
}
