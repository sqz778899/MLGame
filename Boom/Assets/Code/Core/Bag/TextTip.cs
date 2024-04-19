using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;

public class TextTip : MonoBehaviour
{
   public TextMeshProUGUI CurText;
   public CanvasGroup canvasGroup;
   public float duration = 1.0f;
   
   Vector3 end;


   private void Start()
   {
      end = new Vector3(transform.position.x,transform.position.y + 10,transform.position.z);
      
      StartCoroutine(DestroyAfterTween());
   }
   
   IEnumerator DestroyAfterTween()
   {
      // 移动和淡出
      transform.DOMove(end, duration);
      canvasGroup.DOFade(0, duration);

      // 等待动画结束
      yield return new WaitForSeconds(duration + 0.5f);

      // 销毁自己
      Destroy(gameObject);
   }
   
   private void OnDisable()
   {
      // 停止所有DOTween动画
      transform.DOKill();
      canvasGroup.DOKill();
   }
}
