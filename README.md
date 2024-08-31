# Ehco

* 제작기간 : 2024.07.01 ~ 2024.08.08

* 개발환경 : Unity , C#

* DownloadLink : https://store.steampowered.com/app/3132180/Echo/
  
![Alt text](readmeimg/EcoBackgroundImage03.jpg)
![Alt text](readmeimg/EcoBackgroundImage04.jpg)



# Contribution

* AI
* CinemaScene
  

# CinemaScene





# AI

 
<details>
<summary>BehaviorTree코드샘플</summary>
```c#
public class BehaviorTree : MonoBehaviour
{
    //트리의 루트 노드는 항상 브런치노드에서 파생 되어야함
    public BranchNode rootNode;
    private bool isRun = true;
    public void RunTree()
    {
        if(isRun)
        rootNode.Tick();
    }

    public void ChangeTreeState()
    {
        rootNode.currentChild = 0;
        isRun = !isRun;
    }
    public bool GetRunState()
    {
        return isRun;
    }

}

```
</details>



플레이어가 가까운 거리내에 위치한다면 플레이어를 잡아 게임오버시켜줌

플레이어가 시야범위 내에 위치해 있는경우 플레이어를 추격

시야범위 내에 없다면 랜덤한 위치를 순찰





