using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDebris : MonoBehaviour
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
            StartCoroutine(ProjectileDebrisLifeTimer(0.2f));
        }
        else
        {
            StopAllCoroutines();
        }
    }


    IEnumerator ProjectileDebrisLifeTimer(float sec)
    {
        yield return new WaitForSeconds(sec);

        Destroy(gameObject);
    }


}
