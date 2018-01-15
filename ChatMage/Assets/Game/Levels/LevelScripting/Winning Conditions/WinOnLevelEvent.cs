
namespace GameCondition
{
    public class WinOnLevelEvent : BaseWinningCondition
    {
        public string eventName;

        public override void Init(PlayerController player, LevelScript levelScript)
        {
            levelScript.onEventReceived += LevelScript_onEventReceived;
        }

        private void LevelScript_onEventReceived(string text)
        {
            if (text == eventName)
                Game.Instance.levelScript.Win();
        }
    }
}