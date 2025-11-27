using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public static bool showDialogue = false; 
    public Sprite testPortrait;

    void Start()
    {
        if (showDialogue)
        {
            showDialogue = false;
            StartCoroutine(TriggerNextFrame());
        }
    }

    private IEnumerator TriggerNextFrame()
    {
        yield return null; 
    }

    public void TriggerDialogue()
    {
        List<string> lines = new List<string>()
        {
            "Hello there, brave explorer!",
            "This is the 2D world where your adventure begins.",
            "Click to continue… and let your story unfold!"

        };

        if (DialogueManager.Instance != null)   
        {
            DialogueManager.Instance.StartDialogue("Player", testPortrait, lines);
        }
        else
        {
            Debug.LogError("DialogueManager.Instance is null!");
        }
    }
}
