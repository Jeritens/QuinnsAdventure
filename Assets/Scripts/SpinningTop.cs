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
    [SerializeField]
    LayerMask BounceOff;
    Rigidbody rb;
    [SerializeField]
    float knockBack;
    public bool active = false;
    public float pointsPerSecond;

/// <summary>
/// Start is called on the frame when a script is enabled just before
/// any of the Update methods is called the first time.
/// </summary>
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.isKinematic =!active;
    }
    // Update is called once per frame
    void Update()
    {
        if(active){
            SlowDown();
            CheckStopped();
            Tilt();
            childTransform.Rotate(Vector3.up * rpm * Time.deltaTime,Space.Self);
            outerSpin.Rotate(Vector3.up * rpm * Time.deltaTime*0.5f,Space.Self);
        }
        

    }
    public void speedUp(float force){
        rpm= Mathf.Min(maxRpm,force+rpm);
    }
    void Tilt(){
        pivot.localRotation = Quaternion.Slerp(pivot.localRotation,Quaternion.Euler(((1-(rpm/maxRpm))*35),0,0),Time.deltaTime*0.995f);
    }
    public void Hit(Vector3 force){
        rb.AddForce(force,ForceMode.Impulse);
    }
    void SlowDown(){
        rpm = Mathf.Max(rpm-slowDownRate * Time.deltaTime,0);
        //rpm *= Mathf.Pow(slowDownRate,Time.deltaTime);
    }

    void CheckStopped(){
        if(rpm == 0){
            GameManager.instance.stoppedSpinning();
        }
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("test");
        if( BounceOff == (BounceOff | (1 << other.gameObject.layer))){
            
            Vector3 dir = transform.position-other.transform.position;
            dir.y= 0;
            dir.Normalize();
            rb.AddForce(dir*rpm*knockBack,ForceMode.Impulse);
            if(GameManager.instance.powerUps[0] && other.gameObject.GetComponent<SpinningTop>()!=null){
                speedUp(other.gameObject.GetComponent<SpinningTop>().rpm *0.1f);
            }
        }   
        
    }
    public void Throw(Vector3 dir){
        active = true;
        rb.isKinematic=false;
        rb.AddForce(dir,ForceMode.Impulse);
    }
    
}
