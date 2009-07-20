using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;
using Sims3.UI;

namespace ChaosMod.Interactions.World
{
    public class Dehydrate : ImmediateInteraction<Sim, IViewable>, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            base.StandardEntry();
            
            if (ChaosModPainting.FloodedObjects.Count <= 0)
            {
                SimpleMessageDialog.Show("Error", "No flooded objects on lot");
            }
            else
            {
                foreach (GameObject obj2 in ChaosModPainting.FloodedObjects)
                {
                    PuddleManager.RemoveFlowingPuddle(obj2);
                    ChaosModPainting.FloodedObjects.Remove(obj2);
                }
            }

            base.StandardExit();
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : ImmediateInteractionDefinition<Sim, IViewable, Dehydrate>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "Dehydrate Lot";
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
