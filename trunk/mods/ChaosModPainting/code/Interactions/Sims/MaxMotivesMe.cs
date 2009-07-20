using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;

namespace ChaosMod.Interactions.Sims
{
    public class MaxMotivesMe : MaxMotivesInteraction, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton;

        static MaxMotivesMe()
        {
            Singleton = new Definition();
        }

        protected override bool Run()
        {
            MaxMotives(this.Actor);
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : InteractionDefinition<Sim, IViewable, MaxMotivesMe>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "Me";
            }

            protected override bool Test(Sim a, IViewable target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }

            public override string[] GetPath()
            {
                return ChaosModPainting.BuildPath(ChaosInteractionCategory.Sim, MaxMotivesInteraction.Path);
            }
        }
    }
}
