using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueControler : MonoBehaviour
{
    public static DialogueControler instance { get; private set; }

    private static int gameLanguage;
    public static int GetSetGameLanguage { get { return gameLanguage; } set { gameLanguage = value; } }

    public TextMeshProUGUI nameSpeeker;
    public TextMeshProUGUI txtSentence;

    public Image imgSpriteLeft;
    public Image imgSpriteRight;

    private DialogueConfig _dialog;
    private SpeekerConfig _speekerConfig;

    private int eventCount = -1;
    //private int speekerCount = -1;

    /*private int idFirstSpeeker = 0;
    private int idLastSpeeker = -1;*/

    private Queue<string> sentences = new Queue<string>();

    private AudioSource _audioSource;

    [System.Serializable]
    public enum TEXT_ANIMATION
    {
        DEFAULT,
        GRADUAL_ONSET,
        SHAKE
    }

    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    public void StartDialogue(DialogueConfig dialogue, SpeekerConfig speekers)
    {
        if (dialogue.allDialogueEvents.Count == 0) return;

        _dialog = dialogue;
        _speekerConfig = speekers;

        // initialisation
        NextDialogueEvent();
    }

    private void NextDialogueEvent()
    {
        eventCount++;

        if (eventCount >= _dialog.allDialogueEvents.Count)
        {
            DialogueEnd();
            return;
        }

        switch (_dialog.allDialogueEvents[eventCount].source)
        {
            case DialogueEvent.TYPE_EVENT.SENTENCE:

                nameSpeeker.text = _speekerConfig.allSpeekers[_dialog.allDialogueEvents[eventCount].idSpeeker].name;
                sentences.Clear();

                foreach (SentenceConfig.Sentence other in _dialog.allDialogueEvents[eventCount].sentenceConfig.talking)
                {
                    // -------------------------------------------------------- TO DO -------------------------------------------------------- //
                    // Prendre la bonne traduction

                    sentences.Enqueue(other.sentence.FR);
                }

                DisplayNextSentence();
                break;

            case DialogueEvent.TYPE_EVENT.EVENT:

                switch (_dialog.allDialogueEvents[eventCount].eventConfig.actionType)
                {
                    case EventConfig.ACTION_TYPE.SPEAKER_IN:
                        Debug.Log("SPEEKER IN");
                        break;

                    case EventConfig.ACTION_TYPE.SPEAKER_OUT:
                        Debug.Log("SPEEKER OUT");
                        break;
                }
                break;

            case DialogueEvent.TYPE_EVENT.CHOICE:
                break;
        }
    }

    private void DialogueEnd()
    {

    }
    
    public void DisplayNextSentence()
    {
        // si il n'y a plus de phrase à afficher
        if (sentences.Count == 0)
        {
            NextDialogueEvent();
            return;
        }

        // passe à la phrase suivante
        string sentence = sentences.Dequeue();

        // si le jr appuis sur continuer quand la 1er animation ce joue, cette dernière sera stopé
        StopAllCoroutines();

        // affiche les phrase
        StartCoroutine(TypeSentence(sentence));
    }

    /*
    public void StartDialogue(DialogueConfig dialogue, SpeekerConfig speekers)
    {
        if (dialogue.sentenceConfigs.Count == 0) return;

        _dialog = dialogue;
        _speekerConfig = speekers;

        gameObject.SetActive(true);
        imgSpriteRight.gameObject.SetActive(false);

        idFirstSpeeker = _dialog.sentenceConfigs[0].idSpeeker;
        imgSpriteLeft.sprite = _speekerConfig.allSpeekers[idFirstSpeeker].sprite;
        NextSpeeker();
    }

    

    private void NextSpeeker()
    {
        speekerCount++;

        if (speekerCount >= _dialog.sentenceConfigs.Count)
        {
            EndDialogue();
            return;
        }

        if (!(idLastSpeeker == _dialog.sentenceConfigs[speekerCount].idSpeeker))
        {
            // affiche le nom
            nameSpeeker.text = _speekerConfig.allSpeekers[_dialog.sentenceConfigs[speekerCount].idSpeeker].name;

            if (_dialog.sentenceConfigs[speekerCount].idSpeeker == idFirstSpeeker)
            {
                imgSpriteLeft.color = new Color(1, 1, 1);
                imgSpriteRight.color = new Color(0.5f, 0.5f, 0.5f);
            }
            else
            {
                imgSpriteRight.gameObject.SetActive(true);
                imgSpriteLeft.color = new Color(0.5f, 0.5f, 0.5f);
                imgSpriteRight.color = new Color(1, 1, 1);
                imgSpriteRight.sprite = _speekerConfig.allSpeekers[_dialog.sentenceConfigs[speekerCount].idSpeeker].sprite;
            }

            idLastSpeeker = _dialog.sentenceConfigs[speekerCount].idSpeeker;
        }

        // efface les anciennes phrases
        sentences.Clear();

        // récupère les phrase présent dans l'array pour les mettre dans la queux
        foreach (DialogueConfig.SentenceConfig.Sentence other in _dialog.sentenceConfigs[speekerCount].speach)
        {
            sentences.Enqueue(other.sentence);
        }

        DisplayNextSentence();
    }*/

    private IEnumerator TypeSentence(string sentence)
    {
        txtSentence.text = "";
        // toCharArray sépare chaque caractère pour les mettre dans une array
        foreach (char letter in sentence.ToCharArray())
        {
            // ajoute la lette au texte
            txtSentence.text += letter;
            // attend quelque frame 
            yield return new WaitForSeconds(0.1f);
        }
    }

    /*private void EndDialogue()
    {
        gameObject.SetActive(false);
        speekerCount = -1;
        idLastSpeeker = -1;

        _dialog = null;
        _speekerConfig = null;

        animBordureBas.SetBool("isOpen", false);
        animBordureHaut.SetBool("isOpen", false);
        player.enabled = true;
        mouse.enabled = true;
        weapen.enabled = true;
    }*/
}
