using Unity.Entities;

namespace HookDOTS.API.HookRegistration;


internal struct HookHandle
{
    public int Value;
    public HookType HookType;
    public SystemTypeIndex SystemTypeIndex;
}
