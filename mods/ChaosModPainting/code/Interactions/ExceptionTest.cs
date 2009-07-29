using System;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;

namespace ChaosMod.Interactions
{
    public class ExceptionTest : ImmediateInteraction<Sim, IViewable>, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            base.StandardEntry();

            throw new InvalidProgramException("this is a test of exception handling!!");

            base.StandardExit();
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : ImmediateInteractionDefinition<Sim, IViewable, ExceptionTest>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "Exception Test";
            }

            public override string[] GetPath()
            {
                return ChaosModPainting.BuildPath(ChaosInteractionCategory.Debug);
            }

            protected override bool Test(Sim a, IViewable target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
    }
}
