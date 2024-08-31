using System.Collections.Generic;

//시퀀스 노드는 시퀀스의 자식노드를 전부 실행
//자식이 실패한경우 이 노드는 전부 실패처리
public class SequenceNode : BranchNode
{
    public override ReturnCode Tick()
    {
        int icur = currentChild;
        int iListSize = childList.Count;

        for (int iSize = icur; iSize < iListSize; ++iSize)
        {
            ReturnCode State = childList[iSize].Tick();

            currentChild = iSize;
            //이번 틱에서 액션이 성공했고 끝나지 않았다면 
            //다음 틱에서 이어서 실행하기위함
            if (State == ReturnCode.RUNNING)
            {
                return ReturnCode.RUNNING;
            }
            //시퀀스 자식의 컨디션체크가 실패 한 경우 시퀀스를 실패처리
            //다음에 시퀀스에 들어오면 0번 자식부터 실행하기 위함
            else if (State == ReturnCode.FAILURE)
            {
                currentChild = 0;
                return ReturnCode.FAILURE;
            }

        }
        currentChild = 0;
        return ReturnCode.SUCCESS;
    }






}
