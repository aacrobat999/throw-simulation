using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    void Start()
    {
        camera2.gameObject.SetActive(false);
    }

    public void StartTransitionTo()
    {
        camera2.gameObject.SetActive(true);
        camera1.gameObject.SetActive(false);
    }

    public void StartTransitionBack()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }
}
