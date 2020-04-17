using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;

public class OrbVisual : MonoBehaviour
{
    public static OrbSystem playerOrbsStatic;

    [SerializeField] private Sprite orb0Sprite;
    [SerializeField] private Sprite orb1Sprite;
    [SerializeField] private AnimationClip orbFullAnimation;

    private List<OrbImage> orbImageList;
    private OrbSystem orbSystem;
    private bool IsRecovering;

    private void Awake()
    {
        orbImageList = new List<OrbImage>();
    }

    private void Start()
    {
        FunctionPeriodic.Create(RecoveringAnimationPeriodic, .10f);

        OrbSystem orbSystem = new OrbSystem(4);
        SetOrbSystem(orbSystem);

        //Use Test
        CMDebug.ButtonUI(new Vector2(-50, 100), "1 Spent", () => orbSystem.Use(1));

        //Recover Test
        CMDebug.ButtonUI(new Vector2(50, 100), "1 Recovered", () => orbSystem.Recover(1));

    }

    public void SetOrbSystem(OrbSystem orbSystem)
    {
        this.orbSystem = orbSystem;
        playerOrbsStatic = orbSystem;

        List<OrbSystem.Orb> orbList = orbSystem.GetOrbList();
        Vector2 orbAnchoredPosition = new Vector2(0, 0);
        for (int i = 0; i < orbList.Count; i++)
        {
            OrbSystem.Orb orb = orbList[i];
            CreateOrbImage(orbAnchoredPosition).SetOrbFraction(orb.GetFractionsAmount());
            orbAnchoredPosition += new Vector2(30, 0);
        }

        orbSystem.OnUsed += OrbSystem_OnUsed;
        orbSystem.OnRecovered += OrbSystem_OnRecovered;
        orbSystem.OnEmpty += OrbSystem_OnEmpty;
    }

    private void OrbSystem_OnUsed(object sender, System.EventArgs e)
    {
        //Orb Used
        RefreshAllOrb();
    }

    private void OrbSystem_OnRecovered(object sender, System.EventArgs e)
    {
        //Orb Recovered
        //RefreshAllOrb();
        IsRecovering = true;
    }

    private void OrbSystem_OnEmpty(object sender, System.EventArgs e)
    {
        //todo: Can't Cast

        //Empty Orb Test
        CMDebug.TextPopupMouse("Insufficient AP!");
    }

    private void RefreshAllOrb()
    {
        List<OrbSystem.Orb> orbList = orbSystem.GetOrbList();
        for (int i = 0; i < orbImageList.Count; i++)
        {
            OrbImage orbImage = orbImageList[i];
            OrbSystem.Orb orb = orbList[i];
            orbImage.SetOrbFraction(orb.GetFractionsAmount());
        }
    }

    private void RecoveringAnimationPeriodic()
    {
        if (IsRecovering)
        {
            bool fullyRecovered = true;
            List<OrbSystem.Orb> orbList = orbSystem.GetOrbList();
            for (int i = 0; i < orbList.Count; i++)
            {
                OrbImage orbImage = orbImageList[i];
                OrbSystem.Orb orb = orbList[i];
                if (orbImage.GetFractionsAmount() != orb.GetFractionsAmount())
                {
                    //Visual is different from logic
                    orbImage.AddOrbVisualFractions();
                    if (orbImage.GetFractionsAmount() == OrbSystem.MAX_FRACTION_AMOUNT)
                    {
                        //Orb is Filled, Play Animation
                        orbImage.PlayOrbFullAnimation();
                    }
                    fullyRecovered = false;
                    break;
                }
            }
            if (fullyRecovered)
            {
                IsRecovering = false;
            }
        }
    }

    private OrbImage CreateOrbImage(Vector2 anchoredPosition)
    {
        //Game Object Creation
        GameObject orbGameObject = new GameObject("Orb", typeof(Image), typeof(Animation));

        //Set as child of this transform
        orbGameObject.transform.parent = transform;
        orbGameObject.transform.localPosition = Vector3.zero;

        //Location and Size
        orbGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        orbGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(31, 31);

        orbGameObject.GetComponent<Animation>().AddClip(orbFullAnimation, "OrbAnimation");

        //Set orb sprite
        Image orbImageUI = orbGameObject.GetComponent<Image>();
        orbImageUI.sprite = orb0Sprite;

        OrbImage orbImage = new OrbImage(this, orbImageUI, orbGameObject.GetComponent<Animation>());
        orbImageList.Add(orbImage);

        return orbImage;
    }

    //Single Orb Container
    public class OrbImage
    {
        private int fractions;
        private Image orbImage;
        private OrbVisual orbVisual;
        private Animation animation;

        public OrbImage(OrbVisual orbVisual, Image orbImage, Animation animation)
        {
            this.orbImage = orbImage;
            this.orbVisual = orbVisual;
            this.animation = animation;
        }

        public void SetOrbFraction(int fractions)
        {
            this.fractions = fractions;
            switch (fractions)
            {
                case 0:
                    orbImage.sprite = orbVisual.orb0Sprite;
                    break;
                case 1:
                    orbImage.sprite = orbVisual.orb1Sprite;
                    break;
            }
        }

        public int GetFractionsAmount()
        {
            return fractions;
        }

        public void AddOrbVisualFractions()
        {
            SetOrbFraction(fractions + 1);
        }

        public void PlayOrbFullAnimation()
        {
            animation.Play("OrbAnimation", PlayMode.StopAll);
        }
    }
}
