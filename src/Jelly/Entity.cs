using Jelly.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jelly
{
    public abstract class Entity<T> : BaseEntity where T : Entity<T>
    {
        private Dictionary<PropertyInfo, MethodInfo> ValidateTriggers;
        protected T xRec;

        public static event EventHandler<EventArgs> OnAfterInsert;
        public static event EventHandler<EventArgs> OnAfterModify;
        public static event EventHandler<EventArgs> OnAfterDelete;
        public static event EventHandler<EventArgs> OnBeforeInsert;
        public static event EventHandler<EventArgs> OnBeforeModify;
        public static event EventHandler<EventArgs> OnBeforeDelete;
        public static event EventHandler<EventArgs> OnGlobalInsert;
        public static event EventHandler<EventArgs> OnGlobalModify;
        public static event EventHandler<EventArgs> OnGlobalDelete;

        public Entity()
        {
            Initialize();
        }

        protected virtual void OnInsert()
        { }

        protected virtual void OnModify()
        { }

        protected virtual void OnDelete()
        { }

        public void Validate(Expression<Func<T, object>> outExpr, object value)
        {
            var propertyInfo = GetExpressionProperty(outExpr);
            object oldValue = propertyInfo.GetValue(this);
            if (value is not null)
            {
                if (value.Equals(oldValue))
                    return;
            }
            else if (value == oldValue)
                return;

            propertyInfo.SetValue(this, value);
            if (ValidateTriggers.ContainsKey(propertyInfo))
            {
                MethodInfo method = ValidateTriggers.GetValueOrDefault(propertyInfo);
                method.Invoke(this, []);
            }
        }

        private void Initialize()
        {
            InitializeFields();
            InitializeValidateEvents();
        }

        private void InitializeFields()
        {
            List<PropertyInfo> propertyInfos = GetType().GetProperties().ToList();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.Equals(typeof(string)))
                    propertyInfo.SetValue(this, string.Empty);
                else if (propertyInfo.PropertyType.Equals(typeof(int)))
                    propertyInfo.SetValue(this, 0);
                else if (propertyInfo.PropertyType.Equals(typeof(decimal)))
                    propertyInfo.SetValue(this, 0);
                else if (propertyInfo.PropertyType.Equals(typeof(float)))
                    propertyInfo.SetValue(this, 0);
                else if (propertyInfo.PropertyType.Equals(typeof(double)))
                    propertyInfo.SetValue(this, 0);
                else if (propertyInfo.PropertyType.Equals(typeof(bool)))
                    propertyInfo.SetValue(this, false);
                else if (propertyInfo.PropertyType.Equals(typeof(DateTime)))
                    propertyInfo.SetValue(this, DateTime.MinValue);
                else if (propertyInfo.PropertyType.Equals(typeof(TimeSpan)))
                    propertyInfo.SetValue(this, TimeSpan.MinValue);
                else if (propertyInfo.PropertyType.Equals(typeof(byte[])))
                    propertyInfo.SetValue(this, new byte[0]);
                else if (propertyInfo.PropertyType.Equals(typeof(Guid)))
                    propertyInfo.SetValue(this, Guid.Empty);
            }
        }

        private void InitializeValidateEvents()
        {
            Type currentType = GetType();
            List<MethodInfo> methods = currentType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(e => e.GetCustomAttributes().OfType<ValidateEventAttribute>().Any()).ToList();
            Dictionary<PropertyInfo, MethodInfo> map = new Dictionary<PropertyInfo, MethodInfo>();

            foreach (MethodInfo method in methods)
            {
                ValidateEventAttribute triggerAttribute = (ValidateEventAttribute)method.GetCustomAttribute(typeof(ValidateEventAttribute));

                PropertyInfo info = currentType.GetProperty(triggerAttribute.Field);
                if (info is not null)
                {
                    map.Add(info, method);
                    break;
                }
            }

            ValidateTriggers = map;
        }

        private PropertyInfo GetExpressionProperty(Expression<Func<T, object>> outExpr)
        {
            string propertyName = "";
            if (outExpr.Body is MemberExpression)
            {
                propertyName = ((MemberExpression)outExpr.Body).Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)outExpr.Body).Operand;
                propertyName = ((MemberExpression)op).Member.Name;
            }
            return GetType().GetProperty(propertyName);
        }
    }
}
