using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Infozdrav.Web.Helpers
{
    public static class ReflectionHelpers
    {
        public static Assembly LoadAssembly(string assembly)
        {
            var asmPath = Assembly.GetEntryAssembly().Location;
            var dir = Path.GetDirectoryName(asmPath);
            return Assembly.LoadFrom($@"{dir}\{assembly}");
        }

        public static IEnumerable<Type> GetAllTypesWithBase<T>(this Assembly asm)
        {
            return GetAllTypesWithBaseFromAssembly<T>(asm);
        }

        public static IEnumerable<Type> GetAllTypesWithBaseFromAssembly<T>(Assembly asm)
        {
            var assemblies = asm.GetReferencedAssemblies().ToList();
            assemblies.Add(asm.GetName());

            bool SkipCondition(Type type) => !type.GetInterfaces().Contains(typeof(T)) || type.IsAbstract || type.IsGenericTypeDefinition;

            foreach (var assembly in assemblies)
            {
                var dependecy = Assembly.Load(assembly);
                foreach (var type in dependecy.GetTypes())
                {
                    if(SkipCondition(type))
                        continue;

                    yield return type;
                }
            }
        }
    }
}