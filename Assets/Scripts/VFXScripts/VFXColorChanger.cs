using System;
using Scriptables.VFXScriptables;
using UnityEngine;
using UnityEngine.VFX;

namespace VFXScripts
{
    public class VFXColorChanger : MonoBehaviour
    {
        VisualEffect visualEffect;
        
        void Awake()
        {
            visualEffect = GetComponent<VisualEffect>();
        }

        public void ChangeColor(ColorGradient _gradient)
        {
            visualEffect.SetVector4("InnerGradientColor", _gradient.InnerColor);
            visualEffect.SetVector4("OuterGradientColor", _gradient.OuterColor);
        }
    }
}