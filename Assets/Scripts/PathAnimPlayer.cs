using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimPlayer : MonoBehaviour
{
    private Animator jetBotAnimator;

    // Start is called before the first frame update
    void Start()
    {
        jetBotAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            jetBotAnimator.enabled = true;
            
        }
        else
        {
            jetBotAnimator.enabled = false;            
        }
    }
}
