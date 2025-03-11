using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarvingRoom : MonoBehaviour
{
    public SpriteRenderer _renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        SpineHighLight();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SpineHighLight()
    {
        uint layerToAdd = 1u << 1;
        _renderer.renderingLayerMask |= layerToAdd;
    }
    
    void SpineQuitHighLight()
    {
        uint layerToRemove = 1u << 1;
        _renderer.renderingLayerMask &= ~layerToRemove;
    }
}
