using System;

namespace Common.Infs
{
    public static class Mapper
    {
        public static TTarget Map<TTarget, TSource>(TSource source)
        {
            var target = Activator.CreateInstance<TTarget>();
            var properties = source.GetType().GetProperties();
            foreach (var property in properties)
            {
               var targetProperty =  target.GetType().GetProperty(property.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                targetProperty.SetMethod.Invoke(target, new object[] { property.GetValue(source) });
            }
            return target;
        }
    }
}
