using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public float typingSpeed;
    public string level;

    public string sceneName;
    private bool sentenceFinished;

    private Queue<string> sentences;

	// Use this for initialization
	void Start () {

        sentences = new Queue<string>();

        sentenceFinished = false;
		
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
        if (sceneName.Equals("opening")) {
            if (sentences.Count == 1)
            {
                GameObject.Find("Player").GetComponent<Image>().enabled = false;
            }
            if (sentences.Count == 8 || sentences.Count == 2) {
                SoundManager.S.MakeOdinThunder();
            }
            if (sentences.Count == 9) {
                GameObject.Find("Player").GetComponent<Image>().enabled = true;
            }
            if (sentences.Count == 9 || sentences.Count == 3) {
                nameText.text = "Player";
            } else {
                nameText.text = "Odin";
            }
        }

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
        sentenceFinished = false;
        yield return new WaitForSeconds(0.27f);
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            SoundManager.S.MakeTxt();
            if (Input.GetKey(KeyCode.Return))
            {
                yield return new WaitForSeconds(0.0f);
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
            if (Input.GetKey(KeyCode.Space)) {
                EndDialogue();
            }
        }
        sentenceFinished = true;
    }

    void EndDialogue ()
    {
        StopAllCoroutines();
        Debug.Log("End of conversation");

        // Insert transition logic here

        SceneManager.LoadScene(level);
    }

    private void Update()
    {
        if (sentenceFinished && Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextSentence();
        }
    }
}
