using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsController : MonoBehaviour
{
    protected Joystick joystick;
    protected Joybutton joybutton;

    public float VDirection = 0f;
    public float HDirection = 0f;
    public bool jump;
    public bool fireAttack;
    public bool fireAbility;
    public bool cycleAbilityLeft;
    public bool cycleAbilityRight;

    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        joybutton = FindObjectOfType<Joybutton>();

    }

    // Update is called once per frame
    void Update()
    {
        ActiveInputs();
    }

    private void ActiveInputs()
    {
        VDirection = Input.GetAxis("Vertical");
        if (System.Math.Abs(joystick.Vertical) > System.Math.Abs(VDirection))
        {
            VDirection = joystick.Vertical;
        }

        HDirection = Input.GetAxis("Horizontal");
        if (System.Math.Abs(joystick.Horizontal) > System.Math.Abs(HDirection))
        {
            HDirection = joystick.Horizontal;
        }

        jump = (Input.GetButtonDown("Jump") || joybutton.Pressed);
        fireAttack = Input.GetButtonDown("Fire1");
        fireAbility = Input.GetButtonDown("Fire2");
        cycleAbilityLeft = Input.GetButtonDown("AbilitySwapLeft");
        cycleAbilityRight = Input.GetButtonDown("AbilitySwapRight");
    }


}
