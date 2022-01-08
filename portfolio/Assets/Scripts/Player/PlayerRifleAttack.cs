using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRifleAttack : MonoBehaviour
{
    [SerializeField]
    public bool RifleAttackTrue;
    private Animator anim;
    public float FireRate;
    public int maxBullet;
    public int currentBullets;
    private float FireTimer;
    public bool reloading;
    public UI uI;
    public bool Firing;
    public GameObject ReloadParticle;
    public float range;
    public int Damage;
    public TrailRenderer Trail;
    public Transform TrailSpawnTrans;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        ReloadParticle.SetActive(false);
        currentBullets = maxBullet;
        RifleAttackTrue = false;
        uI.SetBulletText(currentBullets, maxBullet);
    }
    // Update is called once per frame
    private void Update()
    {
        if (PlayerHPManager.Instance.death || BossHPManager.Instance != null && BossHPManager.Instance.Death)
            return;
        if (RifleAttackTrue)
        {
            if (Input.GetMouseButton(0))
            {
                if (currentBullets > 0 && !reloading)
                    Fire();
                else
                {
                    Reload();
                }
            }
            if (FireTimer < FireRate)
                FireTimer += Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
        Firing = RifleAttackTrue && Input.GetMouseButton(0) && !reloading;
    }
    public void Reload()
    {
        if (!reloading && currentBullets != maxBullet)
        {

            anim.ResetTrigger("Fire");
            ReloadParticle.SetActive(true);
            anim.PlayInFixedTime("Reload", 4, 0.1f);
            reloading = true;
        }
    }
    private void Fire()
    {
        if (PlayerHPManager.Instance.death || BossHPManager.Instance != null && BossHPManager.Instance.Death)
            return;
        if (FireTimer < FireRate)
            return;
        RaycastHit hit;
        var obj = Instantiate(Trail, TrailSpawnTrans.position, Quaternion.identity);
        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit,Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Boss"))
            {
                if (BossHPManager.Instance.Death)
                    return;
                GameManager.instace.StartCoroutine(GameManager.CameraShake(GetComponentInParent<Animator>(), hit.collider.GetComponent<Animator>(),.5f));
                BossHPManager.ApplyDamage(Damage);
            }
            obj.transform.position = hit.point;
        }
        obj.AddPosition(TrailSpawnTrans.position);
        currentBullets--;
        FireTimer = 0;
        anim.SetTrigger("Fire");
        uI.SetBulletText(currentBullets, maxBullet);
    }
    public void RifleAttackChange()
    {
        RifleAttackTrue = !RifleAttackTrue;
    }
    public void ReloadBullet()
    {
        if (PlayerHPManager.Instance.death)
            return;
        currentBullets = maxBullet;
        uI.SetBulletText(currentBullets, maxBullet);
        ReloadParticle.SetActive(false);
        reloading = false;
    }
}
