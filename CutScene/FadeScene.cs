using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FadeScene : MonoBehaviour
{
    [SerializeField]
    private Image padeout;
    [SerializeField]
    private float finalTime = 1.0f;

    private float time = 0.0f;

    public void FadeOut()
    {
        StartCoroutine(FadeOutFlow());
    }
    public void FadeIn()
    {
        StartCoroutine(FadeInFlow());
    }

    //화면을 점점 검게하여 페이드아웃
    IEnumerator FadeOutFlow()
    {
        time = 0;
        //검정으로 채운 ui 를 활성화 시킴
        padeout.gameObject.SetActive(true); 
        Color alpha = padeout.color;
        //알파값을 0에서 1로 점차 증가시키며 페이드 아웃 효과를 내준다.
        while(alpha.a < 1.0f)
        {

            time+= Time.deltaTime/finalTime;
            alpha.a = Mathf.Lerp(0 , 1, time);

            padeout.color = alpha;
            yield return null;
        }
        StopCoroutine(FadeOutFlow());
        yield return null;

    }
    IEnumerator FadeInFlow()
    {
        time = 0;
        Color alpha = padeout.color;
        //알파값을 1에서 0으로 감소시키며 페이드 인 효과를 내준다.
        while (alpha.a > 0.0f)
        {

            time += Time.deltaTime / finalTime;
            alpha.a = Mathf.Lerp(1, 0, time);

            padeout.color = alpha;
            yield return null;
        }
        padeout.gameObject.SetActive(false);
        StopCoroutine(FadeInFlow());
        yield return null;

    }


}
