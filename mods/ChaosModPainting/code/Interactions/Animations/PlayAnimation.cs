using System.Collections.Generic;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;

namespace ChaosMod.Interactions.Animations
{
    public class PlayAnimation : Interaction<Sim, IViewable>, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            StateMachineClient client = StateMachineClient.Acquire(base.Actor, "single_animation");
            client.SetActor("x", base.Actor);
            client.EnterState("x", "Enter");
            Definition interactionDefinition = base.InteractionDefinition as Definition;
            Sim.AnimationClipDataForCAS[] rcasArray = (interactionDefinition.MenuText == "Full Body Animation Suite") ? Sim.AnimationClipDataForCAS.sCasAnimations["CasFullBodyAnimations"] : Sim.AnimationClipDataForCAS.sCasAnimations["CasFaceAnimations"];
            foreach (Sim.AnimationClipDataForCAS rcas in rcasArray)
            {
                if (Sim.AnimationClipDataForCAS.SimCanPlayAnimation(base.Actor, rcas.AnimationClipName))
                {
                    client.SetParameter("AnimationClip", rcas.AnimationClipName);
                    client.RequestState("x", "Animate");
                    client.RequestState(false, "x", "Enter");
                }
            }
            return true;
        }

        [DoesntRequireTuning]
        private sealed class Definition : InteractionDefinition<Sim, IViewable, PlayAnimation>
        {
            public string[] MenuPath;
            public string MenuText;

            public Definition()
            {
            }

            public Definition(string menuText, string[] menuPath)
            {
                this.MenuText = menuText;
                this.MenuPath = menuPath;
            }

            protected override void AddInteractions(InteractionObjectPair iop, Sim actor, IViewable target, List<InteractionObjectPair> results)
            {
                string[] menuPath = ChaosModPainting.BuildPath(ChaosInteractionCategory.Animation, "CAS - Test Animations...");
                results.Add(new InteractionObjectPair(new PlayAnimation.Definition("Full Body Animation Suite", menuPath), target));
                results.Add(new InteractionObjectPair(new PlayAnimation.Definition("Face Animation Suite", menuPath), target));
            }

            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return this.MenuText;
            }

            public override string[] GetPath()
            {
                return this.MenuPath;
            }

            protected override bool Test(Sim a, IViewable target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
    }
}
