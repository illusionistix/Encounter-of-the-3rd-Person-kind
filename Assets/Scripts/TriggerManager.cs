using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{

    

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
        if (gameObject.CompareTag("WelcomeInfo"))
        {
            MainManager.Instance.playerFirstWelcome = true;
            Destroy(gameObject);
        }

        
        if (gameObject.CompareTag("SecretInfo"))
        {
            MainManager.Instance.playerFoundSecret = true;
            //Destroy(gameObject);
        }


        if (gameObject.CompareTag("JetInfo"))
        {
            MainManager.Instance.playerNearJetVehicle = true;
            //Destroy(gameObject);
        }


        if (gameObject.CompareTag("CarInfo"))
        {
            MainManager.Instance.playerNearCarVehicle = true;
            //Destroy(gameObject);
        }


        if (gameObject.CompareTag("LimitsInfo"))
        {
            MainManager.Instance.playerNearLimits = true;
        }
        
    }

}
