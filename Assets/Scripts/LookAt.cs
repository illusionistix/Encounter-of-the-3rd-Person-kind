using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform lookAtObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(lookAtObject.position.x, lookAtObject.position.y + 3f,
            lookAtObject.position.z) - transform.position;
        transform.rotation = Quaternion.LookRotation(targetPos);
    }
}
