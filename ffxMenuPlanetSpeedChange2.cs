using DG.Tweening;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MainBpmChanger
{
    public class ffxMenuPlanetSpeedChange2 : ffxBase
    {
        private bool changed = false;
        private float pitch = -1;

        public override void Awake()
        {
            base.Awake();
            floor.topGlow.enabled = false;
            floor.floorIcon = FloorIcon.Rabbit;
            floor.UpdateIconSprite();
        }

        public void Start()
        {
            floor.topGlow.gameObject.SetActive(false);
            floor.bottomGlow.gameObject.SetActive(false);
        }
        
        public override void doEffect()
        {
            if (!changed)
            {
                floor.floorIcon = FloorIcon.Snail;
                pitch = -1;
                changed = true;
            }
            else
            {
                ctrl.speed = 1.0;
                floor.floorIcon = FloorIcon.Rabbit;
                cond.song.DOKill();
                cond.song2.DOKill();
                cond.song2.DOFade(0, 0.2f);
                cond.song.pitch = 0.8f;
                cond.song2.pitch = 0.8f;
                changed = false;
            }
            floor.UpdateIconSprite();
        }

        public void Update()
        {
            if (!changed)
                return;
            ctrl.speed = Main.Settings.multiplyMusic ? 1 : Main.Settings.pitch / 100;
            //ctrl.speed = Main.Settings.pitch / 100;
            if (Main.Settings.changeMusic && cond.song2.volume == 0)
            {
                DOTween.TweensByTarget(cond.song2)?.Where(tween => tween.stringId == "fade").ToList().ForEach(tween => tween.Kill());
                cond.song2.DOFade(0.7f, 0.2f).SetId("fade");
            }
            if (!Main.Settings.changeMusic && cond.song2.volume != 0)
            {
                DOTween.TweensByTarget(cond.song2)?.Where(tween => tween.stringId == "fade").ToList().ForEach(tween => tween.Kill());
                cond.song2.DOFade(0, 0.2f);
            }
            if (Main.Settings.multiplyMusic && pitch != Main.Settings.pitch / 100)
            {
                pitch = Main.Settings.pitch / 100;
                cond.song.pitch = pitch * 0.8f;
                cond.song2.pitch = pitch * 0.8f;
            }
            if (!Main.Settings.multiplyMusic && pitch != -1)
            {
                pitch = -1;
                cond.song.pitch = 0.8f;
                cond.song2.pitch = 0.8f;
            }
            if (cond.song.time != cond.song2.time)
                cond.song2.time = cond.song.time;
        }
    }
}
