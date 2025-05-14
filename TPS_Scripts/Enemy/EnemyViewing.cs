using UnityEngine;

public class EnemyViewing : MonoBehaviour
{
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetTrigger("isJumpAttack");
    }
}
