namespace GameCondition
{
    public class LoseOnPlayerDeath : BaseLosingCondition
    {
        public override void Init(PlayerController player, LevelScript levelScript)
        {
            player.vehicle.OnDeath += delegate (Unit unit)
            {
                levelScript.Lose();
            };
        }
    }
}