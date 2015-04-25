namespace Tvl.VisualStudio.Language.Java.Project
{
    using Tvl.VisualStudio.Language.Java.Project.PropertyPages;

    public sealed class JavaBuildOptions
    {
        private JavaGeneralPropertyPagePanel _general;
        private JavaBuildPropertyPagePanel _build;
        private JavaDebugPropertyPagePanel _debug;

        public JavaGeneralPropertyPagePanel General
        {
            get
            {
                return _general;
            }

            set
            {
                _general = value;
                if (_general != null)
                    _general.Disposed += (sender, e) => _general = null;
            }
        }

        public JavaBuildPropertyPagePanel Build
        {
            get
            {
                return _build;
            }

            set
            {
                _build = value;
                if (_build != null)
                    _build.Disposed += (sender, e) => _build = null;
            }
        }

        public JavaDebugPropertyPagePanel Debug
        {
            get
            {
                return _debug;
            }

            set
            {
                _debug = value;
                if (_debug != null)
                    _debug.Disposed += (sender, e) => _debug = null;
            }
        }
    }
}
