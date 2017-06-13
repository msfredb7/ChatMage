namespace GameCondition
{
    public class LoseAfterXSeconds : BaseLosingCondition
    {
        public float delay = 10;

        public override void Init(PlayerController player, LevelScript levelScript)
        {
            levelScript.inGameEvents.AddDelayedAction(levelScript.Lose, delay);
        }
    }
}