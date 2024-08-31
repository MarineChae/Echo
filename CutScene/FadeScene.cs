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

    //ȭ���� ���� �˰��Ͽ� ���̵�ƿ�
    IEnumerator FadeOutFlow()
    {
        time = 0;
        //�������� ä�� ui �� Ȱ��ȭ ��Ŵ
        padeout.gameObject.SetActive(true); 
        Color alpha = padeout.color;
        //���İ��� 0���� 1�� ���� ������Ű�� ���̵� �ƿ� ȿ���� ���ش�.
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
        //���İ��� 1���� 0���� ���ҽ�Ű�� ���̵� �� ȿ���� ���ش�.
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
