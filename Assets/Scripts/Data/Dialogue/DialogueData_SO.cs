using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：对话数据
 * 创建时间：2023年4月1日21:39:32
 */

namespace Dungeon_3DRPG_Demo
{
    [System.Serializable]
    public class DialoguePiece
    {
        public string pieceId;
        public string NPCName;
        [TextArea]
        public string mainText;
        public List<DialogueOption> options = new List<DialogueOption>();
    }

    [System.Serializable]
    public class DialogueOption
    {
        public string text;
        public string targetId;
    }

    [CreateAssetMenu(fileName = "New DialogueData", menuName = "Dialogue Data")]
    public class DialogueData_SO : ScriptableObject
    {
        public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
        public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

#if UNITY_EDITOR
        private void OnValidate()
        {
            dialogueIndex.Clear();
            foreach (DialoguePiece piece in dialoguePieces)
            {
                if (!dialogueIndex.ContainsKey(piece.pieceId))
                    dialogueIndex.Add(piece.pieceId, piece);
            }
        }
#else
    private void Awake()
    {
        dialogueIndex.Clear();
        foreach (DialoguePiece piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.pieceId))
                dialogueIndex.Add(piece.pieceId, piece);
        }
    }
#endif
    }
}
