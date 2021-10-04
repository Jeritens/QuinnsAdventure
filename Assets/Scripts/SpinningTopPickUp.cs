using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningTopPickUp : MonoBehaviour
{
    [SerializeField]
    GameObject spinningTopPrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getSpinningTop(){
        return spinningTopPrefab;
    }
}
