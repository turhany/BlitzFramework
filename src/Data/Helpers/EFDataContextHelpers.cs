using System;
using System.Linq;
using System.Reflection;
using BlitzFramework.Constants;
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
                    if (entry.Entity.HasProperty(FrameworkConstants.CreatedOnFieldName))
                    {
                        entry.Entity.SetPropertyValue(FrameworkConstants.CreatedOnFieldName, DateTimeOffset.Now);
                    }

                    if (entry.Entity.HasProperty(FrameworkConstants.CreatedByFieldName) && userId != null)
                    {
                        entry.Entity.SetPropertyValue(FrameworkConstants.CreatedByFieldName, userId);
                    }
                    
                    if (entry.Entity.HasProperty(FrameworkConstants.UpdatedOnFieldName))
                    {
                        entry.Entity.SetPropertyValue(FrameworkConstants.UpdatedOnFieldName, DateTimeOffset.Now);
                    }

                    if (entry.Entity.HasProperty(FrameworkConstants.UpdatedByFieldName) && userId != null)
                    {
                        entry.Entity.SetPropertyValue(FrameworkConstants.UpdatedByFieldName, userId);
                    }
                }
                else if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    if (entry.Entity.HasProperty(FrameworkConstants.UpdatedOnFieldName))
                    {
                        entry.Entity.SetPropertyValue(FrameworkConstants.UpdatedOnFieldName, DateTimeOffset.Now);
                    }

                    if (entry.Entity.HasProperty(FrameworkConstants.UpdatedByFieldName) && userId != null)
                    {
                        entry.Entity.SetPropertyValue(FrameworkConstants.UpdatedByFieldName, userId);
                    }
                }
            }
        }

        public static void SetDeleteProperties(object entity)
        {
            if (entity.HasProperty(FrameworkConstants.DeletedOnFieldName))
            {
                entity.SetPropertyValue(FrameworkConstants.DeletedOnFieldName, DateTimeOffset.Now);
            }

            if (entity.HasProperty(FrameworkConstants.IsDeletedFieldName))
            {
                entity.SetPropertyValue(FrameworkConstants.IsDeletedFieldName, true);
            }
        }
    }
}
