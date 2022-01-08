using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRifleEquip : MonoBehaviour
{
    public Transform Rifle, Rifle_ueq, Rifle_eq;
    public bool Rifle_is_equipped;
    PlayerAttack playerAttack_;
	Animator anim;
	public MeshRenderer RifleMeshRen;
	public bool RifleOn;
	public float RifleOnSpeed;
	public  bool RigBuilderTrue;
	public RigBuilder rigbuilder;
	public GameObject RifleAimUpRig;
	public GameObject RifleAimUpWalkRig;
	public GameObject RifleFiringRig;
	public GameObject RifleAimUpStrafeLRig;
	public GameObject RifleAimUpStrafeRRig;
	public GameObject RifleAimUpStrafeBRig;
	public GameObject RifleAimDownRig;
	public GameObject RifleAimRig;
	public GameObject L_Hand_Grip;
	public GameObject L_Hand_GripAimUpRig;
	public GameObject L_Hand_GripFiringRig;
	public GameObject L_Hand_GripAimUpWalkRig;
	public GameObject L_Hand_GripAimUpStrafeLRig;
	public GameObject L_Hand_GripAimUpStrafeRRig;
	public GameObject L_Hand_GripAimUpStrafeBRig;
	public GameObject L_Hand_GripAimDownRig;
	public GameObject R_Hand_Grip;
	public GameObject R_Hand_GripAimUpRig;
	public GameObject R_Hand_GripAimDownRig;
	public GameObject Cam_Rifle;
	public GameObject Cam;
	public GameObject CrossHair;
	public bool Aim;
	private PlayerRifleAttack playerRifleAttack;
	private void Awake()
    {
		playerRifleAttack = GetComponent<PlayerRifleAttack>();
		playerAttack_ = GetComponent<PlayerAttack>();
		anim = GetComponent<Animator>();
		RifleMeshRen.material.SetColor("_EmissionColor", Color.black);
    }
    private void Start()
    {
		RigBuilderTrue = false;
		Cam_Rifle.SetActive(false);
	}
    void Update()
	{

		if (PlayerHPManager.Instance.death)
		{
			Aim = false;
			CrossHair.SetActive(false);
			Cam.SetActive(true);
			Cam_Rifle.SetActive(false);
			rigbuilder.layers[1].active = false;
			return;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && !playerAttack_.attacking)
		{
			if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Wait") || anim.GetCurrentAnimatorStateInfo(1).IsTag("Wait")
				|| anim.GetCurrentAnimatorStateInfo(2).IsTag("Wait"))
            {
				return;
            }
			if (anim.GetBool("Sword"))
            {
				anim.SetTrigger("sword_i");
				anim.SetBool("Sword", !anim.GetBool("Sword"));
			}
			if(anim.GetBool("Shield"))
            {

				anim.SetTrigger("Shield_i");
				return;
            }
            else
            {
				Aim = false;
				CrossHair.SetActive(false) ;
				Cam.SetActive(true);
				Cam_Rifle.SetActive(false);
				rigbuilder.layers[1].active = false;
				anim.SetTrigger("Rifle_i");
                return;
            }
		}

        rigbuilder.enabled = RigBuilderTrue && !playerRifleAttack.reloading;
		if (Rifle_is_equipped)
		{
			anim.SetBool("Block", false);
			Rifle.position = Rifle_eq.position;
			Rifle.rotation = Rifle_eq.rotation;
            anim.SetBool("RifleAimUp", Input.GetMouseButton(1));
            if (Aim)
			{
				CrossHair.SetActive(true);
				Cam.SetActive(false);
				Cam_Rifle.SetActive(true);
				rigbuilder.layers[1].active = Input.GetMouseButton(1) || playerRifleAttack.Firing;

				if (Input.GetMouseButton(1) || playerRifleAttack.Firing)
				{
					transform.rotation = Quaternion.Euler(new Vector3(0, Camera.main.transform.eulerAngles.y, 0));
				}
			}

			if (!anim.GetBool("Move"))
			{
				if (playerRifleAttack.Firing)
				{
					L_Hand_Grip.transform.SetPositionAndRotation(L_Hand_GripFiringRig.transform.position, L_Hand_GripFiringRig.transform.rotation);
					RifleAimRig.transform.SetPositionAndRotation(RifleFiringRig.transform.position, RifleFiringRig.transform.rotation);
				}
				else
                {

				RifleAimRig.transform.SetPositionAndRotation(Input.GetMouseButton(1) ? RifleAimUpRig.transform.position : RifleAimDownRig.transform.position, Input.GetMouseButton(1) ? RifleAimUpRig.transform.rotation : RifleAimDownRig.transform.rotation);
				L_Hand_Grip.transform.SetPositionAndRotation(Input.GetMouseButton(1) ? L_Hand_GripAimUpRig.transform.position : L_Hand_GripAimDownRig.transform.position, Input.GetMouseButton(1) ? L_Hand_GripAimUpRig.transform.rotation : L_Hand_GripAimDownRig.transform.rotation);
                }
			}
			else
			{
				if(playerRifleAttack.Firing)
                {
					L_Hand_Grip.transform.SetPositionAndRotation(L_Hand_GripFiringRig.transform.position,L_Hand_GripFiringRig.transform.rotation);
					RifleAimRig.transform.SetPositionAndRotation(RifleFiringRig.transform.position,RifleFiringRig.transform.rotation);
				}
				else if (anim.GetBool("Left") || anim.GetBool("Left") && anim.GetBool("Right") || anim.GetBool("Left") && anim.GetBool("Back"))
				{
					RifleAimRig.transform.SetPositionAndRotation(RifleAimUpStrafeLRig.transform.position, RifleAimUpStrafeLRig.transform.rotation);
					L_Hand_Grip.transform.SetPositionAndRotation(L_Hand_GripAimUpStrafeLRig.transform.position, L_Hand_GripAimUpStrafeLRig.transform.rotation);

				}
				else if (anim.GetBool("Right") || anim.GetBool("Right") && anim.GetBool("Back") || anim.GetBool("Right") && anim.GetBool("Left"))
				{

					RifleAimRig.transform.SetPositionAndRotation(RifleAimUpStrafeRRig.transform.position, RifleAimUpStrafeRRig.transform.rotation);
					L_Hand_Grip.transform.SetPositionAndRotation(L_Hand_GripAimUpStrafeRRig.transform.position, L_Hand_GripAimUpStrafeRRig.transform.rotation);
				}
				else if (anim.GetBool("Back") || anim.GetBool("Back") && anim.GetBool("Left") || anim.GetBool("Back") && anim.GetBool("Right"))
				{

					RifleAimRig.transform.SetPositionAndRotation(RifleAimUpStrafeBRig.transform.position, RifleAimUpStrafeBRig.transform.rotation);
					L_Hand_Grip.transform.SetPositionAndRotation(L_Hand_GripAimUpStrafeBRig.transform.position, L_Hand_GripAimUpStrafeBRig.transform.rotation);
				}
				else
				{
					L_Hand_Grip.transform.SetPositionAndRotation(Input.GetMouseButton(1) ? L_Hand_GripAimUpWalkRig.transform.position : L_Hand_GripAimDownRig.transform.position, Input.GetMouseButton(1) ? L_Hand_GripAimUpWalkRig.transform.rotation : L_Hand_GripAimDownRig.transform.rotation);
					RifleAimRig.transform.SetPositionAndRotation(Input.GetMouseButton(1) ? RifleAimUpWalkRig.transform.position : RifleAimDownRig.transform.position, Input.GetMouseButton(1) ? RifleAimUpWalkRig.transform.rotation : RifleAimDownRig.transform.rotation);
				}
			}
			if (playerRifleAttack.Firing)
				R_Hand_Grip.transform.SetPositionAndRotation(R_Hand_GripAimUpRig.transform.position, R_Hand_GripAimUpRig.transform.rotation);
			else
				R_Hand_Grip.transform.SetPositionAndRotation(Input.GetMouseButton(1) ? R_Hand_GripAimUpRig.transform.position : R_Hand_GripAimDownRig.transform.position, Input.GetMouseButton(1) ? R_Hand_GripAimUpRig.transform.rotation : R_Hand_GripAimDownRig.transform.rotation);
		}
		else
		{
			Rifle.position = Rifle_ueq.position;
			Rifle.rotation = Rifle_ueq.rotation;
		}
		if (RifleOn)
		{
			RifleOnSpeed = 1;
			if (RifleMeshRen.material.GetColor("_EmissionColor") == Color.white)
				return;
			RifleMeshRen.material.SetColor("_EmissionColor", Color.Lerp(RifleMeshRen.material.GetColor("_EmissionColor"), Color.white, RifleOnSpeed * Time.deltaTime));
		}
		else
		{
			RifleOnSpeed = 4;
			if (RifleMeshRen.material.GetColor("_EmissionColor") == Color.black)
				return;
				RifleMeshRen.material.SetColor("_EmissionColor", Color.Lerp(RifleMeshRen.material.GetColor("_EmissionColor"), Color.black, RifleOnSpeed * Time.deltaTime));
		}
	}
	public void Rifle_Equip()
	{
		Aim = true;
		Rifle_is_equipped = true;
		RifleOn = true;
		anim.ResetTrigger("Rifle_i");
	}
	public void RigBuilder()
    {
		RigBuilderTrue = !RigBuilderTrue;
    }
	public void Rifle_Unequiped()
	{
		Rifle_is_equipped = false;
		RifleOn = false;
		anim.ResetTrigger("Rifle_i");

	}
}
