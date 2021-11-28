using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private LineRenderer laserLine;
    private LineRenderer laserLineInst;

    //[SerializeField] private ParticleSystem laserPs;
    //private ParticleSystem laserPsInst;

    private int hitCount;
    private bool isHitting;    

    // Start is called before the first frame update
    void Start()
    {
        laserLineInst = Instantiate(laserLine, laserLine.transform.position, laserLine.transform.rotation);
        
        hitCount = 0;
        isHitting = false;        
    }

    // Update is called once per frame
    void Update()
    {
        DoRaycast(transform.position, transform.forward);        
    }

    void DoRaycast(Vector3 startPos, Vector3 facingDir)
    {
        RaycastHit hit;

        if (Physics.Raycast(startPos, facingDir, out hit, 50f))
        {
            if (hit.collider.CompareTag("Mirror"))
            {
                if (!isHitting)
                {
                    hitCount += 1;                    
                }                

                laserLineInst.positionCount = 1 + hitCount;
                laserLineInst.SetPosition(hitCount - 1, startPos);
                laserLineInst.SetPosition(hitCount, hit.point);

                Vector3 reflectDir = Vector3.Reflect(facingDir, hit.normal);
                DoRaycast(hit.point, reflectDir);
            }
            else
            {                

                if (!isHitting)
                {
                    hitCount += 1;
                    isHitting = true;
                    
                    laserLineInst.positionCount = 1 + hitCount;
                    laserLineInst.SetPosition(hitCount - 1, startPos);
                    laserLineInst.SetPosition(hitCount, hit.point);
                    /*
                    if (laserPsInst == null)
                    {
                        laserPsInst = Instantiate(laserPs, hit.point, Quaternion.LookRotation(facingDir));
                    }*/
                    

                    if (Physics.Raycast(startPos, facingDir, out hit, 50f) && !hit.collider.CompareTag("Mirror"))
                    {
                        hitCount = 0;
                        isHitting = false;
                    }
                }                


                if (Physics.Raycast(transform.position, transform.forward, out hit, 50f) &&
                    !hit.collider.CompareTag("Mirror"))
                {
                    hitCount = 0;
                    isHitting = false;

                    laserLineInst.positionCount = 2;
                    laserLineInst.SetPosition(0, transform.position);
                    laserLineInst.SetPosition(1, hit.point);
                }
                
            }
            
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
            {
                if (!isHitting)
                {
                    laserLineInst.positionCount = 2 + hitCount;
                    laserLineInst.SetPosition(laserLineInst.positionCount - 2, startPos);
                    laserLineInst.SetPosition(laserLineInst.positionCount - 1, startPos + (facingDir * 50f));
                    /*
                    if (laserPsInst != null)
                    {
                        Destroy(laserPsInst.gameObject);
                    }*/
                }                
            }
            else
            {
                hitCount = 0;
                isHitting = false;

                laserLineInst.positionCount = 2;
                laserLineInst.SetPosition(0, transform.position);
                laserLineInst.SetPosition(1, transform.position + (transform.forward * 50f));
            }
            
            hitCount = 0;
            isHitting = false;
        }

    }


}
