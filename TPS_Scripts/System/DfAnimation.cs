using UnityEngine;
using UnityEngine.EventSystems;  // UIイベントを取得するために必要

public class DfAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator animator;
    [SerializeField] private GameObject Covore;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // マウスがUIの上に来た時
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.SetBool("isHover", true);
        }
    }

    // マウスがUIから離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.SetBool("isHover", false);
        }
    }

    // マウスでクリックした時の処理
    public void OnPointerClick(PointerEventData eventData)
    {
        Covore.SetActive(true);

        Debug.Log("UIがクリックされました！");

        if (Covore == null)
        {
            Debug.Log("未完成のマップだよ");
        }
    }
}
