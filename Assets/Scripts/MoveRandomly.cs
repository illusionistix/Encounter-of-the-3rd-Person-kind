using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomly : MonoBehaviour
{
    private GameObject targetObject;    

    private float range;
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetObject = MainManager.Instance.playerObject;
        //range = Random.Range(0f, 179f);
        GetRandomLocation();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {

            transform.Translate((targetPos - transform.position).normalized * Time.deltaTime * 10f);

            if (Vector3.Distance(targetPos, transform.position) < 0.1f)
            {
                GetRandomLocation();
            }

            if (transform.position.y < 1.5f)
            {
                transform.position = new Vector3(transform.position.y, 1.5f, transform.position.z);
                GetRandomLocation();
            }

            if (Vector3.Distance(transform.position, targetObject.transform.position) < 25f)
            {
                transform.position = Vector3.Lerp(transform.position, targetObject.transform.position, Time.deltaTime * 25f);
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
