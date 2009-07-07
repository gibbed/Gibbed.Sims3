using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public static partial class ResourceKeyHelper
    {
        public static ResourceKey ReadResourceKeyTGI(this Stream stream)
        {
            ResourceKey key = new ResourceKey();
            key.TypeId = stream.ReadValueU32();
            key.GroupId = stream.ReadValueU32();
            key.InstanceId = stream.ReadValueU64();
            return key;
        }

        public static void WriteResourceKeyTGI(this Stream stream, ResourceKey key)
        {
            stream.WriteValueU32(key.TypeId);
            stream.WriteValueU32(key.GroupId);
            stream.WriteValueU64(key.InstanceId);
        }

        public static ResourceKey ReadResourceKeyIGT(this Stream stream)
        {
            ResourceKey key = new ResourceKey();
            key.InstanceId = stream.ReadValueU64();
            key.GroupId = stream.ReadValueU32();
            key.TypeId = stream.ReadValueU32();
            return key;
        }

        public static void WriteResourceKeyIGT(this Stream stream, ResourceKey key)
        {
            stream.WriteValueU64(key.InstanceId);
            stream.WriteValueU32(key.GroupId);
            stream.WriteValueU32(key.TypeId);
        }
    }

    public struct ResourceKey
    {
        public UInt32 TypeId;
        public UInt32 GroupId;
        public UInt64 InstanceId;

        public ResourceKey(UInt64 instanceId, UInt32 typeId, UInt32 groupId)
        {
            this.InstanceId = instanceId;
            this.TypeId = typeId;
            this.GroupId = groupId;
        }

        public override string ToString()
        {
            return String.Format("{0:X8}:{1:X8}:{2:X16}", this.TypeId, this.GroupId, this.InstanceId);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            return (ResourceKey)obj == this;
        }

        public static bool operator !=(ResourceKey a, ResourceKey b)
        {
            return a.TypeId != b.TypeId || a.GroupId != b.GroupId || a.InstanceId != b.InstanceId;
        }

        public static bool operator ==(ResourceKey a, ResourceKey b)
        {
            return a.TypeId == b.TypeId && a.GroupId == b.GroupId && a.InstanceId == b.InstanceId;
        }

        public override int GetHashCode()
        {
            return this.InstanceId.GetHashCode() ^ ((int)(this.TypeId ^ (this.GroupId << 16)));
        }
    }
}
