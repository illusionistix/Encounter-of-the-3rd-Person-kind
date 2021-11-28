using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotController : MonoBehaviour
{
    [SerializeField] private float factor;

    // Start is called before the first frame update
    void Start()
    {
        //factor = 10f;
        //Debug.Log(transform.eulerAngles.x);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, (-Time.deltaTime) * (factor));

        if (transform.eulerAngles.x < 338f)
        {
            factor *= -1;
        }
    }
}
