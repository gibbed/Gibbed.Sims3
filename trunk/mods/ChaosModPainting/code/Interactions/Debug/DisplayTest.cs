using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;
using Sims3.UI;

namespace ChaosMod.Interactions.Debug
{
    public class DisplayTest : Interaction<Sim, IViewable>, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton;

        static DisplayTest()
        {
            Singleton = new Definition();
        }

        protected override bool Run()
        {
            base.StandardEntry();
            SimpleMessageDialog.Show("Test Dialog", "This is a test dialog.");
            base.StandardExit();
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : InteractionDefinition<Sim, IViewable, DisplayTest>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "Display Test";
            }

            protected override bool Test(Sim a, IViewable target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }

            public override string[] GetPath()
            {
                return ChaosModPainting.BuildPath(ChaosInteractionCategory.Debug);
            }
        }
    }
}
