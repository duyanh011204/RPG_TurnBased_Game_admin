using UnityEngine;
using System.Collections.Generic;

public class NPCInteractable : MonoBehaviour
{

    public string npcName;
    public Sprite portrait;
    [TextArea] public List<string> dialogueLines;
    public bool showChoicesAtEnd = true; 
}
