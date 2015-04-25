namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System.ComponentModel.DataAnnotations;

    public enum DebugAgent
    {
        [Display(Name = "High-performance debug agent (default)")]
        CustomJvmti,

        [Display(Name = "JDWP debugging (compatibility mode)")]
        Jdwp,
    }
}
