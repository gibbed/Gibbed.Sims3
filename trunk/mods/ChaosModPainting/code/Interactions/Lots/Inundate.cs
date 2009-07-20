using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;

namespace ChaosMod.Interactions.Lots
{
    public class Inundate : Interaction<Sim, IViewable>, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            GameObject[] lotObjs = base.Target.LotCurrent.GetObjects<GameObject>();
            PuddleManager.FlowingPuddleTuning noah = new PuddleManager.FlowingPuddleTuning();
            noah.MaxNumPuddles = 100;
            noah.SpawnSpeedInMinutes = 1;
            foreach (GameObject obj2 in lotObjs)
            {
                PuddleManager.AddFlowingPuddle(obj2, noah);
                ChaosModPainting.FloodedObjects.Add(obj2);
            }
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : InteractionDefinition<Sim, IViewable, Inundate>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "Inundate Lot";
            }

            protected override bool Test(Sim a, IViewable target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }

            public override string[] GetPath()
            {
                return ChaosModPainting.BuildPath(ChaosInteractionCategory.Lot);
            }
        }
    }
}
