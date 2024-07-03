using NaughtyAttributes;
using UnityEngine;

namespace ItemSystem
{
    public class TestExternalDropper : MonoBehaviour
    {
        [SerializeField] ItemDropper dropper;
        [SerializeField] ItemTypeDropTable customDropTable;
        [SerializeField] ItemRarityDropTable customRarityTable;


        [Button]
        void DropItemWithCustomTable()
        {
            dropper.DropItem(1, customDropTable);
        }
        
        [Button]
        void DropItemWithoutCustomTable()
        {
            dropper.DropItem();
        }

        [Button]
        void DropWithCustomRarity()
        {
            dropper.DropItem(1,null, customRarityTable);
        }

        [Button]
        void DropWithBothCustoms()
        {
            dropper.DropItem(1,customDropTable,customRarityTable);
        }
    }
}