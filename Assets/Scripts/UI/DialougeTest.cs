using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public Sprite testPortrait;

    IEnumerator Start()
    {
        yield return null; // đợi 1 frame để DialogueManager kịp Awake

        List<string> lines = new List<string>()
        {
            "Xin chào, đây là hệ thống hội thoại!",
            "Bạn có thể nhấn để xem câu tiếp theo.",
            "Khi hết hội thoại, bảng sẽ biến mất."
        };

        DialogueManager.Instance.StartDialogue("Nhân vật", testPortrait, lines);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DialogueManager.Instance.DisplayNextSentence();
        }
    }
}
