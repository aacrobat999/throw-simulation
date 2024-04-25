using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    public static CameraManager i;
    public int currentCamera;

    private void Awake() => i = this;
    private void Start() => camera2.gameObject.SetActive(false);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            StartTransitionTo();

        if (Input.GetKeyDown(KeyCode.L))
            StartTransitionBack();
    }

    public void StartTransitionTo()
    {
        camera2.gameObject.SetActive(true);
        camera1.gameObject.SetActive(false);

        currentCamera = 2;
    }

    public void StartTransitionBack()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);

        currentCamera = 1;
    }
}
