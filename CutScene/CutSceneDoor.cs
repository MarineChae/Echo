using UnityEngine;

public class CutSceneDoor : Door
{
    float delayTime = 0;

    //기존의 문과 다르게 컷신에서 사용할 문은 한번만 사용할 것이고
    //문이 열리는 타이밍을 조절하기위함
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
