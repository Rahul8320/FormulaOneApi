using FormulaOne.DataService.Data;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.Api;

public static class Extensions
{
    public static IApplicationBuilder ApplyPendingMigrations(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            var context = serviceScope?.ServiceProvider.GetRequiredService<AppDbContext>();
            context?.Database.Migrate();
        }

        return app;
    }
}
