using System;
using System.Collections;
using DG.Tweening;
using Scriptables.VFXScriptables;
using UnityEngine;
using UnityEngine.VFX;

namespace VFXScripts
{
    public class VFXColorChanger : MonoBehaviour
    {
        VisualEffect visualEffect;
        const float duration = 0.5f;

        void Awake()
        {
            visualEffect = GetComponent<VisualEffect>();
        }

        public void ChangeColor(ColorGradient _gradient)
        {
            visualEffect.SetVector4("InnerGradientColor", _gradient.InnerColor);
            visualEffect.SetVector4("OuterGradientColor", _gradient.OuterColor);
        }

        public void ChangeColor(Color _color, string _colorName = "MainColor", bool _instantChange = false)
        {
            if (!_instantChange)
            {
                var colorStart = visualEffect.GetVector4(_colorName);
                StartCoroutine(LerpColorChange(colorStart,_colorName,_color));
            }

            visualEffect.SetVector4(_colorName, _color);
        }

        IEnumerator LerpColorChange(Color _startColor, string _colorName, Color _endColor)
        {
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                visualEffect.SetVector4(_colorName, Color.Lerp(_startColor,_endColor, timer / duration));
                yield return null;
            }
        }
    }
}