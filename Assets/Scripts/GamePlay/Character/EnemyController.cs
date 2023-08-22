using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * 创建人：杜
 * 功能说明：敌人控制
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public enum EnemyStates
    {
        GUARD,
        PATROL,
        CHASE
    }

    public class EnemyController : MonoBehaviour
    {
        public PlayerController playerController;
        private NavMeshAgent nav;
        private Animator animator;
        private Collider coll;
        private EnemyStates enemyStates;
        //private IEnemyModel mEnemyModel;

        [Header("Basic")]
        public float sightRadius;
        public bool isGuard;
        public float lookAtTime;
        protected GameObject attackTarget;
        private float speed;
        private float remainLookAtTime;
        private float lastAttackTime;
        private Quaternion guardRotation;

        [Header("Patrol")]
        public float patrolRange;
        private Vector3 wayPoint;
        private Vector3 guardPosition;

        private bool isPatrol;
        private bool isChase;
        private bool isFollow;
        private bool isDead;
        private bool playerDead;


        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            coll = GetComponent<Collider>();
        }

        private void Start()
        {
            SwitchEnemyGuardOrPatrol();
        }

        private void Update()
        {
            if (isDead) return;
            if (playerDead) return;
            //if (characterStats.CurrentHP <= 0) Dead();

            SwitchStates();
            SwitchAnimation();
        }

        /// <summary>
        /// 切换巡逻类型
        /// </summary>
        private void SwitchEnemyGuardOrPatrol()
        {
            if (isGuard)
                enemyStates = EnemyStates.GUARD;
            else
            {
                enemyStates = EnemyStates.PATROL;
                GetNewWayPoint();
            }
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        private void SwitchStates()
        {

            if (FoundPlayer())
                enemyStates = EnemyStates.CHASE;

            switch (enemyStates)
            {
                case EnemyStates.GUARD:
                    break;
                case EnemyStates.PATROL:
                    break;
                case EnemyStates.CHASE:
                    break;
            }
        }

        /// <summary>
        /// 切换动画
        /// </summary>
        private void SwitchAnimation()
        {
            if (isPatrol || isChase || isFollow)
                animator.SetBool("Walk", true);
            else
                animator.SetBool("Walk", false);
            //animator.SetBool("Chase", isChase);
            //animator.SetBool("Follow", isFollow);
            animator.SetBool("IsDead", isDead);
        }

        /// <summary>
        /// 获取随机巡逻点
        /// </summary>
        private void GetNewWayPoint()
        {
            remainLookAtTime = lookAtTime;

            float randomX = Random.Range(-patrolRange, patrolRange);
            float randomZ = Random.Range(-patrolRange, patrolRange);

            Vector3 randomPoint = new Vector3(guardPosition.x + randomX, transform.position.y, guardPosition.z + randomZ);
            // 为避免出现随机点不在导航网格上，使用下面的方法获取随机点
            NavMeshHit hit;
            wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;

            Debug.Log(wayPoint);
        }

        /// <summary>
        /// 发现玩家
        /// </summary>
        /// <returns>是否发现</returns>
        private bool FoundPlayer()
        {
            var colliders = Physics.OverlapSphere(transform.position, sightRadius);

            foreach (var target in colliders)
            {
                if (target.CompareTag("Player"))
                {
                    attackTarget = target.gameObject;
                    return true;
                }
            }
            attackTarget = null;
            return false;
        }

        /// <summary>
        /// 攻击
        /// </summary>
        //private void Attack()
        //{
        //    transform.LookAt(attackTarget.transform);
        //    if (TargetInAttackRange())
        //        animator.CrossFade("Attack", 0.1f);
        //}

        /// <summary>
        /// 判断是否在攻击范围内
        /// </summary>
        /// <returns></returns>
        //private bool TargetInAttackRange()
        //{
        //    if (attackTarget != null)
        //        return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.templateAttackData.attackRange;
        //    else
        //        return false;
        //}

        private void Dead()
        {
            coll.enabled = false;
            nav.radius = 0;
            Destroy(gameObject, 3f);
        }
    }
}
