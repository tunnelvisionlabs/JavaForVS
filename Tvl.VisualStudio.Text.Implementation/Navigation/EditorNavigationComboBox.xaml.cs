namespace Tvl.VisualStudio.Text.Navigation.Implementation
{
    using System.Windows.Controls;

    public partial class EditorNavigationComboBox : ComboBox
    {
#if false
        static EditorNavigationComboBox()
        {
            if (Application.Current != null)
            {
                ResourceDictionary item = LoadResourceValue<ResourceDictionary>("themes/generic.xaml");
                if (item != null)
                {
                    Application.Current.Resources.MergedDictionaries.Add(item);
                }
            }

            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EditorNavigationComboBox), new FrameworkPropertyMetadata(typeof(EditorNavigationComboBox)));
        }

        internal static T LoadResourceValue<T>(string xamlName)
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name;
            Uri resourceLocator = new Uri("/" + name + ";component/" + xamlName, UriKind.Relative);
            return (T)Application.LoadComponent(resourceLocator);
        }
#endif

        public EditorNavigationComboBox()
        {
            InitializeComponent();
        }
    }
}
