using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTextSortingLayer : MonoBehaviour
{
    [SerializeField] private string layerToPushTo;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().sortingLayerName = layerToPushTo;
    }
}
