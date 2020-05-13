using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{

    public float cameraSpeed = 100f;
    public GameObject v_rotate;
    public GameObject h_rotate;
    public Camera firstCamera;
    public Camera thirdCamera;

    private PlayerInput pi;
    void Start()
    {
        pi = GetComponent<PlayerInput>();
        firstCamera.enabled = false;
        thirdCamera.enabled = this.gameObject == PlayerManager.localPlayer;
    }
    void FixedUpdate()
    {
        if (pi.changeCamera)
        {
            firstCamera.enabled = !firstCamera.enabled;
            thirdCamera.enabled = !thirdCamera.enabled;
        }
        pi.changeCamera = false;

        float speedX = pi.mouseX * cameraSpeed * Time.fixedDeltaTime;
        float speedY = pi.mouseY * cameraSpeed * Time.fixedDeltaTime;
        v_rotate.transform.localRotation = v_rotate.transform.localRotation * Quaternion.Euler(-speedY, 0, 0);
        h_rotate.transform.localRotation = h_rotate.transform.localRotation * Quaternion.Euler(0, speedX, 0);
    }
}
