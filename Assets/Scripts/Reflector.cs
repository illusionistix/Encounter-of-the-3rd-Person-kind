using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            if (!MainManager.Instance.isPlayerInJet)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * (-moveSpeed));
            }
            
        }

    }

    
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);

        transform.rotation = Quaternion.LookRotation(reflectDir);
    }

    
}
