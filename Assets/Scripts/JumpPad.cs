using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private ParticleSystem jumpPadPs;

    [SerializeField] private float jumpPadForce = 2000f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        jumpPadPs = GetComponent<ParticleSystem>();

        if (jumpPadPs.isStopped)
        {
            jumpPadPs.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            if (jumpPadPs.isPaused)
            {
                jumpPadPs.Play();
            }
            
        }
        else
        {
            jumpPadPs.Pause();
        }
        
    }


    /*private void OnCollisionEnter(Collision collision)
    {
        MainManager.Instance.isJumpPadCollided = true;
    }*/


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Animaly"))
        {
            other.attachedRigidbody.AddForce(Vector3.up * jumpPadForce, ForceMode.Impulse);
        }
        
    }

}
