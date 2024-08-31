
using UnityEngine;
using System.Collections;
public class MaterialColorChange : MonoBehaviour
{

    [SerializeField]
    private MeshRenderer meshRenderer;
    private Material material;
    private readonly float colorCurve = -1.0f;
    private Color color;
    private void Start()
    {
        material = meshRenderer.material;
        //메터리얼의 컬러중 emission을 사용
        color = material.GetColor("_EmissionColor");
    }

    public void ChangeMaterialColor()
    {
  
        StartCoroutine("ChangeMat");

    }
    IEnumerator ChangeMat()
    {
        //기존의 색에서 R값을 점차 뺴주며 회색 문으로 바꾸어줌
        while (color.r > 0)
        {
       
            color.r = color.r + (colorCurve * Time.deltaTime);

            material.SetColor("_EmissionColor", color);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopCoroutine("ChangeMat");
        yield return null;
    }

}
