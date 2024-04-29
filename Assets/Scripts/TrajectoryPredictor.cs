using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{
    [SerializeField, Tooltip("The marker will show where the projectile will hit")]
    Transform hitMarker;

    [SerializeField, Tooltip("The trajectory ball prefab")]
    GameObject trajectoryBallPrefab;

    [SerializeField, Range(10, 100), Tooltip("The maximum number of points the LineRenderer can have")]
    int maxPoints = 50;

    [SerializeField, Range(0.01f, 0.5f), Tooltip("The time increment used to calculate the trajectory")]
    float increment = 0.025f;

    [SerializeField, Range(1.05f, 2f), Tooltip("The raycast overlap between points in the trajectory, this is a multiplier of the length between points. 2 = twice as long")]
    float rayOverlap = 1.1f;

    [SerializeField, Tooltip("To display all arrows check it as true")]
    public bool displayAllArrows = false;

    public static TrajectoryPredictor i { get; private set; }

    LineRenderer trajectoryLine;
    List<GameObject> trajectoryBalls = new List<GameObject>();


    private void Awake() => i = this;

    private void Start()
    {
        if (trajectoryLine == null)
            trajectoryLine = GetComponent<LineRenderer>();

        SetTrajectoryVisible(true);
    }

    public void SpawnTrajectoryBalls(ProjectileProperties projectile)
    {
        ClearTrajectoryBalls();

        trajectoryLine.positionCount = maxPoints;
        Vector3 velocity = projectile.direction * (projectile.initialSpeed / projectile.mass);
        Vector3 position = projectile.initialPosition;

        for (int i = 0; i < maxPoints; i++)
        {
            velocity = CalculateNewVelocity(velocity, projectile.drag, increment);
            position += velocity * increment;

            if (i % 2 == 0)
            {
                GameObject ball = Instantiate(trajectoryBallPrefab, position, Quaternion.identity);
                trajectoryBalls.Add(ball);

                Vector3 currentDirection = CalculateNewVelocity(velocity, projectile.drag, increment).normalized;

                ball.transform.LookAt(ball.transform.position + currentDirection);
            }

            trajectoryLine.SetPosition(i, position);
        }
    }

    public void PredictTrajectory(ProjectileProperties projectile)
    {
        Vector3 velocity = projectile.direction * (projectile.initialSpeed / projectile.mass);
        Vector3 position = projectile.initialPosition;
        Vector3 nextPosition;
        float overlap;

        UpdateLineRender(maxPoints, (0, position));

        for (int i = 1; i < maxPoints; i++)
        {
            velocity = CalculateNewVelocity(velocity, projectile.drag, increment);
            nextPosition = position + velocity * increment;

            overlap = Vector3.Distance(position, nextPosition) * rayOverlap;

            int layerMask = ~(1 << LayerMask.NameToLayer("TrajectoryBall"));

            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap, layerMask))
            {
                UpdateLineRender(i, (i - 1, hit.point));
                MoveHitMarker(hit);
                break;
            }

            hitMarker.gameObject.SetActive(false);
            position = nextPosition;
            UpdateLineRender(maxPoints, (i, position));
        }
    }
    private void ClearTrajectoryBalls()
    {
        foreach (GameObject ball in trajectoryBalls)
        {
            Destroy(ball);
        }
        trajectoryBalls.Clear();
    }

    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos)
    {
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointPos.point, pointPos.pos);
    }

    private Vector3 CalculateNewVelocity(Vector3 velocity, float drag, float increment)
    {
        velocity += Physics.gravity * increment;
        velocity *= Mathf.Clamp01(1f - drag * increment);
        return velocity;
    }

    private void MoveHitMarker(RaycastHit hit)
    {
        hitMarker.gameObject.SetActive(true);

        float offset = 0.025f;
        hitMarker.position = hit.point + hit.normal * offset;
        hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    }

    public void SetTrajectoryVisible(bool visible)
    {
        trajectoryLine.enabled = visible;
        hitMarker.gameObject.SetActive(visible);
    }

    public List<Vector3> GetTrajectoryBallPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (GameObject ball in trajectoryBalls)
        {
            positions.Add(ball.transform.position);
        }
        return positions;
    }
}