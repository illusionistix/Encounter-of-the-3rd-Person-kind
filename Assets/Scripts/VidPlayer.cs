using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{
    private VideoPlayer anyVidPlayer; 

    // Start is called before the first frame update
    void Start()
    {
        anyVidPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            if (!MainManager.Instance.isPlayerInJet)
            {
                anyVidPlayer.Play();
            }
            else
            {
                anyVidPlayer.Pause();
            }
            
        }
        else
        {
            anyVidPlayer.Pause();
        }
    }
}
