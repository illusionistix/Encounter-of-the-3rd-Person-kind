using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDebris : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            StartCoroutine(HazardDebrisLifeTimer(0.5f));
        }
        else
        {
            StopAllCoroutines();
        }
    }


    IEnumerator HazardDebrisLifeTimer(float sec)
    {
        yield return new WaitForSeconds(sec);

        Destroy(gameObject);
    }


}
