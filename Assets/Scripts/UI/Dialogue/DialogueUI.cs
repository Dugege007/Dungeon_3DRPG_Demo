using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using FrameworkDesign;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

/*
 * 创建人：杜
 * 功能说明：对话UI控制
 * 创建时间：2023年4月1日22:38:05
 */

namespace Dungeon_3DRPG_Demo
{
    public class DialogueUI : MonoBehaviour, IController, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("主要对话")]
        public Text mMainText;
        public Text mNPCName;
        public Button mNextBtn;

        public GameObject mDialoguePanel;

        [Header("选项")]
        public RectTransform mOptionPanel;
        public GameObject mOptionObj;

        [Header("对话数据")]
        public DialogueData_SO currentDialogueData;
        private int mCurrentDialogueIndex;
        private DialoguePiece mCurrentDialoguePiece;

        private IDialogueModel mDialogueModel;

        public IArchitecture GetArchiteccture()
        {
            return DungeonRPG.Interface;
        }

        private void Awake()
        {
            mMainText = transform.Find("Dialogue Panel/Main Text").GetComponent<Text>();
            mNPCName = transform.Find("Dialogue Panel/Main Text/NPCName Text").GetComponent<Text>();
            mNextBtn = transform.Find("Dialogue Panel/Main Text/Next Btn").GetComponent<Button>();
            mNextBtn.onClick.AddListener(NextDialogue);

            mDialoguePanel = transform.Find("Dialogue Panel").gameObject;

            mOptionPanel = transform.Find("Dialogue Panel/Option Point/Option Panel") as RectTransform;
            mOptionObj = Resources.Load<GameObject>("Prefabs/UI/Option Btn");

            mDialogueModel = this.GetModel<IDialogueModel>();
        }

        private void Start()
        {
            this.RegisterEvent<OnDialogueStartEvent>(OnDialogueStart).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<OnDialogueOverEvent>(OnDialogueOver).UnRegisterWhenGameObjectDestroyed(gameObject);

            mDialogueModel.TargetID.RegisterOnValueChanged(OnTargetIDChanged).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnDialogueStart(OnDialogueStartEvent obj)
        {
            Debug.Log("打开对话");
            SetDialogueData(mDialogueModel.StartDialogueData);
            UpdateDialogue(mDialogueModel.StartDialogueData.dialoguePieces[0]);
            mDialoguePanel.SetActive(true);
        }

        private void OnDialogueOver(OnDialogueOverEvent obj)
        {
            Debug.Log("关闭对话");
            mDialoguePanel.SetActive(false);
        }

        private void OnTargetIDChanged(string targetID)
        {
            UpdateDialogue(currentDialogueData.dialogueIndex[targetID]);
        }

        private void SetDialogueData(DialogueData_SO dialogueData)
        {
            currentDialogueData = dialogueData;
            mCurrentDialogueIndex = 0;
        }

        private void UpdateDialogue(DialoguePiece dialoguePiece)
        {
            if (mCurrentDialogueIndex == 0)
                SetDialogueTest();

            mCurrentDialoguePiece = dialoguePiece;
            mCurrentDialogueIndex = currentDialogueData.dialoguePieces.IndexOf(mCurrentDialoguePiece) + 1;

            if (dialoguePiece.NPCName != "")
                mNPCName.text = dialoguePiece.NPCName + "：";

            mMainText.text = "";
            mMainText.DOText(dialoguePiece.mainText, 1f);

            if (dialoguePiece.options.Count == 0 && currentDialogueData.dialoguePieces.Count > 0)
                mNextBtn.gameObject.SetActive(true);
            else
                mNextBtn.gameObject.SetActive(false);

            CreateOptions(dialoguePiece);
        }

        private void NextDialogue()
        {
            if (mCurrentDialogueIndex < currentDialogueData.dialoguePieces.Count)
                UpdateDialogue(currentDialogueData.dialoguePieces[mCurrentDialogueIndex]);
            else
                this.SendCommand<DialogueOverCommand>();
        }

        private void CreateOptions(DialoguePiece dialoguePiece)
        {
            if (mOptionPanel.childCount > 0)
            {
                for (int i = 0; i < mOptionPanel.childCount; i++)
                {
                    Destroy(mOptionPanel.GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < dialoguePiece.options.Count; i++)
            {
                OptionUI option = Instantiate(mOptionObj, mOptionPanel).GetComponent<OptionUI>();
                option.SetOption(dialoguePiece, dialoguePiece.options[i]);
            }
        }

        private void SetDialogueTest()
        {
            DateTime dateTime = DateTime.Now;
            currentDialogueData.dialoguePieces[0].mainText = "当前日期为：" + dateTime.ToString("yyyy/MM/dd");
            currentDialogueData.dialoguePieces[1].mainText = "当前时间为：" + dateTime.ToString("HH: mm: ss");
            currentDialogueData.dialoguePieces[2].mainText = "你好啊";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            RectTransform rectTrans = (RectTransform)mDialoguePanel.transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTrans, eventData.position))
                this.SendCommand<PointerEnterUICommand>();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            RectTransform rectTrans = (RectTransform)mDialoguePanel.transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTrans, eventData.position))
                this.SendCommand<PointerExitUICommand>();
        }
    }
}
