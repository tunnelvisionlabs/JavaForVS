namespace Tvl.VisualStudio.Text.Navigation
{
    using System.Collections.Generic;

    public abstract class JavaEditorNavigationTypeDefinition
    {
        public JavaEditorNavigationTypeDefinition()
        {
            this.DisplayName = GetType().Name;
            this.TrackCursor = true;
            this.EnclosingTypes = new string[0];
        }

        public string DisplayName
        {
            get;
            protected set;
        }

        public bool TrackCursor
        {
            get;
            protected set;
        }

        public IEnumerable<string> EnclosingTypes
        {
            get;
            protected set;
        }
    }
}
