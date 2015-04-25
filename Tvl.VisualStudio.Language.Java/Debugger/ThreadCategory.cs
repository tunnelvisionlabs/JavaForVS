namespace Tvl.VisualStudio.Language.Java.Debugger
{
    public enum ThreadCategory
    {
        Worker = 0,
        UserInterface = 1,
        Main = 2,
        RemoteProcedureCall = 3,
        Unknown = 4,
    }
}
