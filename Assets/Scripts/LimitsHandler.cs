using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitsHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        MainManager.Instance.playerNearLimits = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        MainManager.Instance.playerNearLimits = false;
    }
}
