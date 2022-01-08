using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    public int HP;
    public Animator anim;
    public int maxHP;
    public bool death;
    public GameObject Sword;
    public GameObject Rifle;
    public GameObject Shield;
    PlayerAnim playerAnim_;
    public static  PlayerHPManager Instance = null;
    private void Awake()
    {
        playerAnim_ = GetComponent<PlayerAnim>();
        anim = GetComponent<Animator>();
        if(Instance == null)
        {
            Instance = this;
        }
    }
    
    
    private void Update()
    {
        HP = Mathf.Clamp(HP,0, maxHP);
        if (Input.GetKeyDown(KeyCode.H))
            HP = 0;
        if(HP == 0)
        {
            if (death)
                return;
            Sword.transform.parent = null;
            Destroy(Sword.GetComponent<BoxCollider>());
            Sword.GetComponent<MeshCollider>().enabled = true;
            Sword.AddComponent<Rigidbody>();
            anim.Play("Death");
            anim.Play("None", 1);
            anim.Play("None", 2);
            anim.Play("None", 3);
            anim.Play("None", 4);
            anim.SetBool("Sword", false);
            anim.SetBool("RifleAimUp", false);
            Rifle.transform.parent = null;
            Rifle.GetComponent<MeshCollider>().enabled = true;
            Rifle.AddComponent<Rigidbody>();
            Shield.transform.parent = null;
            Shield.GetComponent<MeshCollider>().enabled = true;
            Shield.AddComponent<Rigidbody>();
            anim.applyRootMotion = true;
            GameManager.instace.DeathBgAnim.Play();
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            GetComponent<PlayerRifleEquip>().RigBuilderTrue = false;
            GetComponent<PlayerRifleEquip>().rigbuilder.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            death = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Sword.GetComponent<MeshCollider>().enabled = false;
        Rifle.GetComponent<MeshCollider>().enabled = false;
        HP = 100;
        maxHP = HP;
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }

    public static void ApplyDamage(int Damage,bool skill)
    {
        if (Instance.anim.GetCurrentAnimatorStateInfo(0).IsTag("Roll") || Instance.anim.GetCurrentAnimatorStateInfo(0).IsTag("Knock"))
            return;
        if (skill)
        {
            Instance.HP -= Damage;
        return;

        }
            Instance.anim.applyRootMotion = true;
            if (Instance.playerAnim_.Block && PlayerBlockManager.Instance.Block != 0)
            {
                Instance.anim.Play("Block");
                PlayerBlockManager.ApplyBlockDamage(Damage);
            }
            else
            {
                Instance.anim.Play("Hit");
                Instance.HP -= Damage;
            }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag( "bossskill"))
        {
            StartCoroutine(skilldam());
        }
    }
   
    IEnumerator skilldam()
    {
        yield return new WaitForSeconds(2);
        ApplyDamage(1, true);
    }

}
