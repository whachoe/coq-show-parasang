namespace CJPA
{
    using XRL;
    using XRL.Core;
    using XRL.World;
    using XRL.World.Parts;

    [PlayerMutator]
    [HasCallAfterGameLoaded]
    public class ShowParasangPlayerMutator : IPlayerMutator
    {
        public void mutate(GameObject player)
        {
            // add our listener to the player when a New Game begins
            _ = player.AddPart<ShowParasong_EventListener>();
        }

        [CallAfterGameLoaded]
        public static void GameLoadedCallback()
        {
            // Called whenever loading a save game
            var player = XRLCore.Core?.Game?.Player?.Body;
            _ = (player?.RequirePart<ShowParasong_EventListener>());
        }
    }
}