
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
        //���͸����� �÷��� emission�� ���
        color = material.GetColor("_EmissionColor");
    }

    public void ChangeMaterialColor()
    {
  
        StartCoroutine("ChangeMat");

    }
    IEnumerator ChangeMat()
    {
        //������ ������ R���� ���� ���ָ� ȸ�� ������ �ٲپ���
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
