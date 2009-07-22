using System;
using System.Collections.Generic;
using System.Reflection;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Interactions;

namespace ChaosMod
{
    public class ChaosModPainting : Sims3.Gameplay.Objects.Decorations.Mimics.PaintingContemporary
    {
        public static List<GameObject> FloodedObjects = new List<GameObject>();

        public static string[] BuildPath(ChaosInteractionCategory category, params string[] extras)
        {
            string[] rez = new string[1 + extras.Length];
            rez[0] = "Chaos " + Enum.GetName(typeof(ChaosInteractionCategory), category) + "...";
            for (int i = 0; i < extras.Length; i++)
            {
                rez[1 + i] = extras[i];
            }
            return rez;
        }

        protected static List<InteractionDefinition> InteractionCache = null;
        protected static void BuildInteractionCache()
        {
            InteractionCache = new List<InteractionDefinition>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetCustomAttributes(typeof(HasChaosInteractionsAttribute), true).Length == 0)
                {
                    continue;
                }

                Type[] types = null;
                
                try
                {
                    types = assembly.GetTypes();
                }
                catch (Exception)
                {
                    continue;
                }

                foreach (Type type in types)
                {
                    if (typeof(IChaosInteractionProvider).IsAssignableFrom(type) == false)
                    {
                        continue;
                    }

                    IChaosInteractionProvider provider;

                    try
                    {
                        provider = (IChaosInteractionProvider)Activator.CreateInstance(type);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    InteractionCache.Add(provider.GetInteractionDefinition());
                }
            }
        }

        public override void OnStartup()
        {
            base.OnStartup();

            if (InteractionCache == null)
            {
                BuildInteractionCache();
            }

            foreach (InteractionDefinition id in InteractionCache)
            {
                this.AddInteraction(id);
            }
        }
    }
}
