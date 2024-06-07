using CombatSystems;
using UnityEngine;

namespace Scriptables.VFXScriptables
{
    [CreateAssetMenu(menuName = "Scriptables/VFX/ColorGradient", fileName = "NewColorGradient")]
    public class ColorGradient : ScriptableObject
    {
        [ColorUsage(true,true)]
        public Color InnerColor;
        [ColorUsage(true,true)]
        public Color OuterColor;

        public EDamageType DamageType;
    }
}