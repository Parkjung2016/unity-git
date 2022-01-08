using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public CanvasLookatCam[] ChestsCanvas;
    public float InteractionDis;
    bool HpPotionUseTrue;
    public int Hpincrease;
    void Start()
    {
        HpPotionUseTrue = true;
        ChestsCanvas = FindObjectsOfType<CanvasLookatCam>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CanvasLookatCam obj in ChestsCanvas)
        {
            if(Vector3.Distance(transform.position,obj.transform.position) <=InteractionDis)
            {
                if(!obj.Opened)
                {
                obj.gameObject.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        StartCoroutine(getHpPotion(obj,obj.transform.parent.GetComponentsInChildren<HpPotionCheck>()));
                    }

                }
                else
                {
                    obj.gameObject.SetActive(false);
                }
            }
            else
            {
                obj.gameObject.SetActive(false);

            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(GameManager.instace.HpInteractionKeyanim());
            if(GameManager.instace.HpPotionAmount > 0 && HpPotionUseTrue && PlayerHPManager.Instance.HP < PlayerHPManager.Instance.maxHP)
            {
                GameManager.instace.AddHPPotion(1, false);
                HpPotionUseTrue = false;
                StartCoroutine( resetHppotionUse());
                PlayerHPManager.Instance.HP += Hpincrease;
            }
        }
    }
    IEnumerator resetHppotionUse()
    {
        yield return new WaitForSeconds(0.5f);
        HpPotionUseTrue = true;
    }
    IEnumerator getHpPotion(CanvasLookatCam obj,HpPotionCheck[] Potions)
    {
        obj.GetComponentInParent<Animation>().Play();
        obj.Opened = true;
        yield return new WaitForSeconds(1.5f);
        foreach (HpPotionCheck  potion in Potions)
        {
            potion.GetComponent<Animation>().Play();
        }
        GameManager.instace.AddHPPotion(Potions.Length,true);

    }
}
