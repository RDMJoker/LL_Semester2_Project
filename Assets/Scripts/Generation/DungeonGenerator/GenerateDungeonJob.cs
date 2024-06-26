using Cinemachine;
using Unity.Jobs;

namespace Generation.DungeonGenerator
{
    public class GenerateDungeonJob : IJob
    {
        readonly int startLevel;
        readonly int levelAmount;
        readonly LevelGenerator levelGenerator;

        public GenerateDungeonJob(int _startLevel, int _levelAmount, LevelGenerator _levelGenerator )
        {
            startLevel = _startLevel;
            levelAmount = _levelAmount;
            levelGenerator = _levelGenerator;
        }
        
        
        public void Execute()
        {
            for (int i = startLevel; i < levelAmount + 1; i++)
            {
                levelGenerator.ResetSeed();
                levelGenerator.InitMap(i);
            }
        }
    }
}