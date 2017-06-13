
namespace GameCondition
{
    public class WinAfterXKills : BaseWinningCondition
    {
        public int amount = 10;

        private int currentKillCount = 0;

        public override void Init(PlayerController player, LevelScript levelScript)
        {
            player.playerStats.onUnitKilled += PlayerStats_onUnitKilled;
        }

        private void PlayerStats_onUnitKilled(Unit unit)
        {
            currentKillCount++;

            if(currentKillCount == amount)
            {
                Game.instance.currentLevel.Win();
            }
        }
    }
}