using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{    

    [SerializeField] private ParticleSystem hazardPsPrefab;
    private ParticleSystem hazardPs;    

    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (transform.position.y < 2f)
        {
            if (hazardPs == null)
            {
                hazardPs = Instantiate(hazardPsPrefab, transform.position, transform.rotation);
            }
        }

        if (MainManager.Instance.isGameActive)
        {
            if (hazardPs != null)
            {
                hazardPs.Play();
            }
        }
        else
        {
            if (hazardPs != null)
            {
                hazardPs.Pause();                
            }            
        }                
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (hazardPs == null)
        {
            hazardPs = Instantiate(hazardPsPrefab, transform.position, transform.rotation);
        }

        if (other.CompareTag("Launchable"))
        {
            MainManager.Instance.score += 100;
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            MainManager.Instance.energy -= 10;
            Destroy(gameObject);
        }

    }

    
}
