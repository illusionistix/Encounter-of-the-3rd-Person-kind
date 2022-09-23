using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //private Hazard[] hazards;
    //private Vector3 targetPosition;

    //private float[] distanceToTargets;
    //private float minDistance;

    private float m_projectileSpeed = 700f;
    public float projectileSpeed { get { return m_projectileSpeed; } set { m_projectileSpeed = 700f; } }

    
    void Start()
    {
        //hazards = FindObjectsOfType<Hazard>();
        //distanceToTargets = new float[hazards.Length];
    }

    /*
    private void GetTargetPosition()
    {
        for (int i = 0; i < hazards.Length; i++)
        {
            distanceToTargets.SetValue(Vector3.Distance(transform.position, hazards[i].transform.position), i);

            if (Mathf.Min(distanceToTargets) > 0f)
            {
                minDistance = Mathf.Min(distanceToTargets);

                if (minDistance <= distanceToTargets[i])
                {
                    targetPosition = hazards[i].transform.position;
                }
            }
        }
    }
    */

    
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            StartCoroutine(ProjectileLifeTimer(0.1f));
            transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
        }
        else
        {
            StopAllCoroutines();
        }
    }


    IEnumerator ProjectileLifeTimer(float sec)
    {
        yield return new WaitForSeconds(sec);

        Destroy(gameObject);
    }

}
