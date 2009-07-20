using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;

namespace ChaosMod.Interactions.Sims
{
    public class MaxMotivesAll : MaxMotivesInteraction, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            Lot thisLot = base.Actor.LotCurrent;
            
            if (thisLot == null)
            {
                return false;
            }

            foreach (Sim sim in thisLot.GetSims())
            {
                MaxMotives(sim);
            }

            MaxMotives(this.Actor);
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : InteractionDefinition<Sim, IViewable, MaxMotivesAll>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "All Sims on Lot";
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
