using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
	public Text txtNameLeft, txtNameRight;

	public Image imgSpriteLeft, imgSpriteRight;
	
	public Text txtSentence;

	private DialogConfig _dialog;
	private int _idCurrentSentence = 0;
	private AudioSource _audioSource;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void PlayDialog(DialogConfig dialog)
	{
		Time.timeScale = 0;

		gameObject.SetActive(true);

		txtNameLeft.text = dialog.nameLeft;
		imgSpriteLeft.sprite = dialog.spriteLeft;

		txtNameRight.text = dialog.nameRight;
		imgSpriteRight.sprite = dialog.spriteRight;

		_dialog = dialog;

		RefreshBox();
	}

	private void RefreshBox()
	{
		DialogConfig.SentenceConfig sentence = _dialog.sentenceConfig[_idCurrentSentence];

		switch (sentence.position)
		{
			case DialogConfig.SentenceConfig.POSITION.LEFT:
				txtNameLeft.color = Color.black;
				txtNameRight.color = Color.clear;

				imgSpriteLeft.color = Color.white;
				imgSpriteRight.color = Color.gray;
				break;

			case DialogConfig.SentenceConfig.POSITION.RIGHT:
				txtNameLeft.color = Color.clear;
				txtNameRight.color = Color.black;

				imgSpriteLeft.color = Color.gray;
				imgSpriteRight.color = Color.white;
				break;
		}

		txtSentence.text = sentence.sentence;

		_audioSource.Stop();

		_audioSource.clip = sentence.audioClip;
		_audioSource.Play();
	}

	public void NextSentence()
    {
		_idCurrentSentence++;

		if (_idCurrentSentence < _dialog.sentenceConfig.Count)
			RefreshBox();
		else
			CloseDialog();
    }

	public void CloseDialog()
    {
		Time.timeScale = 1;

		_idCurrentSentence = 0;
		_dialog = null;

		gameObject.SetActive(false);
    }
}
