using UnityEngine;

public class PrinceHeal : MonoBehaviour
{
    public float healRadius = 3f;
    public int healAmount = 10;
    public float healInterval = 1f;

    private float lastHealTime;

    void Update()
    {
        if (Time.time < lastHealTime + healInterval)
            return;

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, healRadius);

        foreach (Collider2D t in targets)
        {
            PlayerHealth player = t.GetComponent<PlayerHealth>();
            PrincessHealth princess = t.GetComponent<PrincessHealth>();

            if (player != null)
                HealPlayer(player);

            if (princess != null)
                HealPrincess(princess);
        }

        lastHealTime = Time.time;
    }

    void HealPlayer(PlayerHealth hp)
    {
        int heal = Mathf.Min(healAmount, hp.maxHealth - hp.CurrentHealth);
        if (heal > 0)
            hp.Heal(heal);
    }

    void HealPrincess(PrincessHealth hp)
    {
        int heal = Mathf.Min(healAmount, hp.maxHealth - hp.CurrentHealth);
        if (heal > 0)
            hp.Heal(heal);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRadius);
    }
}