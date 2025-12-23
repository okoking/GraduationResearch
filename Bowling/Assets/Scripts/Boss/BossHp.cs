using Unity.VisualScripting;
using UnityEngine;

public class BossHp : MonoBehaviour
{

    public int maxHp;       //Å‘å‘Ì—Í
    private int currentHp;  //Œ»İ‘Ì—Í

    public bool isPerfectInvincible;    //Š®‘S‚É–³“G
    private bool isInvicible;           //–³“G

    public float maxInvincibleTime;     //Å‘å–³“GŠÔ
    private float invincibleTime;       //–³“GŠÔ

    private bool isDeath;               //€‚ñ‚Å‚¢‚é‚©

    public bool GetIsPerfectInvincible() {  return isPerfectInvincible; }
    public void SetIsPerfectInvincible(bool flg) { isPerfectInvincible = flg; }

    public bool GetIsInvicible() { return isInvicible; }
    public bool GetIsDeath() { return isDeath; }

    public int GetCurrentHp() {  return currentHp; }

    public float GetRatio() {  return currentHp / (float)maxHp; }

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        isDeath = false;
    }

    //Update is called once per frame
    void Update()
    {

        if (isInvicible)    //–³“G’†‚Å‚ ‚ê‚ÎŠÔ‚ğƒJƒEƒ“ƒg
        {
            invincibleTime += Time.deltaTime;
        }

        if (invincibleTime > maxInvincibleTime) //–³“GŠÔ‚ğ’´‚¦‚½‚ç–³“G‰ğœ
        {
            invincibleTime = 0f;
            isInvicible = false;
        }

        if(currentHp <= 0)  //0ˆÈ‰º‚É‚È‚Á‚½‚çE‚·
        {
            isDeath = true;
        }
    }

    public void TakeDamage(int hp)
    {
        if (isPerfectInvincible) return;    //Š®‘S–³“G‚Å‚ ‚ê‚Îƒ_ƒ[ƒW–³Œø

        if (!isInvicible)                   //–³“G‚Å‚È‚¯‚ê‚Îƒ_ƒ[ƒW‚ª’Ê‚é
        {
            currentHp -= hp;
            Debug.Log(currentHp);
            isInvicible = true;
        }
    }

    public void Recovery(int hp)
    {
        currentHp += hp;

        if (currentHp >= maxHp)  //Å‘å‘Ì—Í‚ğ’´‚¦‚È‚¢‚æ‚¤‚É‚·‚é
        {
            currentHp = maxHp;
            Debug.Log(currentHp);
        }
    }

    public void ThisDestroy()
    {
        if (isDeath)
        {
            Destroy(gameObject);
        }
    }
}
