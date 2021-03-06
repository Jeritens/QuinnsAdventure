using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningTop : MonoBehaviour
{
    [SerializeField]
    public float maxRpm = 1000f;
    public float rpm = 1000;
    [SerializeField]
    Transform outerSpin;
    [SerializeField]
    Transform childTransform;
    [SerializeField]
    Transform pivot;
    [SerializeField]
    public float slowDownRate;
    [SerializeField]
    LayerMask BounceOff;
    Rigidbody rb;
    [SerializeField]
    public float knockBack;
    public bool active = false;
    public float pointsPerSecond;
    [SerializeField]
    float randomForce;
    [SerializeField]
    AudioClip[] collisionSounds;
    [SerializeField]
    AudioClip[] wooshSounds;
    AudioClip wooshSound;
    AudioSource audioSource;
    float wooshTime;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        wooshSound = wooshSounds[Random.Range(0, wooshSounds.Length)];
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = wooshSound;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = !active;
    }
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            SlowDown();
            CheckStopped();
            Tilt();
            childTransform.Rotate(Vector3.up * rpm * Time.deltaTime, Space.Self);
            outerSpin.Rotate(Vector3.up * rpm * Time.deltaTime * 0.5f, Space.Self);
            wooshTime += Time.deltaTime * rpm / maxRpm;
            if (wooshTime > 0.314f)
            {
                audioSource.PlayOneShot(wooshSound, 0.5f);
                //audioSource.Play();
                wooshTime = 0;
            }
        }


    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        RandomMove();
    }
    void RandomMove()
    {
        float x = transform.position.x * Time.timeSinceLevelLoad * 0.1f;
        float y = transform.position.y * Time.timeSinceLevelLoad;
        Vector3 force = new Vector3(Mathf.PerlinNoise(x, y) - 0.5f, 0, Mathf.PerlinNoise(x + 0.69f, y + 0.420f) - 0.5f);
        rb.AddForce(force * randomForce);
    }
    public void speedUp(float force)
    {
        rpm = Mathf.Min(maxRpm, force + rpm);
    }
    void Tilt()
    {
        pivot.localRotation = Quaternion.Slerp(pivot.localRotation, Quaternion.Euler(((1 - (rpm / maxRpm)) * 35), 0, 0), Time.deltaTime * 0.995f);
    }
    public void Hit(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }
    void SlowDown()
    {
        rpm = Mathf.Max(rpm - slowDownRate * Time.deltaTime, 0);
        //rpm *= Mathf.Pow(slowDownRate,Time.deltaTime);
    }

    void CheckStopped()
    {
        if (rpm == 0)
        {
            GameManager.instance.stoppedSpinning();
        }
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("test");
        if (BounceOff == (BounceOff | (1 << other.gameObject.layer)))
        {

            Vector3 dir = transform.position - other.transform.position;
            dir.y = 0;
            dir.Normalize();
            rb.AddForce(dir * rpm * knockBack, ForceMode.Impulse);
            if (GameManager.instance.powerUps[0] && other.gameObject.GetComponent<SpinningTop>() != null)
            {
                speedUp(other.gameObject.GetComponent<SpinningTop>().rpm * 0.1f);
            }
            audioSource.PlayOneShot(collisionSounds[Random.Range(0, collisionSounds.Length)], 0.8f);
        }

    }
    public void Throw(Vector3 dir)
    {
        active = true;
        rb.isKinematic = false;
        rb.AddForce(dir, ForceMode.Impulse);
    }
    void TimeTopSlowDown()
    {
        gyro[] gyros = GameObject.FindObjectsOfType<gyro>();
        bool slowed = false;
        foreach (gyro g in gyros)
        {
            if ((transform.position - g.transform.position).magnitude < g.radius)
            {
                slowed = true;
            }
        }
        if (slowed)
            speedUp(slowDownRate / 2);
    }

}
