using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;

public class HeartsVisual : MonoBehaviour
{
    public static HeartsSystem playerHeartsStatic;

    [SerializeField] private Sprite heart0Sprite;
    [SerializeField] private Sprite heart1Sprite;
    [SerializeField] private Sprite heart2Sprite;
    [SerializeField] private Sprite heart3Sprite;
    [SerializeField] private Sprite heart4Sprite;
    [SerializeField] private AnimationClip heartFullAnimation;
    [SerializeField] private AudioSource heartFractionSound;
    [SerializeField] private AudioSource heartFullSound;

    private List<HeartImage> heartImageList;
    private HeartsSystem heartsSystem;
    private bool IsHealing;

    private void Awake()
    {
        heartImageList = new List<HeartImage>();
    }

    private void Start()
    {
        FunctionPeriodic.Create(HealingAnimationPeriodic, .10f);

        HeartsSystem heartsSystem = new HeartsSystem(4);
        SetHeartsSystem(heartsSystem);

        //Damage Test
        //CMDebug.ButtonUI(new Vector2(-50, -100), "1 Damage", () => heartsSystem.Damage(1));
        //CMDebug.ButtonUI(new Vector2(50, -100), "4 Damage", () => heartsSystem.Damage(4));

        //Heal Test
        //CMDebug.ButtonUI(new Vector2(-50, -200), "1 Heal", () => heartsSystem.Heal(1));
        //CMDebug.ButtonUI(new Vector2(50, -200), "4 Heal ", () => heartsSystem.Heal(4));

    }

    private void SetHeartsSystem(HeartsSystem heartsSystem)
    {
        this.heartsSystem = heartsSystem;
        playerHeartsStatic = heartsSystem;

        List<HeartsSystem.Heart> heartList = heartsSystem.GetHeartList();
        Vector2 heartAnchoredPosition = new Vector2(0, 0);
        for (int i = 0; i < heartList.Count; i++)
        {
            HeartsSystem.Heart heart = heartList[i];
            CreateHeartImage(heartAnchoredPosition).SetHeartFraction(heart.GetFractionsAmount());
            heartAnchoredPosition += new Vector2(30, 0);
        }

        heartsSystem.OnDamaged += HeartsSystem_OnDamaged;
        heartsSystem.OnHealed += HeartsSystem_OnHealed;
        heartsSystem.OnDead += HeartsSystem_OnDead;
    }

    private void HeartsSystem_OnDamaged(object sender, System.EventArgs e)
    {
        //Hearts Damaged
        RefreshAllHearts();
    }

    private void HeartsSystem_OnHealed(object sender, System.EventArgs e)
    {
        //Hearts Healed
        //RefreshAllHearts();
        IsHealing = true;
    }

    private void HeartsSystem_OnDead(object sender, System.EventArgs e)
    {
        //todo: Game Over
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Animator anim = player.GetComponent<Animator>();

        anim.SetTrigger("death");

        //Game Over Detection Test
        //CMDebug.TextPopupMouse("Game Over!");
    }

    private void RefreshAllHearts()
    {
        List<HeartsSystem.Heart> heartList = heartsSystem.GetHeartList();
        for (int i = 0; i < heartImageList.Count; i++)
        {
            HeartImage heartImage = heartImageList[i];
            HeartsSystem.Heart heart = heartList[i];
            heartImage.SetHeartFraction(heart.GetFractionsAmount());
        }
    }

    private void HealingAnimationPeriodic()
    {
        if (IsHealing)
        {
            bool fullyHealed = true;
            List<HeartsSystem.Heart> heartList = heartsSystem.GetHeartList();
            for (int i = 0; i < heartList.Count; i++)
            {
                HeartImage heartImage = heartImageList[i];
                HeartsSystem.Heart heart = heartList[i];
                if (heartImage.GetFractionsAmount() != heart.GetFractionsAmount())
                {
                    //Visual is different from logic
                    heartImage.AddHeartVisualFractions();
                    if (heartImage.GetFractionsAmount() == HeartsSystem.MAX_FRACTION_AMOUNT)
                    {
                        //Heart is Filled, Play Animation
                        heartImage.PlayHeartFullAnimation();
                        heartFullSound.Play();
                    } else
                    {
                        heartFractionSound.Play();
                    }
                    fullyHealed = false;
                    break;
                }
            }
            if (fullyHealed)
            {
                IsHealing = false;
            }
        }
    }

    private HeartImage CreateHeartImage(Vector2 anchoredPosition)
    {
        //Game Object Creation
        GameObject heartGameObject = new GameObject("Heart", typeof(Image), typeof(Animation));

        //Set as child of this transform
        heartGameObject.transform.parent = transform;
        heartGameObject.transform.localPosition = Vector3.zero;

        //Location and Size
        heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(31, 31);

        heartGameObject.GetComponent<Animation>().AddClip(heartFullAnimation, "HeartAnimation");

        //Set heart sprite
        Image heartImageUI = heartGameObject.GetComponent<Image>();
        heartImageUI.sprite = heart0Sprite;

        HeartImage heartImage = new HeartImage(this, heartImageUI, heartGameObject.GetComponent<Animation>());
        heartImageList.Add(heartImage);

        return heartImage;
    }

    //Single Heart Container
    public class HeartImage
    {
        private int fractions;
        private Image heartImage;
        private HeartsVisual heartsVisual;
        private Animation animation;

        public HeartImage(HeartsVisual heartsVisual, Image heartImage, Animation animation)
        {
            this.heartImage = heartImage;
            this.heartsVisual = heartsVisual;
            this.animation = animation;
        }

        public void SetHeartFraction(int fractions)
        {
            this.fractions = fractions;
            switch (fractions)
            {
                case 0:
                    heartImage.sprite = heartsVisual.heart0Sprite;
                    break;
                case 1:
                    heartImage.sprite = heartsVisual.heart1Sprite;
                    break;
                case 2:
                    heartImage.sprite = heartsVisual.heart2Sprite;
                    break;
                case 3:
                    heartImage.sprite = heartsVisual.heart3Sprite;
                    break;
                case 4:
                    heartImage.sprite = heartsVisual.heart4Sprite;
                    break;
            }
        }

        public int GetFractionsAmount()
        {
            return fractions;
        }

        public void AddHeartVisualFractions()
        {
            SetHeartFraction(fractions + 1);
        }

        public void PlayHeartFullAnimation()
        {
            animation.Play("HeartAnimation", PlayMode.StopAll);
        }

    }
}
