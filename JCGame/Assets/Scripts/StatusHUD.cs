using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusHUD : MonoBehaviour
{
    public Image statusHPBar;
    public TextMeshProUGUI statusHPValue;
    public Image statusMPBar;
    public TextMeshProUGUI statusMPValue;

    public void SetStatusHUD(PlayerStats status)
    {
        float currentHealth = status.currHealth;
        float currentMana = status.currMana;

        statusHPBar.fillAmount = currentHealth / status.maxHealth;
        statusHPValue.SetText(status.currHealth + "/" + status.maxHealth);

        statusMPBar.fillAmount = currentMana / status.maxMana;
        statusMPValue.SetText(status.currMana + "/" + status.maxMana);
    }
    
    public void SetStatusHUDEnemy(EnemyStats status)
    {
        float currentHealth = status.currHealth;
        float currentMana = status.currMana;

        statusHPBar.fillAmount = currentHealth / status.maxHealth;
        statusHPValue.SetText(status.currHealth + "/" + status.maxHealth);

        statusMPBar.fillAmount = currentMana / status.maxMana;
        statusMPValue.SetText(status.currMana + "/" + status.maxMana);
    }

    public void SetHP(PlayerStats status, float hp)
    {
        StartCoroutine(GraduallySetStatusBar(status, hp, false, 10, 0.05f));
    }
    
    public void SetHPEnemy(EnemyStats status, float hp)
    {
        StartCoroutine(GraduallySetStatusBarEnemy(status, hp, false, 10, 0.05f));
    }

    IEnumerator GraduallySetStatusBar(PlayerStats status, float amount, bool isIncrease, int fillTimes, float fillDelay)
    {
        float percentage = 1 / (float)fillTimes;
        float tempHealth = status.currHealth;

        if (isIncrease)
        {
            status.currHealth += (int)amount;
            for (int fillStep = 0; fillStep < fillTimes; fillStep++)
            {
                float _fAmount = amount * percentage;
                float _dAmount = _fAmount / status.maxHealth;
                tempHealth += _fAmount;
                statusHPBar.fillAmount += _dAmount;
                if (status.currHealth <= status.maxHealth)
                    statusHPValue.SetText((int)tempHealth + "/" + status.maxHealth);

                if (fillStep == fillTimes - 1)
                {
                    statusHPValue.SetText(status.currHealth + "/" + status.maxHealth);
                    statusHPBar.fillAmount = (float)status.currHealth / status.maxHealth;
                }

                yield return new WaitForSeconds(fillDelay);
            }
        }
        else
        {
            status.currHealth -= (int)amount;
            for (int fillStep = 0; fillStep < fillTimes; fillStep++)
            {
                float _fAmount = amount * percentage;
                float _dAmount = _fAmount / status.maxHealth;
                tempHealth -= _fAmount;
                statusHPBar.fillAmount -= _dAmount;
                if (status.currHealth >= 0)
                    statusHPValue.SetText((int)tempHealth + "/" + status.maxHealth);
                
                if(fillStep == fillTimes - 1)
                {
                    statusHPValue.SetText(status.currHealth + "/" + status.maxHealth);
                    statusHPBar.fillAmount = (float)status.currHealth / status.maxHealth;
                }

                yield return new WaitForSeconds(fillDelay);
            }
        }
    }
    
    IEnumerator GraduallySetStatusBarEnemy(EnemyStats status, float amount, bool isIncrease, int fillTimes, float fillDelay)
    {
        float percentage = 1 / (float)fillTimes;
        float tempHealth = status.currHealth;

        if (isIncrease)
        {
            status.currHealth += (int)amount;
            for (int fillStep = 0; fillStep < fillTimes; fillStep++)
            {
                float _fAmount = amount * percentage;
                float _dAmount = _fAmount / status.maxHealth;
                tempHealth += _fAmount;
                statusHPBar.fillAmount += _dAmount;
                if (status.currHealth <= status.maxHealth)
                    statusHPValue.SetText((int)tempHealth + "/" + status.maxHealth);

                if (fillStep == fillTimes - 1)
                {
                    statusHPValue.SetText(status.currHealth + "/" + status.maxHealth);
                    statusHPBar.fillAmount = (float)status.currHealth / status.maxHealth;
                }

                yield return new WaitForSeconds(fillDelay);
            }
        }
        else
        {
            status.currHealth -= (int)amount;
            for (int fillStep = 0; fillStep < fillTimes; fillStep++)
            {
                float _fAmount = amount * percentage;
                float _dAmount = _fAmount / status.maxHealth;
                tempHealth -= _fAmount;
                statusHPBar.fillAmount -= _dAmount;
                if (status.currHealth >= 0)
                    statusHPValue.SetText((int)tempHealth + "/" + status.maxHealth);

                if (fillStep == fillTimes - 1)
                {
                    statusHPValue.SetText(status.currHealth + "/" + status.maxHealth);
                    statusHPBar.fillAmount = (float)status.currHealth / status.maxHealth;
                }

                yield return new WaitForSeconds(fillDelay);
            }
        }
    }
}
