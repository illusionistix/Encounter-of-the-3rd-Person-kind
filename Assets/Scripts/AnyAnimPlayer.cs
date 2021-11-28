using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyAnimPlayer : MonoBehaviour
{
    private Animator anyAnimAnimator;

    // Start is called before the first frame update
    void Start()
    {
        anyAnimAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            //anyAnimAnimator.enabled = true;            
            if (!MainManager.Instance.isPlayerInJet)
            {
                anyAnimAnimator.enabled = true;
            }
            else
            {
                anyAnimAnimator.enabled = false;
            }
            

        }
        else
        {
            anyAnimAnimator.enabled = false;
        }
    }
}
