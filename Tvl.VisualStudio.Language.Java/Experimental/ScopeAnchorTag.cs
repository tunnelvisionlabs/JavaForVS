namespace Tvl.VisualStudio.Language.Java.Experimental
{
    using Microsoft.VisualStudio.Text.Tagging;

    public class ScopeAnchorTag : ITag
    {
        private readonly string _name;
        private readonly int _id;
        private readonly int _type;
        private readonly int _order;

        public ScopeAnchorTag(string name, int id, int type, int order)
        {
            _name = name;
            _id = id;
            _type = type;
            _order = order;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public int Type
        {
            get
            {
                return _type;
            }
        }

        public int Order
        {
            get
            {
                return _order;
            }
        }
    }
}
