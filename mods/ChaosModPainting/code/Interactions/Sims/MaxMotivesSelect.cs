using System.Collections.Generic;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;
using Sims3.UI;

namespace ChaosMod.Interactions.Sims
{
    public class MaxMotivesSelect : MaxMotivesInteraction, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton;

        static MaxMotivesSelect()
        {
            Singleton = new Definition();
        }

        protected override bool Run()
        {
            Sim selSim = this.GetSelectedObject() as Sim;
            MaxMotives(selSim);
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : MaxMotivesInteractionDefinition<MaxMotivesSelect>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "Select Sim on Lot";
            }

            public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
            {
                NumSelectableRows = 1;
                Sim actor = parameters.Actor as Sim;
                Lot curLot = actor.LotCurrent;
                base.PopulateSimPicker(ref parameters, out listObjs, out headers, curLot.GetSims(), false);
            }

            protected override bool Test(Sim a, IViewable target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
    }
}
