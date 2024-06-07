using System.Collections.Generic;
using Scriptables.VFXScriptables;
using UnityEngine;

namespace Scriptables.Lists
{
    [CreateAssetMenu(menuName = "Scriptables/Lists/ColorGradientList", fileName = "NewGradientList")]
    public class ColorGradientList : ScriptableObject
    {
        public List<ColorGradient> colorGradients;
    }
}