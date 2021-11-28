using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascelator : MonoBehaviour
{

    [SerializeField] private float ascelationFactor = 10f;
    private float scaleZ;
    //[SerializeField] private GameObject associatedObject;
    //private float rotXFactor;

    // Start is called before the first frame update
    void Start()
    {
        scaleZ = transform.localScale.z;
        //rotXFactor = (360f - associatedObject.transform.rotation.eulerAngles.x) * 2.25f;
        //Debug.Log(scaleZ);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rotXFactor);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            float dotResult = Vector3.Dot(transform.forward, collision.transform.forward);

            if (dotResult > 0)
            {
                collision.rigidbody.AddForce(transform.forward * (ascelationFactor) * scaleZ, 
                    ForceMode.Impulse);
            }

            if (dotResult < 0)
            {
                collision.rigidbody.AddForce((-transform.forward) * (ascelationFactor) * scaleZ,
                    ForceMode.Impulse);
            }
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.velocity.magnitude < 50f)
            {
                float dotResult = Vector3.Dot(transform.forward, collision.transform.forward);

                if (dotResult > 0)
                {
                    collision.rigidbody.AddForce(transform.forward * Time.deltaTime * (ascelationFactor) * scaleZ,
                        ForceMode.Impulse);
                }

                if (dotResult < 0)
                {
                    collision.rigidbody.AddForce((-transform.forward) * Time.deltaTime * (ascelationFactor) * scaleZ,
                        ForceMode.Impulse);
                }
            }
        }
    }

}
