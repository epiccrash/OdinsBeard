﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public float typingSpeed;
    public string level;

    private Queue<string> sentences;

	// Use this for initialization
	void Start () {

        sentences = new Queue<string>();
		
	}
    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
        

    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        yield return new WaitForSeconds(0.27f);
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            if (Input.GetKey(KeyCode.Return))
            {
                yield return new WaitForSeconds(0.0f);
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }

    void EndDialogue ()
    {
        Debug.Log("End of conversation");

        // Insert transition logic here

        SceneManager.LoadScene(level);
    }
}
