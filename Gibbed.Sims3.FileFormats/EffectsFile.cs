using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class EffectsFile : IFormat
    {
        public UInt16 Version;

        #region Effects
        private static Type[] EffectTypes =
        {
            null,
            typeof(Effects.Particles),
            typeof(Effects.MetaParticles),
            typeof(Effects.Decal),
            typeof(Effects.Sequence),
            typeof(Effects.Sound),
            typeof(Effects.Shake),
            typeof(Effects.Camera),
            typeof(Effects.Model),
            typeof(Effects.Screen),
            null,
            typeof(Effects.Game),
            typeof(Effects.FastParticles),
            typeof(Effects.Distribute),
            typeof(Effects.Ribbon),
        };

        public class EffectArray
        {
            public short Type;
            public short Version;
            public List<IEffectFormat> Entries;
        }
        #endregion
        #region Resources
        private static Type[] ResourceTypes =
        {
            typeof(Effects.Maps),
            typeof(Effects.Material),
        };

        public class ResourceArray
        {
            public short Type;
            public short Version;
            public List<IEffectFormat> Entries;
        }
        #endregion
        #region VisualEffect
        public struct VisualEffectArray
        {
            public short Version;
            public List<IEffectFormat> Entries;
        }
        #endregion

        public List<EffectArray> Effects;
        public List<ResourceArray> Resources;
        public Dictionary<int, string> EffectNames;
        public VisualEffectArray VisualEffects;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            this.Version = input.ReadU16(false);

            if (this.Version != 2)
            {
                throw new InvalidDataException("not version 2");
            }

            // Effects
            {
                this.Effects = new List<EffectArray>();
                short type = input.ReadS16(false);
                while (type != -1)
                {
                    short version = input.ReadS16(false);

                    if (type < 0 || type >= EffectTypes.Length || EffectTypes[type] == null)
                    {
                        throw new InvalidOperationException("invalid type " + type.ToString());
                    }

                    if (EffectTypes[type].GetInterface("IEffectFormat") == null)
                    {
                        throw new InvalidOperationException(EffectTypes[type].ToString() + " does not derive IEffectFormat");
                    }

                    EffectArray effects = new EffectArray();
                    effects.Type = type;
                    effects.Version = version;
                    effects.Entries = new List<IEffectFormat>();

                    int count = input.ReadS32(false);
                    for (int i = 0; i < count; i++)
                    {
                        IEffectFormat instance = (IEffectFormat)Activator.CreateInstance(EffectTypes[type]);

                        if (version < instance.MinimumVersion || version > instance.MaximumVersion)
                        {
                            throw new InvalidOperationException("bad version " + version.ToString() + " for type " + instance.ToString());
                        }

                        instance.Deserialize(input, effects.Version);
                        effects.Entries.Add(instance);
                    }

                    this.Effects.Add(effects);

                    type = input.ReadS16(false);
                }
            }

            // Resources
            {
                this.Resources = new List<ResourceArray>();
                short type = input.ReadS16(false);
                while (type != -1)
                {
                    short version = input.ReadS16(false);

                    if (type < 0 || type >= ResourceTypes.Length || ResourceTypes[type] == null)
                    {
                        throw new InvalidOperationException("invalid resource " + type.ToString());
                    }

                    if (ResourceTypes[type].GetInterface("IEffectFormat") == null)
                    {
                        throw new InvalidOperationException(ResourceTypes[type].ToString() + " does not derive IEffectFormat");
                    }

                    ResourceArray resources = new ResourceArray();
                    resources.Type = type;
                    resources.Version = version;
                    resources.Entries = new List<IEffectFormat>();

                    int count = input.ReadS32(false);
                    for (int i = 0; i < count; i++)
                    {
                        IEffectFormat instance = (IEffectFormat)Activator.CreateInstance(ResourceTypes[type]);

                        if (version < instance.MinimumVersion || version > instance.MaximumVersion)
                        {
                            throw new InvalidOperationException("bad version " + version.ToString() + " for resource " + instance.ToString());
                        }

                        instance.Deserialize(input, resources.Version);
                        resources.Entries.Add(instance);
                    }

                    this.Resources.Add(resources);

                    type = input.ReadS16(false);
                }
            }

            // Visual effects
            {
                short version = input.ReadS16(false);

                this.VisualEffects = new VisualEffectArray();
                this.VisualEffects.Version = version;
                this.VisualEffects.Entries = new List<IEffectFormat>();

                int count = input.ReadS32(false);
                for (int i = 0; i < count; i++)
                {
                    Effects.VisualEffect visualEffect = new Effects.VisualEffect();
                    visualEffect.Deserialize(input, version);
                    this.VisualEffects.Entries.Add(visualEffect);
                }
            }

            // ???
            {
                int unk = input.ReadS32(false);
                while (unk != -1)
                {
                    throw new NotImplementedException();
                    //unk = input.ReadS32(false);
                }
            }

            this.EffectNames = new Dictionary<int, string>();

            int id = input.ReadS32(false);
            while (id != -1)
            {
                this.EffectNames.Add(id, input.ReadASCIIZ());
                id = input.ReadS32(false);
            }

            throw new NotImplementedException();
        }
    }
}
