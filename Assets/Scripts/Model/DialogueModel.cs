using FrameworkDesign;
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
    public interface IDialogueModel : IModel
    {
        public DialogueData_SO StartDialogueData { get; set; }
        public BindableProperty<string> NPCName { get; }
        public BindableProperty<string> MainText { get; }
        public BindableProperty<string> OptionText { get; }
        public BindableProperty<string> TargetID { get; }
    }

    public class DialogueModel : AbstractModel, IDialogueModel
    {
        public DialogueData_SO StartDialogueData { get; set; }

        public BindableProperty<string> NPCName { get; } = new BindableProperty<string>();

        public BindableProperty<string> MainText { get; } = new BindableProperty<string>();

        public BindableProperty<string> OptionText { get; } = new BindableProperty<string>();

        public BindableProperty<string> TargetID { get; } = new BindableProperty<string>();

        protected override void OnInit()
        {
            StartDialogueData = Resources.Load<DialogueData_SO>("Data/DialogueData/Ghost_02 Start DialogueData");

            var storage = this.GetUtility<IStorage>();
        }
    }
}
