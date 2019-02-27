using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Metadata;
using System.Reflection;

namespace DemoPostgres.Extensions
{
    public static class Extensions
    {
        public static Configuration ConfigureDatabase(this Configuration cfg)
        {
            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionProvider<DriverConnectionProvider>();
                x.ConnectionStringName = "Default";
                x.LogSqlInConsole = true;
                x.LogFormattedSql = true;
                x.Dialect<PostGis20Dialect>();
                x.Driver<NpgsqlDriver>();
            });
            cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));
            cfg.SetProperty("current_session_context_class", "web");
            cfg.SetProperty("show_sql", "true");
            cfg.SetProperty("format_sql", "true");
            cfg.AddAssembly(Assembly.GetExecutingAssembly());
            Metadata.AddMapping(cfg, MetadataClass.GeometryColumn);
            Metadata.AddMapping(cfg, MetadataClass.SpatialReferenceSystem);
            return cfg;
        }

        public static string Replace(this string str, string[] oldValues, string[] newValues)
        {
            for (int i = 0; i < oldValues.Length; i++)
            {
                str = str.Replace(oldValues[i], newValues[i]);
            }

            return str;
        }
    }
}