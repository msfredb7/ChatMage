namespace GameCondition
{
    public class WinAfterXSeconds : BaseWinningCondition
    {
        public float delay = 10;

        public override void Init(PlayerController player, LevelScript levelScript)
        {
            Game.instance.events.AddDelayedAction(levelScript.Win, delay);
        }
    }
}