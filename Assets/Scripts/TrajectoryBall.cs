using UnityEngine;

public class TrajectoryBall : MonoBehaviour
{
    private bool isMouseOver = false;
    [SerializeField] GameObject directionArrow; 

    void Update()
    {
        if (isMouseOver)
        {
            Debug.Log("Current Direction: " + transform.forward);
        }
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;

        directionArrow = Instantiate(directionArrow, transform.position, Quaternion.identity, transform);
        UpdateArrowDirection(transform.forward);
    }

    private void OnMouseExit()
    {
        isMouseOver = false;

        if (directionArrow != null)
        {
            Destroy(directionArrow);
        }
    }

    private void UpdateArrowDirection(Vector3 direction)
    {
        if (directionArrow != null)
        {
            directionArrow.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
