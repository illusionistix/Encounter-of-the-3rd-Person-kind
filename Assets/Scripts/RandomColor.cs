using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class RandomColor : MonoBehaviour
{

    private MeshRenderer tintRenderer;
    private SkinnedMeshRenderer tintRenderer1;

    private Color randomColor;

    [SerializeField] private bool shiftHue = false;

    private float sign;

    //private float randomOffset;
    private float randomSpeedFactor;



    void Start()
    {
        randomSpeedFactor = Random.Range(0.2f, 1.9f);

        sign = 1;

        if (GetComponent<MeshRenderer>() != null)
        {
            tintRenderer = GetComponent<MeshRenderer>();
        }

        if (GetComponent<SkinnedMeshRenderer>() != null)
        {
            tintRenderer1 = GetComponent<SkinnedMeshRenderer>();
        }

        float rangeR = Random.Range(0.1f, 1f);
        float rangeG = Random.Range(0.1f, 1f);
        float rangeB = Random.Range(0.1f, 1f);

        randomColor = new Color(rangeR, rangeG, rangeB);
        
        SetColor(randomColor);
    }

    
    void Update()
    {
        if (shiftHue)
        {
            ShiftColorHue();
        }
    }

    
    private void SetColor(Color c)
    {
        if (tintRenderer != null)
        {
            tintRenderer.materials[0].color = c;
        }

        if (tintRenderer1 != null)
        {
            tintRenderer1.materials[0].color = c;
        }
    }


    private void ShiftColorHue()
    {
        if (tintRenderer != null)
        {
            float r = tintRenderer.materials[0].color.r;
            float g = tintRenderer.materials[0].color.g;
            float b = tintRenderer.materials[0].color.b;

            if (r < 0.2f)
            {
                sign = 1;
            }
            
            if (r > 0.8f)
            {
                sign = -1;
            }
            

            tintRenderer.materials[0].color = new Color(r + ((Time.deltaTime) * randomSpeedFactor) * sign,
                g, b, 1f);
        }

        if (tintRenderer1 != null)
        {
            float r = tintRenderer1.materials[0].color.r;
            float g = tintRenderer1.materials[0].color.g;
            float b = tintRenderer1.materials[0].color.b;

            if (r < 0.2f)
            {
                sign = 1;
            }

            if (r > 0.8f)
            {
                sign = -1;
            }


            tintRenderer1.materials[0].color = new Color(r + Time.deltaTime * 0.1f * sign,
                g, b, 1f);
        }
    }
    
}
