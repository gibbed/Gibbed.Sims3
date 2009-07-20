using System.Collections.Generic;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;
using Sims3.UI;

namespace ChaosMod.Interactions.Sims
{
    public class Summon : ImmediateInteraction<Sim, IViewable>, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        public override string GetInteractionName()
        {
            return "Summon Sim";
        }

        protected override bool Run()
        {
            base.StandardEntry();
            SimpleMessageDialog.Show("Hooray!", "Run() is being called!");
            /*SimDescription selSim = base.GetSelectedObject() as SimDescription;
            string simName = selSim.FirstName + " " + selSim.LastName;
            SimpleMessageDialog.Show("Selected Sim:", simName);
            /*Sim simInst = selSim.CreatedSim;
            int hasSim = (simInst == null) ? 0 : 1;
            Vector3 simPos = new Vector3(0,0,0);
            Vector3 simFor = new Vector3(0,0,0);
            Vector3 objPos = base.Target.Position;
            string DebugData = string.Format("objPos: {0:F3}, {0:F3}, {0:F3}\nhasSim: {0:D1}", objPos.x, objPos.y, objPos.z, hasSim);
            SimpleMessageDialog.Show("Debug Data", DebugData);
            if (GlobalFunctions.FindGoodLocation((IGameObject)selSim, new World.FindGoodLocationParams(objPos), out simPos, out simFor))
            {
                SimpleMessageDialog.Show("Debug Data",string.Format("simPos: {0:F3}, {0:F3}, {0:F3}", simPos.x, simPos.y, simPos.z));
                if (hasSim == 1)
                {
                    InteractionInstance aport = Terrain.TeleportMeHere.Singleton.CreateInstance((IGameObject)Terrain.Singleton, 
                        simInst, new InteractionPriority(InteractionPriorityLevel.NonCriticalNPCBehavior), false, true);
                    (aport as Terrain.TeleportMeHere).Hit = InteractionInstance.CreateFakeGameObjectHit(simPos);
                    if ((aport != null) && simInst.InteractionQueue.AddNext(aport))
                        return true;
                }
                else
                {
                    simInst = selSim.Instantiate(simPos);
                    if (simInst != null)
                        return true;
                }
            }*/
            base.StandardExit();
            return true;
        }

        [DoesntRequireTuning]
        private class Definition : ImmediateInteractionDefinition<Sim, IViewable, Summon>
        {
            public Household famblyHouse;
            public string interactionName;

            public Definition()
            {
            }

            public Definition(Household fambly, string interactName)
            {
                this.famblyHouse = fambly;
                this.interactionName = interactName;
            }

            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return this.interactionName;
            }

            public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
            {
                NumSelectableRows = 1;
                base.PopulateSimPicker(ref parameters, out listObjs, out headers, this.famblyHouse.SimDescriptions, false);
            }

            protected override void AddInteractions(InteractionObjectPair iop, Sim actor, IViewable target, List<InteractionObjectPair> results)
            {
                foreach (Household household in Household.sHouseholdList)
                {
                    Summon.Definition interaction = new Summon.Definition(household, household.Name);
                    results.Add(new InteractionObjectPair(interaction, iop.Target));
                }
            }

            protected override bool Test(Sim a, IViewable target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }

            public override string[] GetPath()
            {
                return ChaosModPainting.BuildPath(ChaosInteractionCategory.Sim, "Summon...");
            }
        }
    }
}
