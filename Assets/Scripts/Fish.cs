using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody fishRb = GetComponent<Rigidbody>();
        fishRb.AddRelativeTorque(Vector3.one * 100f, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
