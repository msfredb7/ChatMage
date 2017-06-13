namespace GameCondition
{
    public class LoseOnPlayerDeath : BaseLosingCondition
    {
        public override void Init(PlayerController player, LevelScript levelScript)
        {
            player.vehicle.onDeath += delegate (Unit unit)
            {
                levelScript.Lose();
            };
        }
    }
}