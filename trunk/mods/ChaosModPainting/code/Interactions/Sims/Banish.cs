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
    public class Banish : Interaction<Sim, IViewable>
    {
        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            base.StandardEntry();
            Sim bannedSim = base.GetSelectedObject() as Sim;
            if (bannedSim.LotHome != null && bannedSim.LotHome != base.Target.LotCurrent)
            {
                //Send sim home
            }
            else
            {
                //Send sim to furthest community lot or home lot
            }
            base.StandardExit();
            return true;
        }

        private class Definition : InteractionDefinition<Sim, IViewable, Banish>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "Banish Sim";
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
