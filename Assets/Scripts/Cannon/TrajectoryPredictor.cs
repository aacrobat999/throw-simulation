using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{
    [SerializeField, Tooltip("The marker will show where the projectile will hit")]
    Transform hitMarker;

    [SerializeField, Tooltip("The trajectory ball prefab")]
    GameObject trajectoryBallPrefab;

    [SerializeField, Range(10, 100), Tooltip("The maximum number of points the LineRenderer can have")]
    public int maxPoints = 50;

    [SerializeField, Range(0.01f, 0.5f), Tooltip("The time increment used to calculate the trajectory")]
    float increment = 0.025f;

    [SerializeField, Range(1.05f, 2f), Tooltip("The raycast overlap between points in the trajectory, this is a multiplier of the length between points. 2 = twice as long")]
    float rayOverlap = 1.1f;

    [SerializeField, Range(1.05f, 2f), Tooltip("Simulation of air friction")]
    float airFriction = 0.99f;

    [SerializeField, Tooltip("To display all arrows check it as true")]
    public bool displayAllArrows = false;

    [SerializeField] TextMeshProUGUI highestPointText;
    [SerializeField] TextMeshProUGUI totalDistanceText;
    [SerializeField] TextMeshProUGUI horizontalDistanceText;
    [SerializeField] public TextMeshProUGUI currentVelocityText;
    [SerializeField] TextMeshProUGUI maxVelocityText;
    [SerializeField] TextMeshProUGUI impactVelocityText;

    float highestPoint;
    float totalDistance;
    float horizontalDistance;
    float currentVelocity;
    float maxVelocity;
    float impactVelocity;

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

        highestPoint = position.y;
        totalDistance = 0f;
        horizontalDistance = 0f;
        maxVelocity = 0f;
        impactVelocity = 0f;

        for (int i = 0; i < maxPoints; i++)
        {
            velocity = CalculateNewVelocity(velocity, projectile.drag, increment);
            Vector3 nextPosition = position + velocity * increment;

            currentVelocity = velocity.magnitude;

            if (currentVelocity > maxVelocity)
            {
                maxVelocity = currentVelocity;
            }

            if (nextPosition.y > highestPoint)
            {
                highestPoint = nextPosition.y;
            }

            totalDistance += Vector3.Distance(position, nextPosition);
            horizontalDistance += Mathf.Abs(nextPosition.x - position.x);
            position = nextPosition;

            if (i % 2 == 0)
            {
                Ray ray = new Ray(position, Vector3.down);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.distance > 0)
                    {
                        GameObject ball = Instantiate(trajectoryBallPrefab, position, Quaternion.identity);
                        trajectoryBalls.Add(ball);

                        TrajectoryBall trajectoryBall = ball.GetComponent<TrajectoryBall>();
                        if (trajectoryBall != null)
                        {
                            trajectoryBall.SetVelocity(currentVelocity);
                        }

                        Vector3 currentDirection = CalculateNewVelocity(velocity, projectile.drag, increment).normalized;
                        ball.transform.LookAt(ball.transform.position + currentDirection);
                    }
                }
            }

            trajectoryLine.SetPosition(i, position);
        }

        impactVelocity = currentVelocity;

        UpdateUI();
    }

    public void PredictTrajectory(ProjectileProperties projectile)
    {
        Vector3 velocity = projectile.direction * (projectile.initialSpeed / projectile.mass);
        Vector3 position = projectile.initialPosition;
        Vector3 nextPosition;
        float overlap;

        highestPoint = position.y;
        totalDistance = 0f;
        horizontalDistance = 0f;
        maxVelocity = 0f;

        UpdateLineRender(maxPoints, (0, position));

        for (int i = 1; i < maxPoints; i++)
        {
            velocity = CalculateNewVelocity(velocity, projectile.drag, increment);
            nextPosition = position + velocity * increment;

            currentVelocity = velocity.magnitude;

            if (currentVelocity > maxVelocity)
            {
                maxVelocity = currentVelocity;
            }

            if (nextPosition.y > highestPoint)
            {
                highestPoint = nextPosition.y;
            }

            totalDistance += CalculateDistance(position, nextPosition);
            horizontalDistance += Mathf.Abs(nextPosition.x - position.x);

            overlap = CalculateDistance(position, nextPosition) * rayOverlap;

            int layerMask = ~(1 << LayerMask.NameToLayer("TrajectoryBall"));

            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap, layerMask))
            {
                UpdateLineRender(i, (i - 1, hit.point));
                MoveHitMarker(hit);
                impactVelocity = currentVelocity;
                break;
            }

            hitMarker.gameObject.SetActive(false);
            position = nextPosition;
            UpdateLineRender(maxPoints, (i, position));
        }

        impactVelocity = currentVelocity;

        UpdateUI();
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


    public float CalculateDistance(Vector3 a, Vector3 b)
    {
        float num = a.x - b.x;
        float num2 = a.y - b.y;
        float num3 = a.z - b.z;
        return Mathf.Sqrt(num * num + num2 * num2 + num3 * num3);
    }

    #region UI
    void UpdateUI()
    {
        highestPointText.text = highestPoint.ToString("F2");
        totalDistanceText.text = totalDistance.ToString("F2");
        horizontalDistanceText.text = horizontalDistance.ToString("F2");
        currentVelocityText.text = currentVelocity.ToString("F2");
        maxVelocityText.text = maxVelocity.ToString("F2");
        impactVelocityText.text = impactVelocity.ToString("F2");
    }

    public void DisplayAllArrows(Toggle newValue)
    {
        displayAllArrows = newValue.isOn;
    }

    public void ChangeTrailPoints(TMP_InputField input)
    {
        if (string.IsNullOrEmpty(input.text))
            return;

        if (int.TryParse(input.text, out int newMaxPoints))
        {
            if (newMaxPoints <= 1)
            {
                maxPoints = 1;
                input.text = maxPoints.ToString();
            }
            else if (newMaxPoints >= 150)
            {
                maxPoints = 150;
                input.text = maxPoints.ToString();
            }
            else
            {
                maxPoints = newMaxPoints;
            }
        }
    }

    #endregion
}