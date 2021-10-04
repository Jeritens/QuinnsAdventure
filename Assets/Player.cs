using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    CharacterController controller;
    Vector3 velocity;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    float punchDist;
    [SerializeField]
    float punchRadius;
    [SerializeField]
    LayerMask spinningTopMask;
    [SerializeField]
    float PunchStrength;
    [SerializeField]
    float knockBack;
    bool hasSpinningTop;
    public GameObject spinningTopPrefab;
    [SerializeField]
    float spinningTopReleaseDistance;



    void Awake()
    {
        hasSpinningTop = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Gravity();
        LookToMouse();
        if(Input.GetButtonDown("Fire1")){
            if(hasSpinningTop){
                SpawnSpinningTop();
                hasSpinningTop = false;
            }
            else{
                Hit();
            }
        }
        
    }
    public void Move(){
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        

        Vector3 move = new Vector3(x,0,z).normalized*Time.deltaTime*speed;
        controller.Move(move);
    }
    void Gravity(){
        velocity += Physics.gravity*Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);
    }

    void LookToMouse(){
        Ray cameraRay =Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0,1.5f,0)); // y value (1.5) eye level for better rotation, maybe change with ground floor later
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength)){
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            // lerp not good
            // Quaternion rotation = Quaternion.LookRotation(new Vector3(pointToLook.x, transform.position.y, pointToLook.z)-transform.position);
            // transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            Vector3 direction = new Vector3(pointToLook.x, transform.position.y, pointToLook.z) - transform.position;
            Quaternion rotation1 = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation1, rotationSpeed*Time.deltaTime);
        }
    }
    void Hit(){
        Collider[] tops = Physics.OverlapSphere(transform.position+transform.forward*punchDist,punchRadius,spinningTopMask);
        foreach (Collider top in tops)
        {
            SpinningTop st = top.GetComponent<SpinningTop>();
            st.speedUp(PunchStrength);
            st.Hit(transform.forward*knockBack);
        }


        // Debug.DrawRay(transform.position+transform.forward*punchDist,Vector3.forward *punchRadius,Color.green);
        // Debug.DrawRay(transform.position+transform.forward*punchDist,-Vector3.forward *punchRadius,Color.green);
        // Debug.DrawRay(transform.position+transform.forward*punchDist,Vector3.up *punchRadius,Color.green);
        // Debug.DrawRay(transform.position+transform.forward*punchDist,-Vector3.up *punchRadius,Color.green);
        // Debug.DrawRay(transform.position+transform.forward*punchDist,Vector3.right *punchRadius,Color.green);
        // Debug.DrawRay(transform.position+transform.forward*punchDist,-Vector3.right *punchRadius,Color.green);
    }

    void SpawnSpinningTop(){
        Instantiate(spinningTopPrefab, transform.position + transform.forward * spinningTopReleaseDistance, Quaternion.identity);
    }




}
