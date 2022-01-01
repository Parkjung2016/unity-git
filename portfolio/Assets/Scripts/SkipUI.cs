using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkipUI : MonoBehaviour
{
    public Image ProgressBar;
    public GameObject SkipText;
    public Text SkipPercent;
    float fillamount;
    public GameManager GM_;
    void Start()
    {
        gameObject.SetActive(true);
        ProgressBar.gameObject.SetActive(false);

    }
    void Update()
    {
        SkipPercent.text = Mathf.Floor(ProgressBar.fillAmount * 100).ToString() + "%";
        if (Input.GetKey(KeyCode.Space))
        {
            fillamount += 2;
            ProgressBar.gameObject.SetActive(true);
            SkipText.SetActive(true);
            ProgressBar.fillAmount = fillamount / 100;

            if(ProgressBar.fillAmount == 1 && GM_.Timeline.time < 5.6f)
            {
                GM_.Timeline.time = 5.6f;
            }
        }
        else
        {
            ProgressBar.gameObject.SetActive(false);
            SkipText.SetActive(false);
            fillamount = 0;
        }
    }
}
