using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;

namespace ChaosMod.Interactions.Sims
{
    public abstract class MaxMotivesInteraction : ImmediateInteraction<Sim, IViewable>
    {
        protected static void MaxMotives(Sim sim)
        {
            sim.Motives.ForceSetMax(CommodityKind.Hygiene);
            sim.Motives.ForceSetMax(CommodityKind.Bladder);
            sim.Motives.ForceSetMax(CommodityKind.Energy);
            sim.Motives.ForceSetMax(CommodityKind.Hunger);
            sim.Motives.ForceSetMax(CommodityKind.Fun);
            sim.Motives.ForceSetMax(CommodityKind.Social);
            sim.Motives.ForceSetMax(CommodityKind.Fatigue);
        }

        public abstract class MaxMotivesInteractionDefinition<TInteraction> : ImmediateInteractionDefinition<Sim, IViewable, TInteraction>
            where TInteraction : MaxMotivesInteraction, new()
        {
            public override string[] GetPath()
            {
                return ChaosModPainting.BuildPath(ChaosInteractionCategory.Sim, "Max Motives...");
            }
        }
    }
}
