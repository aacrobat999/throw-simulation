using UnityEngine;

public class TrajectoryBall : MonoBehaviour
{
    [SerializeField] GameObject directionArrow;

    bool isMouseOver = false;

    public void Update()
    {
        /*if (isMouseOver)
            Debug.Log(transform.forward);
        */
        if (TrajectoryPredictor.i.displayAllArrows)
            directionArrow.SetActive(true);
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

}
