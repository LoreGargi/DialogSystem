using Script.Assets.Script.Dialog.Enum;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Assets.Script.Dialog
{
    /// <summary>
    /// scriptableObject composto da una lista di dialogFaces
    /// </summary>
    [CreateAssetMenu(fileName = "DialogTalkersSO", menuName = "Dialoghi/DialogTalkersSO")]
    public class DialogTalkerSO : ScriptableObject
    {
        public List<DialogFace> Faces = new List<DialogFace>();

        /// <summary>
        /// metodo per trovare nell'elenco dei Talker il Talker che viene dato in input dal dialogueManager
        /// </summary>
        /// <param name="talkerName"><see cref="DialogTalker"/> name di chi parla</param>
        /// <returns>Ritorna lo <see cref="Sprite"/> da mostrare.</returns>
        public Sprite GetTalkerImage(DialogTalker talkerName)
        {
            DialogFace talker = Faces.Find(t => t.Talker == talkerName);
            return talker?.Image;
        }
    }
}
