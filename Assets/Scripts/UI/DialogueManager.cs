using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject continueIndicator;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private Queue<string> sentences;
    private bool isTyping = false;
    private string currentSentence;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        continueIndicator.SetActive(false);
        sentences = new Queue<string>();

        
    }

    public void StartDialogue(string speakerName, Sprite portrait, List<string> dialogueLines)
    {
        dialoguePanel.SetActive(true);
        nameText.text = speakerName;
        portraitImage.sprite = portrait;
        sentences.Clear();

        foreach (string line in dialogueLines)
            sentences.Enqueue(line);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping) return;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        continueIndicator.SetActive(false);

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continueIndicator.SetActive(true);
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
