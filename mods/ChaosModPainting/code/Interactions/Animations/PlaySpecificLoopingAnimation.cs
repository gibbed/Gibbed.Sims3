using System.Collections.Generic;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;

namespace ChaosMod.Interactions.Animations
{
    public class PlaySpecificLoopingAnimation : Interaction<Sim, IViewable>, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        public override string GetInteractionName()
        {
            return "Play Looping Animation";
        }

        protected override bool Run()
        {
            StateMachineClient client = StateMachineClient.Acquire(base.Actor, "single_looping_animation");
            client.SetActor("x", base.Actor);
            client.EnterState("x", "EnterExit");
            Definition interactionDefinition = base.InteractionDefinition as Definition;
            if (Sim.AnimationClipDataForCAS.SimCanPlayAnimation(base.Actor, interactionDefinition.ClipName))
            {
                client.SetParameter("AnimationClip", interactionDefinition.ClipName);
                client.RequestState("x", "Animate");
                base.DoLoop(~(ExitReason.MidRoutePushRequested | ExitReason.ObjectStateChanged | ExitReason.PlayIdle | ExitReason.MaxSkillPointsReached));
                client.RequestState("x", "EnterExit");
                return true;
            }
            return false;
        }

        // Nested Types
        [DoesntRequireTuning]
        private sealed class Definition : InteractionDefinition<Sim, IViewable, PlaySpecificLoopingAnimation>
        {
            // Fields
            public string ClipName;
            public string InteractionName;
            public string[] MenuPath;

            // Methods
            public Definition()
            {
            }

            public Definition(string interactionName, string clipName, string[] menuPath)
            {
                this.InteractionName = interactionName;
                this.ClipName = clipName;
                this.MenuPath = menuPath;
            }

            protected override void AddInteractions(InteractionObjectPair iop, Sim actor, IViewable target, List<InteractionObjectPair> results)
            {
                foreach (string str in Sim.AnimationClipDataForCAS.sProductionAnimationClassification.Keys)
                {
                    Sim.AnimationClipDataForCAS[] rcasArray;
                    if (Sim.AnimationClipDataForCAS.sProductionAnimationClassification.TryGetValue(str, out rcasArray))
                    {
                        foreach (Sim.AnimationClipDataForCAS rcas in rcasArray)
                        {
                            PlaySpecificLoopingAnimation.Definition interaction = new PlaySpecificLoopingAnimation.Definition(rcas.InGameTextForInteraction, rcas.AnimationClipName, new string[] { "Animate...", "Play Specific Looping Animation...", str });
                            results.Add(new InteractionObjectPair(interaction, target));
                        }
                    }
                }
            }

            protected override string GetInteractionName(Sim actor, IViewable target, InteractionObjectPair iop)
            {
                return this.InteractionName;
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