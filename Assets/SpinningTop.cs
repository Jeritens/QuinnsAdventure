using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningTop : MonoBehaviour
{
    [SerializeField]
    float maxRpm = 1000f;
    public float rpm = 1000;
    [SerializeField]
    Transform outerSpin;
    [SerializeField]
    Transform childTransform;
    [SerializeField]
    Transform pivot;
    [SerializeField]
    float slowDownRate;
    Rigidbody rb;

/// <summary>
/// Start is called on the frame when a script is enabled just before
/// any of the Update methods is called the first time.
/// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        SlowDown();
        Tilt();
        childTransform.Rotate(Vector3.up * rpm * Time.deltaTime,Space.Self);
        outerSpin.Rotate(Vector3.up * rpm * Time.deltaTime*0.5f,Space.Self);

    }
    public void speedUp(float force){
        rpm= Mathf.Min(maxRpm,force+rpm);
    }
    void Tilt(){
        pivot.localRotation = Quaternion.Euler(((1-(rpm/maxRpm))*35),0,0);
    }
    public void Hit(Vector3 force){
        rb.AddForce(force,ForceMode.Impulse);
    }
    void SlowDown(){
        rpm = Mathf.Max(rpm-slowDownRate * Time.deltaTime,0);
        //rpm *= Mathf.Pow(slowDownRate,Time.deltaTime);
    }
}
