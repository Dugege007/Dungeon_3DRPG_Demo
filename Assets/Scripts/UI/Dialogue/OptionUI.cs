using FrameworkDesign;
using UnityEngine;
using UnityEngine.UI;

/*
 * 创建人：杜
 * 功能说明：选项UI
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class OptionUI : MonoBehaviour, IController
    {
        private DialoguePiece mCurrentPiece;
        private string mTargetId;
        private Text mOptionText;
        private Button mOptionBtn;

        private IDialogueModel mDialogueModel;

        public IArchitecture GetArchiteccture()
        {
            return DungeonRPG.Interface;
        }

        private void Awake()
        {
            mOptionText = transform.Find("Option Text").GetComponent<Text>();

            mOptionBtn = GetComponent<Button>();
            mOptionBtn.onClick.AddListener(OnOptionClick);

            mDialogueModel = this.GetModel<IDialogueModel>();
        }

        public void SetOption(DialoguePiece dialoguePiece, DialogueOption option)
        {
            Debug.Log("设置选项");
            mCurrentPiece = dialoguePiece;
            mOptionText.text = option.text;
            mTargetId = option.targetId;
        }

        public void OnOptionClick()
        {
            if (mTargetId == "")
            {
                this.SendCommand<DialogueOverCommand>();
            }
            else
            {
                mDialogueModel.TargetID.Value = mTargetId;
            }
        }
    }
}
