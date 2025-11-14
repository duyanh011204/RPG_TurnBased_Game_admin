using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadDialogueScene()
    {
        DialogueTest.showDialogue = true;
        SceneManager.LoadScene("GameWorld");
    }
}
