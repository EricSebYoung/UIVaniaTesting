using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeThrow : PlayerAbilities
{
    [SerializeField] private float verticalForce;

    protected override void StartSetup()
    {
        Rigidbody2D playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        verticalForce = verticalForce + playerRB.velocity.y;
        this.HorizontalSpeed = this.HorizontalSpeed + System.Math.Abs(playerRB.velocity.x);
        rb.velocity = transform.right * this.HorizontalSpeed;
        rb.velocity = new Vector2(rb.velocity.x, verticalForce);
    }

    protected override void UpdateSetup()
    {
        transform.Rotate(new Vector3(0, 0, -720 * Time.deltaTime));
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
