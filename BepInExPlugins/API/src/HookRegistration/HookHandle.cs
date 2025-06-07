using Unity.Entities;

namespace HookDOTS.HookRegistration;


internal struct HookHandle
{
    public int Value;
    public HookType HookType;
    public SystemTypeIndex SystemTypeIndex;
}
