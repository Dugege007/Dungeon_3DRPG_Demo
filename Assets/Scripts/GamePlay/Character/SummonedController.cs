using UnityEngine;
using UnityEngine.AI;

/*
 * 创建人：杜
 * 功能说明：召唤物控制
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class SummonedController : MonoBehaviour
    {
        //跟随目标点
        private NavMeshAgent nav;
        private Animator animator;
        private Transform targetTrans;

        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            targetTrans = GameObject.FindWithTag("Player").GetComponent<PlayerController>().mSummonedFollowTrans;
        }

        private void Update()
        {
            MoveToPlayer();
        }

        /// <summary>
        /// 向玩家身后移动
        /// </summary>
        private void MoveToPlayer()
        {
            if (Vector3.Distance(transform.position, targetTrans.position) > nav.stoppingDistance)
            {
                nav.destination = targetTrans.position;
                animator.SetFloat("MoveSpeed", 1);
            }
            else
            {
                animator.SetFloat("MoveSpeed", 0);
            }
        }
    }
}
