using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopPad : MonoBehaviour
{

    [SerializeField] private float hopPadForce = 150f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            float dotResult = Vector3.Dot(transform.forward, other.transform.forward);

            if (dotResult > 0)
            {
                other.attachedRigidbody.AddForce((Vector3.up + transform.forward) * hopPadForce, ForceMode.Impulse);
            }

        }
        
    }

}
