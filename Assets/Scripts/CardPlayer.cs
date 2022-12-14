using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class CardPlayer : MonoBehaviour
{
    public Card chosenCard;
    public Transform atkPosRef;
    private Tweener animationTweener;
    public float Health;
    [SerializeField] private TMP_Text nameText;
    public TMP_Text healthText;
    public HealthBar healthBar;
    public AudioSource audioSource;
    public AudioClip damageClip;
    public TMP_Text NickName { get => nameText; }
    public bool IsReady = false;
    public PlayerStats stats = new PlayerStats
    {
        MaxHealth = 200,
        RestoreValue = 5,
        DamageValue = 10
    };

    private void Start()
    {
        Health = stats.MaxHealth;
    }
    public void SetStats(PlayerStats newStats, bool RestoreFullHealth = false)
    {
        this.stats = newStats;
        if (RestoreFullHealth)
            Health = stats.MaxHealth;
        UpdateHealthBar();
    }

    public Attack? AttackValue
    {
        get => chosenCard == null ? null : chosenCard.AttackValue;
        // get
        // {
        //     if (chosenCard == null)
        //     {
        //         return null;
        //     }
        //     else
        //     {
        //         return chosenCard.AttackValue;
        //     }
        // }
    }


    public void Reset()
    {
        if (chosenCard != null)
        {
            chosenCard.Reset();
        }
        chosenCard = null;
    }
    public void SetChosenCard(Card newCard)
    {
        if (chosenCard != null)
        {
            chosenCard.transform.DOKill();
            chosenCard.Reset();
        }

        chosenCard = newCard;
        chosenCard.transform.DOScale(chosenCard.transform.localScale * 1.2f, 0.2f);
    }
    
    public void UpdateHealthBar()
    {
        //healtbar
        healthBar.UpdateBar(Health / stats.MaxHealth);
        //text
        healthText.text = Health + "/" + stats.MaxHealth;
    }
    public void ChangeHealth(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, stats.MaxHealth);
        UpdateHealthBar();
    }



    public void AnimateAttack()
    {
        animationTweener = chosenCard.transform
            .DOMove(atkPosRef.position, 0.5f);

    }


    public void AnimateDamage()
    {
        audioSource.PlayOneShot(damageClip);
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
            .DOColor(Color.red, 0.1f)
            .SetLoops(3, LoopType.Yoyo)
            .SetDelay(0.2f);


    }
    public void AnimateDraw()
    {
        animationTweener = chosenCard.transform
            .DOMove(chosenCard.OriginalPosition, 1)
            .SetDelay(0.2f)
            .SetEase(Ease.InBack);
    }



    public bool IsAnimating()
    {
        return animationTweener.IsActive();
    }
    public void isClickable(bool value)
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.SetClickable(value);
        }
    }
}
