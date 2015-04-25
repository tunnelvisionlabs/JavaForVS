namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.Shell;

    public abstract class DebuggerRegistrationAttribute : RegistrationAttribute
    {
        protected DebuggerRegistrationAttribute(MetricTypes type, Guid section)
        {
            MetricType = type;
            MetricSection = section;
        }

        protected static string GetMetricTypeString(MetricTypes type)
        {
            switch (type)
            {
            case MetricTypes.DebugEngine:
                return "Engine";

            case MetricTypes.PortSupplier:
                return "PortSupplier";

            case MetricTypes.Exception:
                return "Exception";

            case MetricTypes.ExpressionEvaluator:
                return "ExpressionEvaluator";

            case MetricTypes.ExpressionEvaluatorExtension:
                return "EEExtension";

            case MetricTypes.SymbolProvider:
                return "SymbolProvider";

            default:
                throw new ArgumentException();
            }
        }

        protected MetricTypes MetricType
        {
            get;
            set;
        }

        protected Guid MetricSection
        {
            get;
            set;
        }

        protected virtual string RegistrationKey
        {
            get
            {
                return string.Format(@"AD7Metrics\{0}\{1}", GetMetricTypeString(MetricType), MetricSection.ToString("B"));
            }
        }

        public override void Register(RegistrationContext context)
        {
            using (Key key = context.CreateKey(RegistrationKey))
            {
                foreach (PropertyInfo property in GetType().GetProperties())
                {
                    if (property == null)
                        continue;

                    DebugMetricAttribute metric = property.GetCustomAttributes(typeof(DebugMetricAttribute), true).FirstOrDefault() as DebugMetricAttribute;
                    MethodInfo accessor = property.GetGetMethod();
                    if (metric != null && accessor != null)
                    {
                        if (typeof(string).IsAssignableFrom(property.PropertyType))
                        {
                            string value = accessor.Invoke(this, null) as string;
                            if (value != null)
                                key.SetValue(metric.Name, value);
                        }
                        else if (typeof(Guid[]).IsAssignableFrom(property.PropertyType))
                        {
                            Guid[] value = accessor.Invoke(this, null) as Guid[];
                            if (value != null && value.Length > 0)
                            {
                                if (value.Length == 1 && metric.Name == "PortSupplier")
                                {
                                    key.SetValue(metric.Name, value[0].ToString("B"));
                                }
                                else
                                {
                                    using (Key subkey = key.CreateSubkey(metric.Name))
                                    {
                                        for (int i = 0; i < value.Length; i++)
                                        {
                                            subkey.SetValue(i.ToString(), value[i].ToString("B"));
                                        }
                                    }
                                }
                            }
                        }
                        else if (typeof(Guid).IsAssignableFrom(property.PropertyType))
                        {
                            Guid value = (Guid)accessor.Invoke(this, null);
                            if (value != Guid.Empty)
                                key.SetValue(metric.Name, value.ToString("B"));
                        }
                        else if (typeof(Guid?).IsAssignableFrom(property.PropertyType))
                        {
                            Guid? value = accessor.Invoke(this, null) as Guid?;
                            if (value != null)
                                key.SetValue(metric.Name, ((Guid)value).ToString("B"));
                        }
                        else if (typeof(int).IsAssignableFrom(property.PropertyType))
                        {
                            int value = (int)accessor.Invoke(this, null);
                            if (value != 0)
                                key.SetValue(metric.Name, value);
                        }
                        else if (typeof(int?).IsAssignableFrom(property.PropertyType))
                        {
                            int? value = accessor.Invoke(this, null) as int?;
                            if (value != null)
                                key.SetValue(metric.Name, (int)value);
                        }
                        else if (typeof(uint).IsAssignableFrom(property.PropertyType))
                        {
                            uint value = (uint)accessor.Invoke(this, null);
                            if (value != 0)
                                key.SetValue(metric.Name, value);
                        }
                        else if (typeof(uint?).IsAssignableFrom(property.PropertyType))
                        {
                            uint? value = accessor.Invoke(this, null) as uint?;
                            if (value != null)
                                key.SetValue(metric.Name, (uint)value);
                        }
                        else if (typeof(bool).IsAssignableFrom(property.PropertyType))
                        {
                            bool value = (bool)accessor.Invoke(this, null);
                            if (value != false)
                                key.SetValue(metric.Name, value ? 1 : 0);
                        }
                        else if (typeof(bool?).IsAssignableFrom(property.PropertyType))
                        {
                            bool? value = accessor.Invoke(this, null) as bool?;
                            if (value != null)
                                key.SetValue(metric.Name, (bool)value ? 1 : 0);
                        }
                        else
                        {
                            throw new NotSupportedException("MetricAttribute does not support properties of this type.");
                        }
                    }
                }
            }
        }

        public override void Unregister(RegistrationContext context)
        {
            context.RemoveKey(RegistrationKey);
        }
    }
}
