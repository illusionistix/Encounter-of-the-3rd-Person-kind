using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_projectileSpeed = 700f;
    public float projectileSpeed { get { return m_projectileSpeed; } set { m_projectileSpeed = 700f; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            StartCoroutine(ProjectileLifeTimer(0.1f));
            transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
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
