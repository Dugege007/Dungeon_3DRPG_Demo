using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class PlayerInputStateController : StateMachineBehaviour
    {
        private PlayerController playerController;
        [Header("Lock Base")]
        public bool lockAll;
        public bool keepLocking;

        [Header("Lock Custom")]
        public bool lockMove;
        public bool lockRotate;
        public bool lockJump;
        public bool lockAttack;
        public bool lockEquip;
        public bool lockTalk;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerController = animator.GetComponent<PlayerController>();

            LockInput();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (keepLocking)
                LockInput();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerController.UnlockInput();
        }

        private void LockInput()
        {
            if (lockAll)
                playerController.canInput = false;

            if (lockMove)
                playerController.canMove = false;
            if (lockRotate)
                playerController.canRotate = false;
            if (lockJump)
                playerController.canJump = false;
            if (lockAttack)
                playerController.canAttack = false;
            if (lockTalk)
                playerController.canTalk = false;
            if (lockEquip)
                playerController.canEquip = false;
        }
    }
}
