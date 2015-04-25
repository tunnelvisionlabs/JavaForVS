namespace Tvl.Java.DebugInterface.Types
{
    public interface IConstantMemberReference
    {
        ushort ClassIndex
        {
            get;
        }

        ushort NameAndTypeIndex
        {
            get;
        }
    }
}
