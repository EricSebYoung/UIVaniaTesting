using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerThrow : PlayerAbilities
{
    protected override void StartSetup()
    {
        rb.velocity = transform.right * this.HorizontalSpeed;
    }

    protected override void UpdateSetup()
    {

    }

    protected override void ActionOnTimer()
    {
        DestroyAbility();
    }

    protected override void DestroyAbility()
    {
        Destroy(this.gameObject);
    }
}
