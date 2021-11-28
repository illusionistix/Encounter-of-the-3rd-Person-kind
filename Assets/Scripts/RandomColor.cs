using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{

    private MeshRenderer tintRenderer;
    private int tintMaterialSlot;

    // Start is called before the first frame update
    void Start()
    {
        tintRenderer = GetComponent<MeshRenderer>();
        tintMaterialSlot = 0;
        float rangeR = Random.Range(0.1f, 255f);
        float rangeG = Random.Range(0.1f, 255f);
        float rangeB = Random.Range(0.1f, 255f);
        Color randomColor = new Color(50f, 120f, 0f);
        SetColor(randomColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void SetColor(Color c)
    {
        var prop = new MaterialPropertyBlock();
        prop.SetColor("_BaseColor", c);
        tintRenderer.SetPropertyBlock(prop, tintMaterialSlot);
    }
    
}
