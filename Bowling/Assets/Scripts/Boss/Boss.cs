using UnityEngine;

public class Boss : MonoBehaviour
{
    //Š®‘S–³“G‚©‚Ç‚¤‚©
    bool isPerfectInvincible = true;

    //–³“G‚©‚Ç‚¤‚©
    bool isPerfect = false;

    //–³“GŽžŠÔ
    float isInvincibleTime;

    int hp = 50;

    public void FalseIsPerfectInvincible()
    {
        isPerfectInvincible = false;
    }

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

        if (isInvincibleTime > 5f)
        {
            isInvincibleTime = 0f;
            isPerfect = false;
        }

        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

        //‰¼
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage();
        }

    }

    public void TakeDamage()
    {
        //–³“G‚Å‚ ‚ê‚ÎˆÈ‰º‚Ìˆ—‚ðs‚í‚È‚¢
        if (isPerfectInvincible) return;

        //–³“G’†‚Å‚È‚¯‚ê‚ÎUŒ‚‚ª—˜‚­
        if (!isPerfect)
        {
            hp--;
            Debug.Log(hp);
            isPerfect = true;
        }
    }
}