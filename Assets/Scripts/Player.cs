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
    bool isGrounded;
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
    GameObject spinningTopHold;
    [SerializeField]
    float spinningTopReleaseDistance;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    float groundDistance;
    [SerializeField]
    LayerMask groundMask;
    [SerializeField]
    Transform holdingPosition;

    [SerializeField]
    AudioClip[] punchSounds;
    AudioSource audio;


    private void Start() {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Gravity();
        LookToMouse();
        Drag();
        if(Input.GetButtonDown("Fire1")){
            if(spinningTopHold!=null){
                ThrowSpinningTop();
                spinningTopHold=null;
            }
            else{
                Hit();
            }
        }
        
    }
    void Drag(){
        velocity = new Vector3(velocity.x*Mathf.Pow(isGrounded?0.005f:0.5f,Time.deltaTime),velocity.y,velocity.z*Mathf.Pow(isGrounded?0.005f:0.5f,Time.deltaTime));
        if(isGrounded && Mathf.Sqrt(velocity.x*velocity.x + velocity.z * velocity.z)< 2){
            velocity.x=0;
            velocity.z=0;
        }
    }
    public void Move(){
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        

        Vector3 move = new Vector3(x,0,z).normalized*Time.deltaTime*speed;
        controller.Move(move);
        MoveAnim(move.normalized);
    }
    void MoveAnim(Vector3 dir){
        Vector3 direction = transform.InverseTransformDirection(dir).normalized;

        // Debug.DrawRay(transform.position,transform.right);
        // Debug.DrawRay(Vector3.zero,direction,Color.green);
        animator.SetFloat("PosX",direction.x);
        animator.SetFloat("PosY",direction.z);
    }
    void Gravity(){
        isGrounded= Physics.CheckSphere(groundCheck.position, groundDistance,groundMask);
        if(isGrounded && velocity.y<0){
            velocity.y =-2f;
        }
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
        if(!animator.GetCurrentAnimatorStateInfo(1).IsName("punch")){
            animator.Play("punch");
            audio.PlayOneShot(punchSounds[Random.Range(0, punchSounds.Length)]);
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
    }

    void ThrowSpinningTop(){
        animator.SetBool("holding",false);
        animator.Play("throw");
        //Instantiate(spinningTopHold, transform.position + transform.forward * spinningTopReleaseDistance, Quaternion.identity);
        spinningTopHold.GetComponent<SpinningTop>().Throw(transform.forward*10);
        spinningTopHold.transform.parent=null;
        GameManager.instance.addSpinningTop();
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.GetComponent<SpinningTop>()!=null){
            
            SpinningTop top =hit.gameObject.GetComponent<SpinningTop>();
            if(top.active){
                Vector3 topToPlayer = transform.position-hit.gameObject.transform.position;
                topToPlayer.y=0;
                topToPlayer.Normalize();
                Vector3 dir = Quaternion.Euler(0,80,0) * topToPlayer;
                // Debug.DrawRay(transform.position,topToPlayer,Color.blue,5);
                // Debug.DrawRay(transform.position,dir,Color.red,5);
                velocity += dir*hit.gameObject.GetComponent<SpinningTop>().rpm*0.001f;
                velocity+=Vector3.up*top.rpm*0.01f;
                velocity.y = Mathf.Min(2,velocity.y);
            }
            
        }

        if(hit.gameObject.GetComponent<SpinningTopPickUp>()!=null){
            spinningTopHold = hit.gameObject.GetComponent<SpinningTopPickUp>().getSpinningTop();
            animator.SetBool("holding",true);
            spinningTopHold.transform.position=holdingPosition.position;
            spinningTopHold.transform.parent=transform;
            Destroy(hit.gameObject.GetComponent<SpinningTopPickUp>().gameObject);
        }
    }


}
