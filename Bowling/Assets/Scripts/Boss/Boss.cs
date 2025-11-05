using UnityEngine;

public class Boss : MonoBehaviour
{
    //Š®‘S–³“G‚©‚Ç‚¤‚©
    bool isPerfectInvincible = true;

    //–³“G‚©‚Ç‚¤‚©
    bool isPerfect = false;

    //–³“GŠÔ
    float isInvincibleTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPerfect)
        {
            isInvincibleTime++;
        }

        if(isInvincibleTime > 5f)
        {
            isInvincibleTime = 0f;
            isPerfect = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //–³“G‚Å‚ ‚ê‚ÎˆÈ‰º‚Ìˆ—‚ğs‚í‚È‚¢
            if (isPerfectInvincible) return;

            //–³“G’†‚Å‚È‚¯‚ê‚ÎUŒ‚‚ª—˜‚­
            if (!isPerfect)
            {
                Debug.Log("aaaaaaaaa");
                isPerfect = true;
            }
        }
    }
}
