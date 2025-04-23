using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application;

public class AssemblyScanResult
{
    public readonly Type Interface;
    public readonly Assembly Assembly;
    public readonly Type Implementation;

    public AssemblyScanResult(Assembly assembly, Type @interface, Type implementation)
    {
        Interface = @interface;
        Assembly = assembly;
        Implementation = implementation;
    }
}
public static class Helpers
{
    public static AssemblyScanResult[] ScanHandlersFromAssembly()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var handlerTypeTemplate = typeof(IRequestHandler<,>);
        var fireAndForgetHandlerTemplate = typeof(IRequestHandler<>);

        var implementations = assembly
            .GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type =>
                type.GetInterfaces()
                    .Where(i => i.IsGenericType && 
                        (i.GetGenericTypeDefinition() == handlerTypeTemplate 
                        || i.GetGenericTypeDefinition() == fireAndForgetHandlerTemplate))
                    .Select(i => new AssemblyScanResult(assembly, i, type))
            );

        return implementations.ToArray();
    }
}
