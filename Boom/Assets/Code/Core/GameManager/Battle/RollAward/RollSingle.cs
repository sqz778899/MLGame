using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RollSingle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
   public int ItemID;
   [Header("依赖资产")]
   [SerializeField] Image Icon;
   [SerializeField] TextMeshProUGUI txtItemName;
   [SerializeField] TextMeshProUGUI txtItemDesc;
   [SerializeField] TextMeshProUGUI txtItemFlavor;
   [SerializeField] Image highlightBG; // 背景图用于高亮
   [SerializeField] Image RarityOutLine; // 背景图用于高亮
   [Header("稀有度颜色")]
   public Color Rare1;
   public Color Rare2;
   public Color Rare3;
   public Color Rare4;

   private Action<DropedObjEntry> _onSelected;
   DropedObjEntry _dropedObjEntry;
   
   public void InitData(DropedObjEntry dropedObj, Action<DropedObjEntry> onSelected)
   {
      _onSelected = onSelected;
      _dropedObjEntry = dropedObj;
      ItemID = dropedObj.ID;
      Icon.sprite = ResManager.instance.GetItemIcon(ItemID);
      ItemJson itemJson = TrunkManager.Instance.GetItemJson(ItemID);
      txtItemName.text = itemJson.Name;
      txtItemDesc.text = TextProcessor.Parse(itemJson.Desc);
      txtItemFlavor.text = itemJson.Flavor;
      
      //同步稀有度颜色
      switch (itemJson.Rarity)
      {
         case DropedRarity.Common:
            RarityOutLine.color = Rare1;
            break;
         case DropedRarity.Rare:
            RarityOutLine.color = Rare2;
            break;
         case DropedRarity.Epic:
            RarityOutLine.color = Rare3;
            break;
         case DropedRarity.Legendary:
            RarityOutLine.color = Rare4;
            break;
      }
      
      highlightBG.enabled = false;
   }

   public void OnPointerEnter(PointerEventData eventData)=> highlightBG.enabled = true;   
   public void OnPointerExit(PointerEventData eventData) => highlightBG.enabled = false;
   public void OnPointerClick(PointerEventData eventData)
   {
      _onSelected?.Invoke(_dropedObjEntry);
   }
}
