using UnityEngine;

namespace KI.BehaviourTree
{
    public class BehaviourTreeAgentOCE : MonoBehaviour
    {
        /*
         *  -> Root (Selector)
         *      -> Aggro (Sequence)
         *          -> Sichtbereich Check (Leaf)
         *          -> At Player (Selector)
         *              -> Attackrange Check (Leaf)
         *              -> Move To Player (Leaf)
         *          -> Attack Player (Leaf)
         *      -> NonAggro (Selector)
         *
         * DAS IST DOCH ALLES VOLL DUMM UND KACKE WARUM SOLLTE ICH DAS TUN HILFE ICH HASSE ALLES AAAAA
         */
         
    }
}