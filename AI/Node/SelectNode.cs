

//셀렉트 노드는 한곳에서 성공했다면 그자리에서 실행 종료
//실패한 경우 다음 노드실행을 반복한다
public class SelectNode : BranchNode
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
            //셀렉터의 자식이 성공적으로 끝났다면 다음 틱에서 셀렉터의 첫 번째 부터 시작
            else if (State == ReturnCode.SUCCESS)
            {
                currentChild = 0;
                return ReturnCode.SUCCESS;
            }

        }

        currentChild = 0;
        return ReturnCode.FAILURE;
    }
}
