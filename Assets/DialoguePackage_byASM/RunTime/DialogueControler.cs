using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueControler : MonoBehaviour
{
    public static DialogueControler instance { get; private set; }

    public TextMeshProUGUI nameSpeeker;
    public TextMeshProUGUI txtSentence;

    public Image imgSpriteLeft;
    public Image imgSpriteRight;

    private DialogueConfig _dialog;
    private SpeekerConfig _speekerConfig;

    private int speekerCount = -1;

    private int idFirstSpeeker = 0;
    private int idLastSpeeker = -1;

    private Queue<string> sentences;

    private AudioSource _audioSource;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    void Start()
    {
        sentences = new Queue<string>();
        gameObject.SetActive(false);
    }

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

    public void DisplayNextSentence()
    {
        // si il n'y a plus de phrase à afficher
        if (sentences.Count == 0)
        {
            NextSpeeker();
            return;
        }

        // passe à la phrase suivante
        string sentence = sentences.Dequeue();

        // si le jr appuis sur continuer quand la 1er animation ce joue, cette dernière sera stopé
        StopAllCoroutines();

        // affiche les phrase
        StartCoroutine(TypeSentence(sentence));
    }

    private void NextSpeeker()
    {
        speekerCount++;

        Debug.Log(speekerCount + " " + _dialog.sentenceConfigs.Count);

        if(speekerCount >= _dialog.sentenceConfigs.Count)
        {
            EndDialogue();
            return;
        }

        if (!(idLastSpeeker == _dialog.sentenceConfigs[speekerCount].idSpeeker))
        {
            // affiche le nom
            nameSpeeker.text = _speekerConfig.allSpeekers[_dialog.sentenceConfigs[speekerCount].idSpeeker].name;

            if(_dialog.sentenceConfigs[speekerCount].idSpeeker == idFirstSpeeker)
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
    }

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

    private void EndDialogue()
    {
        gameObject.SetActive(false);
        speekerCount = -1;
        idLastSpeeker = -1;

        _dialog = null;
        _speekerConfig = null;

       /* animBordureBas.SetBool("isOpen", false);
        animBordureHaut.SetBool("isOpen", false);
        player.enabled = true;
        mouse.enabled = true;
        weapen.enabled = true;*/
    }
}
