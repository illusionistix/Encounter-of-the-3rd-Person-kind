using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetVehicle : MonoBehaviour
{
    private Rigidbody jetRb;
    [SerializeField] private ParticleSystem thrustPs;
    [SerializeField] private float hRotSpeed = 100f;
    [SerializeField] private float playerSpeed = 100f;
    private Vector3 startPos;
    private Quaternion startRot;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        jetRb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            if (MainManager.Instance.isPlayerInJet)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                transform.Rotate(Vector3.up, horizontalInput * Time.deltaTime * hRotSpeed);

                UpdatePlayerAcceleration();
                UpdateThrustActivation();
            }
            else
            {
                transform.position = startPos;
                transform.rotation = startRot;
            }

        }
    }


    private void UpdatePlayerAcceleration()
    {
        if (Input.GetKey(KeyCode.M) || Input.GetKey(KeyCode.Joystick1Button2))
        {
            jetRb.AddRelativeForce(Vector3.forward * playerSpeed * Time.deltaTime,
                ForceMode.Impulse);
        }
    }


    private void UpdateThrustActivation()
    {
        if (Input.GetKey(KeyCode.B) || Input.GetKey(KeyCode.Joystick1Button5))
        {
            if (thrustPs.isStopped && MainManager.Instance.energy > 2)
            {
                thrustPs.Play();
            }
            else if (MainManager.Instance.energy < 2)
            {
                thrustPs.Stop();
            }


            if (MainManager.Instance.energy > 2)
            {
                jetRb.AddRelativeForce(Vector3.forward * playerSpeed * 2.5f * Time.deltaTime,
                ForceMode.Impulse);
            }

            MainManager.Instance.energy -= 5f * Time.deltaTime;
        }
        else
        {
            thrustPs.Stop();
            MainManager.Instance.energy += 35f * Time.deltaTime;
        }
    }

}
