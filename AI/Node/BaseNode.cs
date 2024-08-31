

//behaviortree를 사용하기위해서 이 이넘값을 리턴해야만 사용가능
public enum ReturnCode { FAILURE, SUCCESS, RUNNING };

//노드는 따로 스크립트로 만들 필요가 없기 때문에 기본클래스로 생성
public class BaseNode 
{
    // Start is called before the first frame update

    public virtual ReturnCode Tick()
    {
        return ReturnCode.SUCCESS;
    }


}
