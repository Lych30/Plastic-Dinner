using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
public class FadeInOut : MonoBehaviour
{
    public float seconds = 2.0f;
    public float min = 0.3f;
    public float max = 0.7f;
    public AnimationCurve fadeCurve;
    public Image image;

    private void Start()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float timer = 0.0f;

        while(true)
        {
            timer += Time.deltaTime;
            if(timer < seconds)
            {
                Color tempColor = image.color;
                float ratio = timer / seconds;
                tempColor.a = fadeCurve.Evaluate(Mathf.Lerp(min, max, ratio));
                image.color = tempColor;
            }
            else
            {
                float temp = min;
                min = max;
                max = temp;

                timer = 0.0f;
            }

            yield return null;
        }
    }
}
