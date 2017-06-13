namespace GameCondition
{
    public class WinAfterXSeconds : BaseWinningCondition
    {
        public float delay = 10;

        public override void Init(PlayerController player, LevelScript levelScript)
        {
            levelScript.inGameEvents.AddDelayedAction(levelScript.Win, delay);
        }
    }
}