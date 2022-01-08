using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnClickSFX : MonoBehaviour
{
    public AudioSource btnclicksound;
    public void BtnClick()
    {
        btnclicksound.Play();
    }
}
