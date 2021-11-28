using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlPool : MonoBehaviour
{
    //private Vector3 forceDir;

    // Start is called before the first frame update
    void Start()
    {
        //forceDir = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float freq = Time.deltaTime;// * (2 * Mathf.PI);
        float posX = Mathf.Sin(freq * 10f) * 25f;
        float posZ = -Mathf.Cos(freq * 10f) * 25f;
        forceDir = new Vector3(posX, 0f, posZ);
        */
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            other.gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up, ForceMode.VelocityChange);
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.5f, ForceMode.VelocityChange);            
        }
        
    }

}
