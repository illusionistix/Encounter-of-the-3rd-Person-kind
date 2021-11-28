using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBoom : MonoBehaviour
{
    [SerializeField] private Slider mouseSensitivitySlider;

    [SerializeField] private Transform playerCamTransform;
    [SerializeField] private Transform playerObjectTransform;
    [SerializeField] private Transform jetObjectTransform;

    //private bool isJetCamIntro;
    //private Vector3 startPos;

    private float camVerticalAngle;
    //private bool isAlternateCamPos;

    // Start is called before the first frame update
    void Start()
    {
        //isJetCamIntro = false;
        camVerticalAngle = 0;
        //isAlternateCamPos = false;
        mouseSensitivitySlider.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
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

        MainManager.Instance.mouseSensitivity = mouseSensitivitySlider.value;


    }


    private void UsePlayerCamBoom()
    {
        float mousePosX = Input.GetAxisRaw("Mouse X");
        float mousePosY = Input.GetAxisRaw("Mouse Y");

        MainManager.Instance.mouseSensitivity = Mathf.Clamp(MainManager.Instance.mouseSensitivity, 0.1f, 2f);

        transform.Rotate(new Vector3(0f, mousePosX * MainManager.Instance.mouseSensitivity, 0f));

        camVerticalAngle += mousePosY * MainManager.Instance.mouseSensitivity;
        camVerticalAngle = Mathf.Clamp(camVerticalAngle, -89f, 89f);

        playerCamTransform.localEulerAngles = new Vector3(-camVerticalAngle, 0, 0);
        playerCamTransform.localPosition = new Vector3(3f, 7f, -12.5f);

        //transform.position = Vector3.Lerp(transform.position, playerObjectTransform.position, Time.deltaTime * 4f);
        transform.position = playerObjectTransform.position;
    }

    

    private void UseJetCamBoom()
    {
        //transform.SetParent(jetObjectTransform);
        transform.position = jetObjectTransform.position;
        //transform.rotation = jetObjectTransform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, jetObjectTransform.rotation, Time.deltaTime * 2f);
        //transform.localPosition = new Vector3(0f, 5f, -15f);        

        playerCamTransform.localPosition = new Vector3(0f, 12f, -40f);
        Vector3 targetDir = jetObjectTransform.position - playerCamTransform.position;
        playerCamTransform.rotation = Quaternion.LookRotation(targetDir);

        //while (Vector3.Distance(transform.position, jetObjectTransform.position) > 50f)
        //{            
            //transform.position = Vector3.Lerp(transform.position, jetObjectTransform.position, Time.deltaTime * 2f);            
        //}        
        
    }

}
