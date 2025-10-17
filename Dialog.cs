using System.Collections.Generic;

namespace Script.Assets.Script.Dialog
{
    /// <summary>
    /// Classe che definisce ogni Dialogo è composto da un titolo e da una lista di linee
    /// </summary>
    [System.Serializable]
    public class Dialog
    {
        public string Title;
        public List<DialogLine> Lines = new List<DialogLine>();
        public bool isSkippable;
    }
}
