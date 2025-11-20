using System.Collections;
using UnityEngine;

public class Enemy2DRespawn : MonoBehaviour
{
    public string enemyID;
    public Animator animator;
    public float respawnDelay = 60f;
    public float dieAnimationDuration = 1f;

    private Collider2D[] colliders;
    private Renderer[] renderers;
    private EnemyAI2D ai;

    void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>();
        renderers = GetComponentsInChildren<Renderer>();
        ai = GetComponent<EnemyAI2D>();
    }

    void Start()
    {
        if (EnemyBattleData.IsDefeated(enemyID))
            StartCoroutine(DieAndRespawn());
        else
            SetState(true);

        IEnumerator DieAndRespawn()
        {

            SetState(false);


            foreach (var r in renderers) r.enabled = true;


            if (animator != null)
                animator.Play("Die");


            yield return new WaitForSeconds(dieAnimationDuration);


            foreach (var r in renderers) r.enabled = false;


            yield return new WaitForSeconds(respawnDelay);

            foreach (var r in renderers) r.enabled = true;
            SetState(true);

            if (animator != null)
            {
                animator.Play("GreenSlime_idle");
                animator.Update(0f);
            }
        }

        void SetState(bool state)
        {
            foreach (var c in colliders) c.enabled = state;
            if (ai != null) ai.enabled = state;
        }
    }
}
