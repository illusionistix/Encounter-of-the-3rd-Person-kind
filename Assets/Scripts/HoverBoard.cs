using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBoard : MonoBehaviour
{

    private Vector3 boardOffset;
    private Rigidbody boardRb;

    // Start is called before the first frame update
    void Start()
    {        
        boardRb = GetComponent<Rigidbody>();
        boardOffset = new Vector3(0f, 2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {        
        if (MainManager.Instance.isPlayerOnHoverboard)
        {
            Transform pTransform = MainManager.Instance.playerLegRTransform;
            
            transform.position = pTransform.position - boardOffset;            

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, pTransform.rotation.eulerAngles.y + 22.5f,
            transform.eulerAngles.z);

            if (transform.position.y < 2f)
            {
                boardRb.AddForce(Vector3.up * Time.deltaTime * 150f, ForceMode.Impulse); 
                // / Mathf.Pow(transform.position.y, 2)), ForceMode.Impulse);
            }
            
        }        

        
        if (MainManager.Instance.offHoverboard)
        {
            boardRb.velocity = new Vector3(0f, 0f, 0f);
        }

    }

    
}
