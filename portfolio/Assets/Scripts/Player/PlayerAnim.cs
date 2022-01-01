using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Animator anim;
    CharacterController con;
    public bool roll;
    public LayerMask _fieldLayer;
    public float maxDistance;
    PlayerAttack playerAttack_;
    PlayerMove PlayerMove_;
    public bool Block;

    private void Awake()
    {
        PlayerMove_ = GetComponent<PlayerMove>();
        playerAttack_ = GetComponent<PlayerAttack>();
        con = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        anim.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHPManager.Instance.death)
        {
            return;
        }
        Block = Input.GetButton("Block");
        anim.SetBool("Block", Block);
        anim.SetBool("Run", Input.GetButton("Sprint") && !Block);
        anim.SetBool("Move", Mathf.Abs(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).magnitude) > 0 && PlayerMove_.movetrue);
        if (!roll && !PlayerMove_.IsImpact)
            anim.SetBool("Ground", IsCheckGrounded());

        if (Input.GetButtonDown("Roll"))
        {
            playerAttack_.ComboReset();
            anim.applyRootMotion = true;
            roll = true;
            anim.SetBool("Ground", true);
            anim.SetTrigger("Roll");
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Roll") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            anim.ResetTrigger("Roll");
            anim.applyRootMotion = false;
            roll = false;
        }
    }

    private bool IsCheckGrounded()
    {
        bool check = false;
        if (con.enabled)
        {

            // CharacterController.IsGrounded�� true��� Raycast�� ������� �ʰ� ���� ����
            if (con.isGrounded) check = true;
        }
        else
        {
            // �߻��ϴ� ������ �ʱ� ��ġ�� ����
            // �ణ ��ü�� ���� �ִ� ��ġ�κ��� �߻����� ������ ����� ������ �� ���� ���� �ִ�.
            var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
            // Ž�� �Ÿ�
            Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
            // Raycast�� hit ���η� ����
            // ���󿡸� �浹�� ���̾ ����
            check = Physics.Raycast(ray, maxDistance, _fieldLayer);
        }
            return check;
    }
    private void OnCollisionEnter(Collision collision)
    {
            if (PlayerMove_.IsImpact)
            {
            anim.applyRootMotion = true;
                anim.SetTrigger("Knock");
                PlayerMove_.IsImpact = false;
            }
    }
    public void rootmotionfalse()
    {
        anim.applyRootMotion = false;
    }
}