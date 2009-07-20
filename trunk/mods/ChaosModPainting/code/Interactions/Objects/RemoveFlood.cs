using System.Collections.Generic;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.InteractionsShared;
using Sims3.SimIFace;
using Sims3.UI;

namespace ChaosMod.Interactions.Objects
{
    public class RemoveFlood : Interaction<Sim, IViewable>, IChaosInteractionProvider
    {
        public InteractionDefinition GetInteractionDefinition()
        {
            return Singleton;
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            GameObject objectToDry = base.GetSelectedObject() as GameObject;
            PuddleManager.RemoveFlowingPuddle(objectToDry);
            ChaosModPainting.FloodedObjects.Remove(objectToDry);
            return true;
        }

        private class Definition : InteractionDefinition<Sim, IViewable, RemoveFlood>
        {
            protected override string GetInteractionName(Sim a, IViewable target, InteractionObjectPair interaction)
            {
                return "Remove Flood";
            }

            protected override bool Test(Sim a, IViewable target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }

            public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
            {
                List<ObjectPicker.ColumnInfo> colInfo;
                NumSelectableRows = 1;
                headers = new List<ObjectPicker.HeaderInfo>();
                listObjs = new List<ObjectPicker.TabInfo>();
                headers.Add(new ObjectPicker.HeaderInfo("Objects", "Objects to Dry", 200));
                List<GameObject> objList = ChaosModPainting.FloodedObjects;
                List<ObjectPicker.RowInfo> rowInfo = new List<ObjectPicker.RowInfo>();
                ObjectPicker.RowInfo item = null;
                if ((objList == null) || (objList.Count <= 0))
                {
                    colInfo = new List<ObjectPicker.ColumnInfo>();
                    colInfo.Add(new ObjectPicker.ThumbAndTextColumn("", "No Flooded Objects in Painting's List"));
                    item = new ObjectPicker.RowInfo(null, colInfo);
                    rowInfo.Add(item);
                }
                Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
                foreach (GameObject objInst in objList)
                {
                    string key = objInst.CatalogName;
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary[key] = objInst;
                    }
                }
                foreach (KeyValuePair<string, GameObject> keyPair in dictionary)
                {
                    GameObject obj3 = keyPair.Value;
                    colInfo = new List<ObjectPicker.ColumnInfo>();
                    ThumbnailKey objTNK;
                    string objName;
                    if (obj3 is Sim)
                    {
                        objTNK = (obj3 as Sim).SimDescription.GetThumbnailKey(ThumbnailSize.Medium, 0);
                        objName = (obj3 as Sim).SimDescription.FullName;
                    }
                    else
                    {
                        objTNK = obj3.GetThumbnailKey();
                        objName = obj3.GetLocalizedName();
                    }
                    colInfo.Add(new ObjectPicker.ThumbAndTextColumn(objTNK, objName));
                    item = new ObjectPicker.RowInfo(obj3, colInfo);
                    rowInfo.Add(item);
                }
                ObjectPicker.TabInfo info2 = new ObjectPicker.TabInfo("Coupon", "Objects on Flood List", rowInfo);
                listObjs.Add(info2);
            }

            public override string[] GetPath()
            {
                return ChaosModPainting.BuildPath(ChaosInteractionCategory.Object);
            }
        }
    }
}
