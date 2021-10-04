using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gyro : MonoBehaviour
{
    public Transform outer;
    [SerializeField]
    float outerRpm;
    public Transform inner;
    [SerializeField]
    float innerRpm;
    [SerializeField]
    LayerMask topMask;
    [SerializeField]
    float radius;



    // Update is called once per frame
    void Update()
    {
        outer.Rotate(Vector3.forward * outerRpm * Time.deltaTime, Space.Self);
        //inner.localRotation = Quaternion.Euler(inner.localRotation.eulerAngles + Vector3.right * innerRpm);
        inner.Rotate(Vector3.up * innerRpm * Time.deltaTime, Space.Self);

        speedUpAroundYou();
    }
    void speedUpAroundYou()
    {
        Collider[] tops = Physics.OverlapSphere(transform.position, radius, topMask);
        foreach (Collider col in tops)
        {
            if (col.GetComponent<SpinningTop>() != null)
            {
                SpinningTop top = col.GetComponent<SpinningTop>();
                top.speedUp((top.slowDownRate / 2) * Time.deltaTime);
            }
        }
    }
}
