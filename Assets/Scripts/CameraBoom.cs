using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;


public class CameraBoom : MonoBehaviour
{
    [SerializeField] private Slider mouseSensitivitySlider;

    [SerializeField] private Transform playerCamTransform;
    [SerializeField] private Vector3 camOffset;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform yRotatorTransform;

    [SerializeField] private Rigidbody playerObjectRbd;
    [SerializeField] private Rigidbody jetObjectRbd;

    //private bool isJetCamIntro;
    //private Vector3 startPos;

    private float camVerticalAngle;
    private bool isAlternateCamPos;

    
    void Start()
    {
        //isJetCamIntro = false;
        camVerticalAngle = 0;
        isAlternateCamPos = false;
        mouseSensitivitySlider.value = 1f;
    }

    
    void Update()
    {
        MainManager.Instance.mouseSensitivity = mouseSensitivitySlider.value;

        if (MainManager.Instance.isGameActive)
        {
            SwitchView();

            if (MainManager.Instance.isPlayerInJet)
            {
                UseJetCamBoom();
            }

            if (!MainManager.Instance.isPlayerInJet)
            {
                UsePlayerCamBoom();
                //isJetCamIntro = false;
            }

        }
    }


    private void FixedUpdate()
    {
        if (MainManager.Instance.isPlayerInJet)
        {
            transform.position = jetObjectRbd.transform.position;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, jetObjectRbd.transform.rotation, Time.deltaTime * 2f);
        }        
    }


    private void SwitchView()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.R))
        {
            if (!isAlternateCamPos)
            {
                isAlternateCamPos = true;
                camOffset.x = -1.5f;
            }
            else
            {
                isAlternateCamPos = false;
                camOffset.x = 1.5f;
            }
        }
    }



    private void UsePlayerCamBoom()
    {
        float mousePosX = Input.GetAxisRaw("Mouse X");
        float mousePosY = Input.GetAxisRaw("Mouse Y");

        MainManager.Instance.mouseSensitivity = Mathf.Clamp(MainManager.Instance.mouseSensitivity, 0.1f, 2f);

        transform.position = playerTransform.position + new Vector3(0f, 5f, 0f);
        transform.Rotate(0f, mousePosX * MainManager.Instance.mouseSensitivity, 0f);

        camVerticalAngle += mousePosY * MainManager.Instance.mouseSensitivity;
        camVerticalAngle = Mathf.Clamp(camVerticalAngle, -60f, 60f);

        yRotatorTransform.localEulerAngles = new Vector3(-camVerticalAngle, 0f, 0f);

        playerCamTransform.localPosition = camOffset + new Vector3(0f, 0f, ((camVerticalAngle + 35f) * camVerticalAngle) * 0.001f);
    }



    private void UseJetCamBoom()
    {
        playerCamTransform.localPosition = new Vector3(0f, 12f, -40f);
        Vector3 targetDir = jetObjectRbd.transform.position - playerCamTransform.position;
        playerCamTransform.rotation = Quaternion.LookRotation(targetDir);
    }

}
