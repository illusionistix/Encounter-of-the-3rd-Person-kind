using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingPool : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        //if (other.attachedRigidbody != null)
        //{
            //float mass = other.attachedRigidbody.mass;
            other.attachedRigidbody.AddForce(Vector3.up * Time.deltaTime * 150f, ForceMode.Impulse);
            //other.attachedRigidbody.drag *= 2;
        //}
        
    }


    
}
