using UnityEngine;

public class CutScenePlayer : MonoBehaviour, IUpdateable
{
    [SerializeField]
    private PoolingType PlayerEcho;
    private bool useEco = true;
    private float time;

    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.OffSubscribe(this, true, false, false);
    }

    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        time += Time.deltaTime;
        if (time > 1.0f && useEco)
        {
            time = 0.0f;
            var t1 = GameManager.Instance.ObjectPool.Get(PlayerEcho);
            t1.Activate(transform);
            GameManager.Instance.ObjectPool.Return(t1);
        }

    }
    public void LateUpdateWork() { }


    public void ChangeEco()
    {
        if(PlayerEcho == PoolingType.PlayerEcho)
        {
            PlayerEcho = PoolingType.NPCEcho;
        }
        else
        {
            PlayerEcho = PoolingType.PlayerEcho;
        }

      
    }
    public void ChangeEcoState()
    {
        useEco = !useEco;
    }
    public void FootSound()
    {

        GameManager.Instance.SoundManager.PlaySound3D("Sound/walk", transform, SoundType.SFX, 0.2f);

    }
    public void PunchSound()
    {

        GameManager.Instance.SoundManager.PlaySound3D("Sound/Punch", transform, SoundType.SFX, 0.2f);

    }
}
