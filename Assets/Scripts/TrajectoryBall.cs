using UnityEngine;

public class TrajectoryBall : MonoBehaviour
{
    [SerializeField] GameObject directionArrow;

    bool isMouseOver = false;
    Rigidbody thrownObject;


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
        directionArrow.SetActive(true);
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        directionArrow.SetActive(false);
    }

    public void SetThrownObject(Rigidbody thrownObject)
    {
        this.thrownObject = thrownObject;
        SetArrowDirection();
    }

    private void SetArrowDirection()
    {
        if (thrownObject != null)
        {
            Vector3 velocity = thrownObject.velocity;
            Vector3 direction = velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            directionArrow.transform.rotation = targetRotation;
        }
    }
}
