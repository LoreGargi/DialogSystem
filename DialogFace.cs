using Script.Assets.Script.Dialog.Enum;
using UnityEngine;


namespace Script.Assets.Script.Dialog
{
    /// <summary>
    /// classe che gestisce chi parla nel dialogo assegnando un nome dell'enum dialogTalker e uno sprite
    /// </summary>
    [System.Serializable]
    public class DialogFace
    {
        public DialogTalker Talker;
        public Sprite Image;
    }

}