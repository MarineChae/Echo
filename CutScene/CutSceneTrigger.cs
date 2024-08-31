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

    //�÷��̾ �ƽ��� ���۵Ǵ� ���� ���°��
    //�ڿ������� ������ ���� ���տ� �̵� �� �ٶ󺸴� ���⵵ �������ش�
    //�׷��߸� ������⿡�� ���� ��� �ƽ��� �ڿ������� �̾���
    public void FixedUpdateWork()
    {
        if (door.IsOpenInfo && !isTriggerd)
        {
            //�÷��̾��� ��ǲ�� �ߴ�
            movePlayer.CharacterStop = true;
            Vector3 moveDirection = movetarget.position - movePlayer.transform.position;
            //�÷��̾��� ���� �� ��ġ�� �̵�������
            movePlayer.transform.DOLookAt(new Vector3(300, 0, 0), 0.5f);
            characterCamera.transform.DOLookAt(new Vector3(300, 0, 0), 0.5f);
            movePlayer.transform.DOMove(movetarget.position, 0.5f);
            //Ʈ���� �̺�Ʈ�� �ѹ��� Ȱ��ȭ �� �� �ֵ���
            isTriggerd = true;
            //�޸��鼭 ���� ���°�� ���׹̳ʰ� ��� Ȱ��ȭ�Ǳ� ������ �����ϱ�����
            movePlayer.staminaUI.gameObject.SetActive(false);
            //�÷��̾ �̵� �� �ƽ��� �۵��ǵ�����
            Invoke("PlayCutScene", 0.5f);

        }
    }
    public void UpdateWork() { }
    public void LateUpdateWork() { }



    void PlayCutScene()
    {
        //����ī�޶��� �켱���� ���� ����ī�޶� �ƴ� ����ī�޶� ����Ͽ� �ƽ��� ���
        cutSceneCam.GetComponent<CinemachineVirtualCamera>().Priority = 11;
        cutSceneCam.SetActive(true);
        //������ �÷��̾�� ��� ��Ȱ��ȭ
        movePlayer.gameObject.SetActive(false);
        //�ƽ����
        playableDirector.Play();
    }
    public void FinishCutScene()
    {
        //ai�� �Ѹ�ġ�� �̷η� ���� �ƽ�
        playableDirector.playableAsset = timeineAssets[1];
        playableDirector.Play();
    }
    public void TrueEndingScene()
    {
        //������ �ƽ�
        playableDirector.playableAsset = timeineAssets[2];
        playableDirector.Play();

    }
    //�̷η� ĳ���Ͱ� ������ �� ������ �����ϱ����� 
    //��ġ�� ī�޶� ������ �������ش�.
    public void EnterMaze()
    {
        //�÷��̾� ��ǲ�� �ٽ� Ȱ��ȭ
        movePlayer.CharacterStop = false;
        //��ġ�� ������ ���Ͻ��� �ڿ������� ���ӽ����� ���� �ۼ�
        movePlayer.transform.position = startPos.transform.position;
        movePlayer.transform.DOLookAt(new Vector3(300, 0, 0), 0.5f);
        characterCamera.transform.DOLookAt(new Vector3(300, 0, 0), 0.5f);
        //����ī�޶��� �켱�� ���� �� ��Ȱ��ȭ
        cutSceneCam.GetComponent<CinemachineVirtualCamera>().Priority = 9;
        cutSceneCam.SetActive(false);
        //���� �÷��̾� Ȱ��ȭ
        movePlayer.gameObject.SetActive(true);
    }

    //������ ���� ���͸����� �����ϴ� �Լ� 
    //���� ����������� MaterialColorChange��ũ��Ʈ�� ���
    public void ChangeMaterial()
    {
        entranceDoor.GetComponent<MeshRenderer>().material = entranceMaterial;
    }
    
    public void OpenMainScene()
    {
        GameManager.Instance.SceneManager.LoadScene("MainMenu");
    }

}
