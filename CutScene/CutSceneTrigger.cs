using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneTrigger : MonoBehaviour, IUpdateable
{
    private Camera characterCamera;
    private PlayableDirector playableDirector;
    private bool isTriggerd;
    [SerializeField]
    private MovePlayer movePlayer;
    [SerializeField]
    private Transform movetarget;
    [SerializeField]
    private Door door;
    [SerializeField]
    private GameObject entranceDoor;
    [SerializeField]
    private Material entranceMaterial;
    [SerializeField]
    private TimelineAsset[] timeineAssets;
    [SerializeField]
    private GameObject cutSceneCam;
    [SerializeField]
    private GameObject startPos;

    private void Awake()
    {
        door = GetComponent<Door>();
        isTriggerd = false;
        characterCamera = Camera.main;
        playableDirector = GetComponent<PlayableDirector>();
        cutSceneCam.SetActive(false);
    }

    private void Start()
    {
        playableDirector.playableAsset = timeineAssets[0];
    }
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, false, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.OffSubscribe(this, false, true, false);
    }

    //플레이어가 컷신이 시작되는 문을 여는경우
    //자연스러운 연출을 위해 문앞에 이동 후 바라보는 방향도 변경해준다
    //그래야만 어느방향에서 문을 열어도 컷신이 자연스럽게 이어짐
    public void FixedUpdateWork()
    {
        if (door.IsOpenInfo && !isTriggerd)
        {
            //플레이어의 인풋을 중단
            movePlayer.CharacterStop = true;
            Vector3 moveDirection = movetarget.position - movePlayer.transform.position;
            //플레이어의 방향 및 위치를 이동시켜줌
            movePlayer.transform.DOLookAt(new Vector3(300, 0, 0), 0.5f);
            characterCamera.transform.DOLookAt(new Vector3(300, 0, 0), 0.5f);
            movePlayer.transform.DOMove(movetarget.position, 0.5f);
            //트리거 이벤트가 한번만 활성화 될 수 있도록
            isTriggerd = true;
            //달리면서 문을 여는경우 스테미너가 계속 활성화되기 때문에 방지하기위함
            movePlayer.staminaUI.gameObject.SetActive(false);
            //플레이어가 이동 후 컷신이 작동되도록함
            Invoke("PlayCutScene", 0.5f);

        }
    }
    public void UpdateWork() { }
    public void LateUpdateWork() { }



    void PlayCutScene()
    {
        //가상카메라의 우선도를 높혀 메인카메라가 아닌 가상카메라를 사용하여 컷신을 재생
        cutSceneCam.GetComponent<CinemachineVirtualCamera>().Priority = 11;
        cutSceneCam.SetActive(true);
        //기존의 플레이어는 잠시 비활성화
        movePlayer.gameObject.SetActive(false);
        //컷신재생
        playableDirector.Play();
    }
    public void FinishCutScene()
    {
        //ai를 뿌리치고 미로로 들어가는 컷신
        playableDirector.playableAsset = timeineAssets[1];
        playableDirector.Play();
    }
    public void TrueEndingScene()
    {
        //진엔딩 컷신
        playableDirector.playableAsset = timeineAssets[2];
        playableDirector.Play();

    }
    //미로로 캐릭터가 진입한 후 게임을 시작하기위해 
    //위치와 카메라 방향을 조절해준다.
    public void EnterMaze()
    {
        //플레이어 인풋을 다시 활성화
        movePlayer.CharacterStop = false;
        //위치와 방향을 통일시켜 자연스럽게 게임시작을 위해 작성
        movePlayer.transform.position = startPos.transform.position;
        movePlayer.transform.DOLookAt(new Vector3(300, 0, 0), 0.5f);
        characterCamera.transform.DOLookAt(new Vector3(300, 0, 0), 0.5f);
        //가상카메라의 우선도 낮춤 및 비활성화
        cutSceneCam.GetComponent<CinemachineVirtualCamera>().Priority = 9;
        cutSceneCam.SetActive(false);
        //기존 플레이어 활성화
        movePlayer.gameObject.SetActive(true);
    }

    //진입한 문의 메터리얼을 변경하는 함수 
    //현재 사용하지않음 MaterialColorChange스크립트를 사용
    public void ChangeMaterial()
    {
        entranceDoor.GetComponent<MeshRenderer>().material = entranceMaterial;
    }
    
    public void OpenMainScene()
    {
        GameManager.Instance.SceneManager.LoadScene("MainMenu");
    }

}
