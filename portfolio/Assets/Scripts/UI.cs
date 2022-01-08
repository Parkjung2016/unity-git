using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Image PlayerHP;
    public Image PlayerBlock;
    public float LerpSpeed;
    public GameObject UICanavas;
    public GameObject BossHPObj;
    public Image BossHP;
    public Animation QuestAnim;
    bool isfade;
    public Text BulletText;
    public Text HPPotionAmountText;
    public Text HpInteractionKey;
    public GameObject Pause;
    private void Start()
    {
        Pause.SetActive(false);
        isfade = false;
        QuestAnim.gameObject.SetActive(false);
    }
    void Update()
    {
        PlayerHP.fillAmount = Mathf.Lerp(PlayerHP.fillAmount, (float)PlayerHPManager.Instance.HP / (float)PlayerHPManager.Instance.maxHP, Time.deltaTime * LerpSpeed);
        PlayerBlock.fillAmount = Mathf.Lerp(PlayerBlock.fillAmount, (float)PlayerBlockManager.Instance.Block / (float)PlayerBlockManager.Instance.MaxBlock, Time.deltaTime * LerpSpeed);
        if (BossHPManager.Instance == null)
            return;
        BossHP.fillAmount = Mathf.Lerp(BossHP.fillAmount, (float)BossHPManager.Instance.Hp / (float)BossHPManager.Instance.MaxHp, Time.deltaTime * LerpSpeed);
    }
    public void SetHPPotionAmountText()
    {
        HPPotionAmountText.text = GameManager.instace.HpPotionAmount.ToString();
    }
    public void FadeQuestAnim()
    {
        if (isfade)
            return;
        QuestAnim.gameObject.SetActive(true);
        QuestAnim.Play();
        isfade = true;
    }
    public void SetBulletText(int CurrentBullet,int MaxBullet)
    {
        BulletText.text = "Åº¾Ë : " + CurrentBullet + "/" + MaxBullet;
    }
}
