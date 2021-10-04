using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraLife : MonoBehaviour
{
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.up*Time.deltaTime*90);
        
    }
    public void AddLife(){
        GameManager.instance.extraLifes++;
    }
}
