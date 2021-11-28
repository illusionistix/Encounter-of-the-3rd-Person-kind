using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaly : MonoBehaviour
{
    private Animator animalyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        animalyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            if (!MainManager.Instance.isPlayerInJet)
            {
                animalyAnimator.enabled = true;
                transform.Translate(Vector3.forward * Time.deltaTime * -5f);
            }
            else
            {
                animalyAnimator.enabled = false;
            }
            
        }
        else
        {
            animalyAnimator.enabled = false;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);

        transform.rotation = Quaternion.LookRotation(reflectDir);
    }
}
