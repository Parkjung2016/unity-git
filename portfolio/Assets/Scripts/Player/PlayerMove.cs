using System;
using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;      // 캐릭터 움직임 스피드.
    public float jumpSpeed; // 캐릭터 점프 힘.
    public float gravity;    // 캐릭터에게 작용하는 중력.
    float ySpeed;
    float orignalStepOffset;
    
    private CharacterController controller; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러 콜라이더. 
    public bool RotatePlayer;
    public float rotation_speed;
    bool forward, back, left, right;
    int angle_to_rotete;
    Animator anim;
    PlayerAnim _PlayerAnim;
    public float walk;
    public float run;
    public bool movetrue;
    PlayerAttack playerAttack_;
    public Vector3 velocity;
    public Vector3 MoveDir;
    Rigidbody Rig;
    Collider col;
    public float withShieldSpeed;
    public bool IsImpact;
    private void Awake()
    {
        col = GetComponent<CapsuleCollider>();
        Rig = GetComponent<Rigidbody>();
        playerAttack_ = GetComponent<PlayerAttack>();
        _PlayerAnim = GetComponent<PlayerAnim>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        speed = 3.0f;
        jumpSpeed = 6.0f;
        gravity = 20.0f;

        orignalStepOffset = controller.stepOffset;
    }

    void Update()
    {
        if (PlayerHPManager.Instance.death)
        {
            return;
        }
        forward = Input.GetKey(KeyCode.W);
        back = Input.GetKey(KeyCode.S);
        left = Input.GetKey(KeyCode.A);
        right = Input.GetKey(KeyCode.D);
        // 현재 캐릭터가 땅에 있는가?
        if (movetrue && !anim.applyRootMotion)
        {
            if(_PlayerAnim.Block)
            {
                speed = withShieldSpeed;
            }
            if (Input.GetButton("Sprint") && _PlayerAnim.Block)
            {
                speed = withShieldSpeed+0.5f;
            }
            else if(Input.GetButton("Sprint") && !anim.GetBool("RifleAimUp"))
            {
                speed = run;
            }
            else if(!_PlayerAnim.Block)
            {
                speed = walk;
            }
        }
        else
        {
            speed = 0;
        }
        
    }
    private void FixedUpdate()
    {
        if (PlayerHPManager.Instance.death)
        {
            return;
        }
        float Hor = Input.GetAxisRaw("Horizontal");
        float Ver = Input.GetAxisRaw("Vertical");

        MoveDir = new Vector3(Hor, 0, Ver);
        MoveDir = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * MoveDir;
        float magnitude = Mathf.Clamp01(MoveDir.magnitude) * speed;
        MoveDir.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (controller.isGrounded)
        {
            controller.stepOffset = orignalStepOffset;
            ySpeed = -0.5f;
            if (Input.GetButton("Jump") && !playerAttack_.attacking)
                ySpeed = jumpSpeed;

            

        }
        else
        {
            controller.stepOffset = 0;
        }
        velocity = MoveDir * magnitude;
        velocity = AdjustVelocityToSlope(velocity);
        velocity.y += ySpeed;

        calculate_angle();
        if (controller.enabled)
        controller.Move(velocity * Time.deltaTime);
        if (RotatePlayer && anim.GetBool("Move"))
        {
            transform.eulerAngles += new Vector3(0, Mathf.DeltaAngle(transform.eulerAngles.y, Camera.main.transform.eulerAngles.y + angle_to_rotete) * Time.deltaTime * rotation_speed, 0);
        }
     
    }
    public void addImpact(float force,float upForce)
    {
        col.enabled = true;
        controller.enabled = false;
        Rig.useGravity = true;
        Vector3 dir = -(GameObject.FindGameObjectWithTag("Boss").transform.position - transform.position);
        dir.y = 0;
        Rig.AddForce(dir.normalized  * force,ForceMode.Impulse);
        Rig.AddForce(transform.up * upForce);
        IsImpact = true;
        anim.SetBool("Ground", false);
    }
    Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray,out RaycastHit hitInfo,0.2f))
        {
            var slopeRot = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustVelocity = slopeRot * velocity;
            if(adjustVelocity.y < 0)
            {
                return adjustVelocity;
            }
        }
        return velocity;
    }
    void calculate_angle()
    {
        if (forward && !back)
        {
            if (left && !right)
                angle_to_rotete = -45;
            else if (!left && right)
                angle_to_rotete = 45;
            else
                angle_to_rotete = 0;
        }
        else if (!forward && back)
        {
            if (left && !right)
                angle_to_rotete = -135;
            else if (!left && right)
                angle_to_rotete = 135;
            else
                angle_to_rotete = 180;
        }
        else
        {
            if (left && !right)
                angle_to_rotete = -90;
            else if (right && !left)
                angle_to_rotete = 90;
            else
                angle_to_rotete = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        controller.enabled = true; 
        Rig.useGravity = false;
        col.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerHPManager.Instance.death)
            return;
        if(other.CompareTag("portal"))
        {
            StartCoroutine(teleport(0));
        }
        if (other.CompareTag("portal2"))
        {
            StartCoroutine(teleport(1));
        }
        if (other.CompareTag("BossSpawnCol"))
        {
            GameManager.SpawnBoss();
            other.gameObject.SetActive(false);
        }
    }
    IEnumerator teleport(int num)
    {
        WaitForSeconds wfs = new WaitForSeconds(0.7f);
            GameManager.instace.SpawnPoint = 1;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        controller.enabled = false;
        movetrue = false;
        if (num == 0)
        {
            GameManager.instace.PortalEffect.SetActive(true);
            yield return wfs;
            transform.position = GameManager.instace.TeleportPos;
            GameManager.instace.PortalEffect.SetActive(false);
            controller.enabled = true;
            movetrue = true;
            yield return null;
        }
        else
        {
            GameManager.instace.PortalEffect2.SetActive(true);
            yield return wfs;
            transform.position = GameManager.instace.TeleportPos2;
            GameManager.instace.PortalEffect2.SetActive(false);
            controller.enabled = true;
            movetrue = true;
            yield return null;
        }
    }
}
