using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject spinningTopPickUp;
    void Start()
    {
        spawnSpinningTop();
    }

    public void spawnSpinningTop(){
        Instantiate(spinningTopPickUp, transform.position, Quaternion.identity,transform);
    }
}
