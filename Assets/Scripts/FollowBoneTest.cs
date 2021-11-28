using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoneTest : MonoBehaviour
{
    [SerializeField] private Transform targetBoneTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetBoneTransform.position - new Vector3(0f, 0.5f, 0f);
    }
}
