using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTopSpeedUp : MonoBehaviour
{
    [SerializeField]
    Transform Top1;
    
    [SerializeField]
    Transform Top2;
    public float rpm;
    // Start is called before the first frame update
    public void activatePowerUp(){
        GameManager.instance.powerUps[0] = true;
    }

    // Update is called once per frame
    void Update()
    {
        Top1.RotateAround(Top1.position,Top1.forward,Time.deltaTime*rpm);
        Top2.RotateAround(Top2.position,Top2.forward,-Time.deltaTime*rpm);
    }
}
