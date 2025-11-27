using UnityEngine;

public class PlayerInteraction2D : MonoBehaviour
{
    public float interactionRadius = 1.5f;
    public GameObject eIndicator;

    private NPCInteractable nearbyNPC;

    void Update()
    {
        FindNearbyNPC();

        if (nearbyNPC != null)
            eIndicator.SetActive(!DialogueManager.IsDialogueActive);
        else
            eIndicator.SetActive(false);

        if (nearbyNPC != null && Input.GetKeyDown(KeyCode.E) && !DialogueManager.IsDialogueActive)
        {
            DialogueManager.Instance.StartDialogue(
                nearbyNPC.npcName,
                nearbyNPC.portrait,
                nearbyNPC.dialogueLines
                
            );
        }
    }

    void FindNearbyNPC()
    {
        nearbyNPC = null;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
        foreach (var hit in hits)
        {
            var npc = hit.GetComponent<NPCInteractable>();
            if (npc != null)
            {
                nearbyNPC = npc;
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
