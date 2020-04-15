using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbilities : MonoBehaviour
{
    [SerializeField] protected int Damage;
    [SerializeField] protected int Cost;
    [SerializeField] protected float TimeActive;
    [SerializeField] protected float HorizontalSpeed;

    private float timer;

    protected Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = TimeActive;
        StartSetup();
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer = timer - (Time.deltaTime);
        } else
        {
            DestroyAbility();
        }
        UpdateSetup();
    }

    protected abstract void StartSetup();

    protected abstract void UpdateSetup();

    protected abstract void ActionOnTimer();

    protected abstract void DestroyAbility();

    public int GetCost()
    {
        return Cost;
    }

    public int GetDamage()
    {
        return Damage;
    }
}
