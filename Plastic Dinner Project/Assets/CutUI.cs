using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutUI : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public Animator animator;
    AudioSource[] knifeAudio;

    void Start()
    {
        knifeAudio = GetComponents<AudioSource>();
    }

    public void PlayIntro()
    {
        animator.Play("IntroKnifeAnim");
    }
    public void CutSwitch()
    {
        if (image1.enabled)
        {
            image1.enabled = false;
            image2.enabled = true;
        }
        else
        {
            image2.enabled = false;
            image1.enabled = true;
        }
        PlayAudio();
    }

    void PlayAudio()
    {
        knifeAudio[Random.Range(0, knifeAudio.Length)].Play();
    }
}
