using System.Collections.Generic;
using UnityEngine;

namespace Script.Assets.Script.Dialog
{
    /// <summary>
    /// ScriptableObject composto da titolo, id della lingua e lista di dialoghi
    /// </summary>
    [CreateAssetMenu(fileName = "DialogSO", menuName = "Dialoghi/DialogSO")]
    public class DialogSO : ScriptableObject
    {
        public string Title;
        public int IDLang;
        public List<Dialog> Dialogs = new List<Dialog>();
    }
}
