using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetBot : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    //private Animator jetBotAnimator;
    private TrailRenderer jetBotTrail;

    // Start is called before the first frame update
    void Start()
    {        
        //jetBotAnimator = GetComponent<Animator>();
        jetBotTrail = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {            
            //jetBotAnimator.enabled = true;
            jetBotTrail.time = 0.5f;
            transform.position = Vector3.Lerp(transform.position, targetObject.position, Time.deltaTime * 1.5f);
            Vector3 targetDir = targetObject.position - transform.position;
            transform.rotation = Quaternion.LookRotation(targetDir);
        }
        else
        {
            //jetBotAnimator.enabled = false;
            jetBotTrail.time = Mathf.Infinity;
        }
    }

    
}
