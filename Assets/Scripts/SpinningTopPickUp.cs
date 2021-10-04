using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningTopPickUp : MonoBehaviour
{
    [SerializeField]
    GameObject spinningTopPrefab;
    [SerializeField]
    Transform spin;
    [SerializeField]
    float rpm;
      [SerializeField]
    float pivotRpm;
    [SerializeField]
    Transform outerRotation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        outerRotation.Rotate(Vector3.up * rpm * Time.deltaTime*0.5f,Space.Self);
        spin.Rotate(Vector3.up * rpm * Time.deltaTime,Space.Self);
    }

    public GameObject getSpinningTop(){
        
        return GameObject.Instantiate(spinningTopPrefab,transform.position,Quaternion.identity);
    }
}
