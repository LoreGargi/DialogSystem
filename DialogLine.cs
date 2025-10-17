
using Script.Assets.Script.Dialog.Enum;
using UnityEngine;

namespace Script.Assets.Script.Dialog
{
    /// <summary>
    /// Linee di dialogo composte da array di stringhe dove ogni elemento si collega all'id Lingua
    /// ogni linea di dialogo ha poi una definizione di posizione nel canvas (Position) e una definizione
    /// di chi sta parlando
    /// </summary>
    [System.Serializable]
    public class DialogLine
    {
        [TextArea(3, 6)]
        public string[] DialogText;
        public DialogPosition Position;
        public DialogTalker Talker;
    }
}