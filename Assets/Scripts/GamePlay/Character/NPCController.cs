using Dungeon_3DRPG_Demo;
using FrameworkDesign;
using System;
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
    public enum NPCStates
    {
        IDLE,
        WALK,
        TALK
    }

    public class NPCController : MonoBehaviour, IController
    {
        public DialogueData_SO DialogueData;
        private NPCStates mNPCStates;
        private Transform mPlayerTrans;

        public IArchitecture GetArchiteccture()
        {
            return DungeonRPG.Interface;
        }

        private void Awake()
        {
            mPlayerTrans = GameObject.FindWithTag("Player").transform;
        }

        private void Start()
        {
            this.RegisterEvent<OnDialogueStartEvent>(OnDialogueStart).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<OnDialogueOverEvent>(OnDialogueOver).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnDialogueStart(OnDialogueStartEvent obj)
        {
            mNPCStates = NPCStates.IDLE;
            StartCoroutine(RotateToPlayer());
            mNPCStates = NPCStates.TALK;
        }

        private void OnDialogueOver(OnDialogueOverEvent obj)
        {
            mNPCStates = NPCStates.IDLE;
        }

        private IEnumerator RotateToPlayer()
        {
            Vector3 direction = (mPlayerTrans.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            float elapsedTime = 0;

            while (elapsedTime < 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
        }
    }
}
