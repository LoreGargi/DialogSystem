using Assets.Script.Common.Enum;
using Script.Assets.Script.Common.Enum;
using Script.Assets.Script.Dialog;
using Script.Assets.Script.Dialog.Enum;
using Script.Assets.Script.Interactive.Impl.Read;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Script.Assets.Script.Manager
{
    /// <summary>
    /// Manager per gestire i dialoghi del gioco
    /// </summary>
    public class DialogManager : MonoBehaviour
    {
        [SerializeField]
        private DialogSO _dialogSO;

        [SerializeField]
        private DialogTalkerSO _dialogTalkersSO;

        [SerializeField]
        private int _IDLang;

        private TextMeshProUGUI _dialogueText;
        private Image _leftImage;
        private Image _rightImage;
        private Image _centerImage;
        private Image _leftClickButton;
        private Image _playButton;
        private Image _xboxButton;
        private Image _switchButton;

        [SerializeField]
        private float _textSpeed;

        [SerializeField]
        private GameObject _dialogPanel;

        private int _characterID;
        private Coroutine _writingCoroutine;
        private int _lineID = 0;
        private bool _isWriting, _skipWriting;

        public static DialogManager Instance;

        /// <inheritdoc/>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        /// <inheritdoc/>
        private void Start()
        {
            _dialogPanel.SetActive(false);
            _skipWriting = false;
            _IDLang = _dialogSO.IDLang;
            GameManager.Instance.GameState = GameState.Dialogue;
            _leftImage = _dialogPanel.transform.GetChild(0).GetComponent<Image>();
            _centerImage = _dialogPanel.transform.GetChild(1).GetComponent<Image>();
            _rightImage = _dialogPanel.transform.GetChild(2).GetComponent<Image>();
            _dialogueText = _dialogPanel.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
            _leftClickButton = _dialogPanel.transform.GetChild(4).GetComponent<Image>();
            _playButton = _dialogPanel.transform.GetChild(5).GetComponent<Image>();
            _xboxButton = _dialogPanel.transform.GetChild(6).GetComponent<Image>();
            _switchButton = _dialogPanel.transform.GetChild(7).GetComponent<Image>();
            StartDialog(0);
        }


        /// <summary>
        /// Metodo richiamato nel GameManager che gestisce il metodo Next e Close
        /// </summary>
        public void DialogState()
        {
            EnableDisableClickImage();
            if (InputManager.Instance.ActionClickDown())
            {
                if (_isWriting)
                {
                    _skipWriting = true;
                }
                else
                {
                    Next(_characterID, _lineID);
                }
            }
            if (InputManager.Instance.BackButton())
            {
                Close();
            }
        }

        /// <summary>
        /// Metodo per gestire a schermo l'icona di mouse o X del controller a seconda 
        /// dell'ultimo input premuto
        /// </summary>
        public void EnableDisableClickImage()
        {
            switch (InputManager.Instance.GetInputType())
            {
                case DeviceInputType.PlayStation:
                    _playButton.enabled = true;
                    _leftClickButton.enabled = false;
                    _xboxButton.enabled = false;
                    _switchButton.enabled = false;
                    break;
                case DeviceInputType.Xbox:
                    _xboxButton.enabled = true;
                    _leftClickButton.enabled = false;
                    _playButton.enabled = false;
                    _switchButton.enabled = false;
                    break;
                case DeviceInputType.Switch:
                    _switchButton.enabled = true;
                    _leftClickButton.enabled = false;
                    _playButton.enabled = false;
                    _xboxButton.enabled = false;
                    break;
                default:
                    _leftClickButton.enabled = true;
                    _playButton.enabled = false;
                    _xboxButton.enabled = false;
                    _switchButton.enabled = false;
                    break;
            }
        }

        /// <summary>
        /// metodo che abilita o disabilita il componente Image del character
        /// e ne gestisce l'alpha nel caso in cui non ci sia uno sprite
        /// </summary>
        /// <param name="value">valore che gestisce l'Image del character</param>
        public void EnableDisableLeftImage(bool value)
        {
            _leftImage.enabled = value;
            if (_leftImage.sprite == null)
            {
                _leftImage.color = new Color(255, 255, 255, 0);
            }
            else
            {
                _leftImage.color = new Color(255, 255, 255, 1);
            }
        }

        /// <summary>
        /// metodo che abilita o disabilita il componente Image dell'Right
        /// e ne gestisce l'alpha nel caso in cui non ci sia uno sprite
        /// </summary>
        /// <param name="value">valore che gestisce l'Image dell'npc</param>
        public void EnableDisableRightImage(bool value)
        {
            _rightImage.enabled = value;
            if (_rightImage.sprite == null)
            {
                _rightImage.color = new Color(255, 255, 255, 0);
            }
            else
            {
                _rightImage.color = new Color(255, 255, 255, 1);
            }
        }

        /// <summary>
        /// metodo che abilita o disabilita il componente Image dell'interactable item
        /// e ne gestisce l'alpha nel caso in cui non ci sia uno sprite
        /// </summary>
        /// <param name="value">valore che gestisce l'Image dell'interactable item</param>
        public void EnableDisableCenterImage(bool value)
        {
            _centerImage.enabled = value;
            if (_centerImage.sprite == null)
            {
                _centerImage.color = new Color(255, 255, 255, 0);
            }
            else
            {
                _centerImage.color = new Color(255, 255, 255, 1);
            }
        }

        /// <summary>
        /// metodo che restituisce il testo che verrà stampato a schermo dato l'id dell'oggetto con cui interagiamo 
        /// </summary>
        /// <param name="DialogID">id del dialogo nello scriptableObject</param>
        /// <param name="lineID">linea del dialogo da richiamare</param>
        /// <returns></returns>
        public string GetDialogText(int DialogID, int lineID)
        {
            return _dialogSO.Dialogs[DialogID].Lines[lineID].DialogText[_IDLang];
        }

        /// <summary>
        /// metodo che resistuisce il numero di elementi in Dialogs
        /// </summary>
        /// <returns>numero di elementi in Dialogs</returns>
        public int GetDialogSOCount()
        {
            return _dialogSO.Dialogs.Count;
        }

        /// <summary>
        /// metodo che gestisce l'inizio del dialogo e verifica chi sta parlando per attivare l'Image corrispondente
        /// fa poi partire la coroutine
        /// </summary>
        /// <param name="DialogID"> id del dialogo nello scriptableObject</param>
        public void StartDialog(int DialogID)
        {
            _characterID = DialogID;
            if (GameManager.Instance.GameState != GameState.Dialogue)
            {
                GameManager.Instance.GameState = GameState.Dialogue;
            }
            _dialogPanel.SetActive(true);
            if (_dialogSO.Dialogs[DialogID].Lines[_lineID].Position == DialogPosition.Left)
            {
                _leftImage.sprite = _dialogTalkersSO.GetTalkerImage(_dialogSO.Dialogs[DialogID].Lines[_lineID].Talker);
                EnableDisableLeftImage(true);
                EnableDisableRightImage(false);
                EnableDisableCenterImage(false);
            }
            if (_dialogSO.Dialogs[DialogID].Lines[_lineID].Position == DialogPosition.Right)
            {
                _rightImage.sprite = _dialogTalkersSO.GetTalkerImage(_dialogSO.Dialogs[DialogID].Lines[_lineID].Talker);
                EnableDisableLeftImage(false);
                EnableDisableRightImage(true);
                EnableDisableCenterImage(false);
            }
            if (_dialogSO.Dialogs[DialogID].Lines[_lineID].Position == DialogPosition.Center)
            {
                _centerImage.sprite = _dialogTalkersSO.GetTalkerImage(_dialogSO.Dialogs[DialogID].Lines[_lineID].Talker);
                EnableDisableLeftImage(false);
                EnableDisableRightImage(false);
                EnableDisableCenterImage(true);
            }
            _writingCoroutine = StartCoroutine(WriteText(GetDialogText(DialogID, _lineID), _textSpeed));
        }

        /// <summary>
        /// coroutine che scrive a schermo il testo
        /// </summary>
        /// <param name="text">testo da stampare a schermo</param>
        /// <param name="textspeed">velocità con cui stampare a schermo</param>
        /// <returns></returns>
        private IEnumerator WriteText(string text, float textspeed)
        {
            _isWriting = true;
            _skipWriting = false;
            _dialogueText.text = "";
            foreach (char letter in text)
            {
                if (_skipWriting)
                {
                    _dialogueText.text = text;
                    break;
                }
                _dialogueText.text += letter;
                yield return new WaitForSeconds(textspeed);
            }
            _isWriting = false;
        }

        /// <summary>
        /// metodo che passa alla linea successiva di dialogo
        /// se terminate le linee di dialogo chiude il dialogo
        /// </summary>
        /// <param name="dialogID">id del dialogo dello scriptableObject</param>
        /// <param name="lineID">linea di dialogo interna</param>
        private void Next(int dialogID, int lineID)
        {
            int currentLineID = lineID;
            if (currentLineID < _dialogSO.Dialogs[dialogID].Lines.Count - 1)
            {
                _lineID++;
                StartDialog(dialogID);
            }
            else
            {
                Close();
            }
        }
        /// <summary>
        /// Metodo per far partire il dialogo interagendo con un oggetto
        /// </summary>
        /// <param name="readableItem">gli passiamo lo script <see cref="ReadableItem"/> che interagisce</param>
        public void DialogFromObject(ReadableItem readableItem)
        {
            _characterID = readableItem.CharacterID;
            if (_characterID < 0 || _characterID >= _dialogSO.Dialogs.Count)
            {
                print("WARNING: ID al di fuori dei limiti");
            }
            StartDialog(_characterID);
        }

        /// <summary>
        /// metodo di chiusura del dialogo
        /// </summary>
        public void Close()
        {
            StopCoroutine(_writingCoroutine);
            _lineID = 0;
            _dialogPanel.SetActive(false);
            GameManager.Instance.GameState = GameState.EndDialogue;
        }
    }
}

