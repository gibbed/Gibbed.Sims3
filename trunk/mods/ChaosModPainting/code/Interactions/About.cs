using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;
using Sims3.UI;

namespace ChaosMod.Interactions
{
    public class About : ImmediateInteraction<Sim, IViewable>, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            base.StandardEntry();

            SimpleMessageDialog.Show("About Chaos Mod Painting", "To be properly implemented.");
            
            base.StandardExit();
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : ImmediateInteractionDefinition<Sim, IViewable, About>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "About Chaos Mod Painting...";
            }

            protected override bool Test(Sim a, IViewable target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
    }
}
