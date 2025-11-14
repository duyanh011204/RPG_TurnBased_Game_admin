using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public static bool showDialogue = false; // Flag bật dialogue khi load scene
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
        yield return null; // đợi 1 frame để DialogueManager.Instance được set
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        List<string> lines = new List<string>()
        {
            "Hello, this is the dialogue system!",
            "You can click to see the next line.",
            "When the dialogue ends, the panel will disappear."
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
