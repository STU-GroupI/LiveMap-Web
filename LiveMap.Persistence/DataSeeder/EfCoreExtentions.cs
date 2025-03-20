using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Persistence.DataSeeder;
public static class EfCoreExtentions
{
    public static void SeedContext(this LiveMapContext context)
    {
        DevelopmentSeeder.SeedDatabase(context);
    }
}