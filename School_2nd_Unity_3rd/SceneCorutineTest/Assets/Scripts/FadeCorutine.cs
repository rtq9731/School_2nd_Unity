using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Text;

public enum eFadeState
{
    None,
    FadeOut,
    ChangeBg,
    FadeIn,
    Done
}

public class FadeCorutine : MonoBehaviour
{

    public float fadeInEffectTime = 1;
    public float fadeOutEffectTime = 1;

    Image imgBg;

    eFadeState fadeState;

    IEnumerator iStartCo = null;

    StringBuilder sb;
    private void Awake()
    {
        imgBg = this.gameObject.GetComponent<Image>();
        if (imgBg == null)
            Debug.LogError("img is Null");

        if (fadeInEffectTime == 0)
        {
            fadeInEffectTime = 1;
            Debug.Log("페이드 인에 걸리는 시간이 0일 수 없습니다!");
        }
        if (fadeOutEffectTime == 0)
        {
            fadeOutEffectTime = 1;
            Debug.Log("페이드 아웃에 걸리는 시간이 0일 수 없습니다!");
        }

        sb = new StringBuilder();

    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && fadeState == eFadeState.None)
        {
            fadeState = eFadeState.None;
            NextState();
        }
    }


    IEnumerator NoneState()
    {
        while(fadeState == eFadeState.None)
        {
            fadeState = eFadeState.FadeOut;
            yield return null;
        }

        NextState();
    }

    IEnumerator FadeOutState()
    {
        float alpha = 0f;

        while (fadeState == eFadeState.FadeOut)
        {
            if (alpha < 1)
            {
                alpha += Time.deltaTime / fadeOutEffectTime;
            }
            else
            {
                fadeState = eFadeState.ChangeBg;
            }

            alpha = Mathf.Clamp(alpha, 0, 1);
            imgBg.color = new Color(imgBg.color.r, imgBg.color.g, imgBg.color.b, alpha);
            yield return null;
        }

        NextState();
    }

    IEnumerator ChangeBgState()
    {
        Debug.Log("리소스 로드 단계"); 
        yield return null;

        fadeState = eFadeState.FadeIn;

        NextState();
    }

    IEnumerator FadeInState()
    {

        float alpha = 1f;

        while (fadeState == eFadeState.FadeIn)
        {
            if (alpha > 0)
            {
                alpha -= Time.deltaTime / fadeInEffectTime;
            }
            else
            {
                fadeState = eFadeState.Done;
            }

            alpha = Mathf.Clamp(alpha, 0, 1);
            imgBg.color = new Color(imgBg.color.r, imgBg.color.g, imgBg.color.b, alpha);
            yield return null;
        }

        NextState();
    }

    IEnumerator DoneState()
    {
        fadeState = eFadeState.None;

        Debug.Log("화면 전환 종료");

        yield return null;
    }

    void NextState()
    {
        sb.Remove(0, sb.Length);
        sb.Append(fadeState.ToString());
        sb.Append("State");


        MethodInfo mInfo = this.GetType().GetMethod(sb.ToString(), BindingFlags.Instance | BindingFlags.NonPublic);
        iStartCo = (IEnumerator)mInfo.Invoke(this, null);
        StartCoroutine(iStartCo);
    }
}
