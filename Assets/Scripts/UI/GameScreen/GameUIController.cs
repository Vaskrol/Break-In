﻿using System;
using Basics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScreen {
    class GameUIController : MbSingleton<GameUIController> {

        [SerializeField] public BarController HealthBar;
        [SerializeField] public Image         Fader;

        public void FadeGame(bool fade, Action onComplete) {
            Fader.gameObject.SetActive(true);
            var startAlpha = fade ? 0f : 1f;
            Fader.color = new Color(0, 0, 0, startAlpha);
            var targetAlpha = fade ? 1f : 0f;
            LeanTween.alpha(Fader.rectTransform, targetAlpha, 1f)
                .setOnComplete(onComplete);
        }
    }
}

