using Cars.WebAPI.Help;
using System;
using System.Collections.Generic;

namespace Cars.WebAPI.Extensions
{
    internal static class EnumExtension
    {
        internal static IList<EnumeratorModel> EnumToDictonary<T>() where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"{typeof(T)} is not an Enum type");
            var enumerators = new List<EnumeratorModel>();
            var enums = Enum.GetValues(typeof(T));
            foreach (var enumItem in enums)
            {
                enumerators.Add(new EnumeratorModel()
                {
                    Id = (int)enumItem,
                    Name = enumItem.ToString()
                });
            }
            return enumerators;
        }
    }
}
