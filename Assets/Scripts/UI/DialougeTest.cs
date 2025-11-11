using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public Sprite testPortrait;

    public void TriggerDialogue()
    {
        List<string> lines = new List<string>()
        {
            "Hello, this is the dialogue system!",
            "You can click to see the next line.",
            "When the dialogue ends, the panel will disappear."
        };

        DialogueManager.Instance.StartDialogue("Player", testPortrait, lines);
    }
}
