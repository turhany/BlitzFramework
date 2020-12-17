using System;
using System.Linq;
using System.Reflection;
using BlitzFramework.Extensions;
using BlitzFramework.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace BlitzFramework.Data.Helpers
{
    public static class EFDataContextHelpers
    {
        public static void RegisterConfigurationFiles(ModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    t != typeof(IEntityTypeConfiguration<>) &&
                    t.GetInterfaces().Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToList();

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
        
        public static void SetAuditProperties(ChangeTracker changeTracker,object userId)
        {
            var entries = changeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.HasProperty(Literals.CreatedOnFieldName))
                    {
                        entry.Entity.SetPropertyValue(Literals.CreatedOnFieldName, DateTimeOffset.Now);
                    }

                    if (entry.Entity.HasProperty(Literals.CreatedByFieldName) && userId != null)
                    {
                        entry.Entity.SetPropertyValue(Literals.CreatedByFieldName, userId);
                    }
                    
                    if (entry.Entity.HasProperty(Literals.UpdatedOnFieldName))
                    {
                        entry.Entity.SetPropertyValue(Literals.UpdatedOnFieldName, DateTimeOffset.Now);
                    }

                    if (entry.Entity.HasProperty(Literals.UpdatedByFieldName) && userId != null)
                    {
                        entry.Entity.SetPropertyValue(Literals.UpdatedByFieldName, userId);
                    }
                }
                else if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    if (entry.Entity.HasProperty(Literals.UpdatedOnFieldName))
                    {
                        entry.Entity.SetPropertyValue(Literals.UpdatedOnFieldName, DateTimeOffset.Now);
                    }

                    if (entry.Entity.HasProperty(Literals.UpdatedByFieldName) && userId != null)
                    {
                        entry.Entity.SetPropertyValue(Literals.UpdatedByFieldName, userId);
                    }
                }
            }
        }

        public static void SetDeleteProperties(object entity)
        {
            if (entity.HasProperty(Literals.DeletedOnFieldName))
            {
                entity.SetPropertyValue(Literals.DeletedOnFieldName, DateTimeOffset.Now);
            }

            if (entity.HasProperty(Literals.IsDeletedFieldName))
            {
                entity.SetPropertyValue(Literals.IsDeletedFieldName, true);
            }
        }
    }
}
