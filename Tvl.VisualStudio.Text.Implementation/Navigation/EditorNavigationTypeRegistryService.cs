namespace Tvl.VisualStudio.Text.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(IJavaEditorNavigationTypeRegistryService))]
    public sealed class EditorNavigationTypeRegistryService : IJavaEditorNavigationTypeRegistryService
    {
        private IEnumerable<Lazy<JavaEditorNavigationTypeDefinition, IEditorNavigationTypeDefinitionMetadata>> _editorNavigationTypeDefinitions;

        private readonly Dictionary<string, EditorNavigationType> _navigationTypes =
            new Dictionary<string, EditorNavigationType>();

        [ImportMany]
        private IEnumerable<Lazy<JavaEditorNavigationTypeDefinition, IEditorNavigationTypeDefinitionMetadata>> EditorNavigationTypeDefinitions
        {
            get
            {
                return _editorNavigationTypeDefinitions;
            }
            set
            {
                if (_editorNavigationTypeDefinitions == value)
                    return;

                _editorNavigationTypeDefinitions = value;
                var navigationTypes = _editorNavigationTypeDefinitions.ToList();
                navigationTypes.RemoveAll(navigationType => _navigationTypes.ContainsKey(navigationType.Metadata.Name));
                for (int i = navigationTypes.Count; i > 0; i--)
                {
                    int currentIndex = navigationTypes.FindIndex(navigationType => navigationType.Metadata.BaseDefinition == null || navigationType.Metadata.BaseDefinition.All(_navigationTypes.ContainsKey));
                    if (currentIndex < 0)
                        throw new InvalidOperationException("Circular editor navigation type definition.");

                    var current = navigationTypes[currentIndex];
                    string currentName = current.Metadata.Name;
                    IEnumerable<string> currentBaseDefinitions = current.Metadata.BaseDefinition ?? new string[0];
                    this._navigationTypes.Add(current.Metadata.Name, new EditorNavigationType(current.Value, currentName, currentBaseDefinitions.Select(GetEditorNavigationType)));
                    navigationTypes.RemoveAt(currentIndex);
                }
            }
        }

        public IEditorNavigationType CreateEditorNavigationType(JavaEditorNavigationTypeDefinition definition, string type, IEnumerable<IEditorNavigationType> baseTypes)
        {
            var navigationType = new EditorNavigationType(definition, type, baseTypes);
            _navigationTypes.Add(type, navigationType);
            return navigationType;
        }

        public IEditorNavigationType CreateTransientEditorNavigationType(IEnumerable<IEditorNavigationType> baseTypes)
        {
            throw new NotImplementedException();
        }

        public IEditorNavigationType CreateTransientEditorNavigationType(params IEditorNavigationType[] baseTypes)
        {
            throw new NotImplementedException();
        }

        public IEditorNavigationType GetEditorNavigationType(string type)
        {
            EditorNavigationType navigationType;
            if (!this._navigationTypes.TryGetValue(type, out navigationType))
                return null;

            return navigationType;
        }

        [Export(typeof(JavaEditorNavigationTypeDefinition))]
        [Name(PredefinedEditorNavigationTypes.Types)]
        [Order(Before = PredefinedEditorNavigationTypes.Members)]
        internal sealed class TypesStandardEditorNavigationTypeDefinition : JavaEditorNavigationTypeDefinition
        {
            public TypesStandardEditorNavigationTypeDefinition()
            {
                this.DisplayName = "Types";
            }
        }

        [Export(typeof(JavaEditorNavigationTypeDefinition))]
        [Name(PredefinedEditorNavigationTypes.Members)]
        internal sealed class MembersStandardEditorNavigationTypeDefinition : JavaEditorNavigationTypeDefinition
        {
            public MembersStandardEditorNavigationTypeDefinition()
            {
                this.DisplayName = "Members";
                this.EnclosingTypes = new string[] { PredefinedEditorNavigationTypes.Types };
            }
        }
    }
}
