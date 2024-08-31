using UnityEngine;

public class CutSceneDoor : Door
{
    float delayTime = 0;

    //������ ���� �ٸ��� �ƽſ��� ����� ���� �ѹ��� ����� ���̰�
    //���� ������ Ÿ�̹��� �����ϱ�����
    void Update()
    {
        if(IsOpenInfo)
        {
            delayTime += Time.deltaTime;
        }
        if (IsOpenInfo && delayTime >= 0.5f)
        {

            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, doorTime * Time.deltaTime);
        }

    }





}
