using Unity.Entities;

namespace HookDOTS.HookRegistration;


internal struct HookHandle
{
    public int Value;
    public HookType HookType;

    // this isn't relevant for everything. currently using SystemTypeIndex.Null for those cases
    // but might refactor
    public SystemTypeIndex SystemTypeIndex; 
}
