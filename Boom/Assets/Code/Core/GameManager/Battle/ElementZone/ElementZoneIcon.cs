using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementZoneIcon:MonoBehaviour
{
    [Header("依赖资产")]
    public Image IconIce;
    public Image IconFire;
    public Image IconThunder;
    public TextMeshProUGUI txtElementalInfusionValue;
    
    [Header("数据")]
    [SerializeField] ElementalTypes _ElementalType;
    [SerializeField] int ElementalInfusionValue;

    public void InitData(ElementalTypes _type)
    {
        _ElementalType = _type;
        switch (_ElementalType)
        {
            case ElementalTypes.Ice:
                IconIce.gameObject.SetActive(true);
                IconFire.gameObject.SetActive(false);
                IconThunder.gameObject.SetActive(false);
                break;
            case ElementalTypes.Fire:
                IconIce.gameObject.SetActive(false);
                IconFire.gameObject.SetActive(true);
                IconThunder.gameObject.SetActive(false);
                break;
            case ElementalTypes.Thunder:
                IconIce.gameObject.SetActive(false);
                IconFire.gameObject.SetActive(false);
                IconThunder.gameObject.SetActive(true);
                break;
        }
        ElementalInfusionValue = 0;
        txtElementalInfusionValue.text = ElementalInfusionValue.ToString();
    }

    public void AddValue(int value)
    {
        ElementalInfusionValue += value;
        txtElementalInfusionValue.text = ElementalInfusionValue.ToString();
    }
    
    public void AnimateEnter()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    public IEnumerator AnimateReactAndFade()
    {
        // 发光/震动
        transform.DOShakePosition(0.2f, strength: 5f);
    
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 1;
        }

        yield return new WaitForSeconds(0.15f);
        cg.DOFade(0, 0.3f).OnComplete(() =>
        {
            cg.alpha = 1;
            gameObject.SetActive(false);
        });
    }
}