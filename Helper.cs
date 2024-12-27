using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AIQueryGeneratorDemo
{
    public static class Helper
    {
        public static string GenerateSchema(DbContext context)
        {
            var model = context.Model;
            var sb = new StringBuilder();

            foreach (var entityType in model.GetEntityTypes())
            {
                sb.AppendLine($"Table: {entityType.GetTableName()}");
                sb.AppendLine($"Schema: {entityType.GetSchema() ?? "default"}");

                foreach (var property in entityType.GetProperties())
                {
                    sb.AppendLine($"Column: {property.Name}");
                    sb.AppendLine($"Type: {property.GetColumnType()}");
                    sb.AppendLine($"Nullable: {property.IsNullable}");
                }

                foreach (var key in entityType.GetKeys())
                {
                    sb.AppendLine($"Primary Key: {string.Join(", ", key.Properties.Select(p => p.Name))}");
                }

                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    sb.AppendLine($"Foreign Key:");
                    sb.AppendLine($"Principal Table: {foreignKey.PrincipalEntityType.GetTableName()}");
                    sb.AppendLine($"Columns: {string.Join(", ", foreignKey.Properties.Select(p => p.Name))}");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
