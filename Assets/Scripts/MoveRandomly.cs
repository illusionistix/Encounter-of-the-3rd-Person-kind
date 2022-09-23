using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomly : MonoBehaviour
{
    private GameObject targetObject;    

    private float range;
    private Vector3 targetPos;

    
    void Start()
    {
        targetObject = MainManager.Instance.playerObject;
        //range = Random.Range(0f, 179f);
        GetRandomLocation();
    }

    
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 0.5f);
            //transform.Translate((targetPos - transform.position).normalized * Time.deltaTime * 10f);
            //transform.position += new Vector3(0f, Mathf.Sin(Time.time) * 0.1f, 0f);

            if (Vector3.Distance(targetPos, transform.position) < 1f)
            {
                GetRandomLocation();
            }
            else if (transform.position.y < 1.5f)
            {
                transform.position = new Vector3(transform.position.y, 1.5f, transform.position.z);
                GetRandomLocation();
            }
            else if (Vector3.Distance(transform.position, targetObject.GetComponent<Rigidbody>().transform.position) < 50f)
            {
                transform.position = Vector3.Lerp(transform.position, targetObject.GetComponent<Rigidbody>().transform.position + new Vector3(0f, 3f, 0f), Time.deltaTime * 5f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 0.5f);
            }

        }
    }


    void GetRandomLocation()
    {
        range = Random.Range(-10f, 10f);        
        
        if (CompareTag("Hazard"))
        {
            targetPos = transform.position + new Vector3(range * 2 - 1, Random.Range(-5f, 5f), range * 2 - 1);
        }

    }

}
