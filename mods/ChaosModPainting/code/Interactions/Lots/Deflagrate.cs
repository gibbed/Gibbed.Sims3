using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;

namespace ChaosMod.Interactions.Lots
{
    public class Deflagrate : Interaction<Sim, IViewable>
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            GameObject[] lotObjs = base.Target.LotCurrent.GetObjects<GameObject>();
            /*
            int saveme = FireManager.kMaxFiresPerLot;
            FireManager.kMaxFiresPerLot = lotObjs.Length;
            foreach (GameObject obj2 in lotObjs)
                FireManager.AddFire(obj2);
            FireManager.kMaxFiresPerLot = saveme;
            */
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : InteractionDefinition<Sim, IViewable, Deflagrate>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "Deflagrate Lot";
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
