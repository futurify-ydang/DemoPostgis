using DemoPostgres.Extensions;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NHibernate;
using NHibernate.Cfg;
using Npgsql;
using System.Collections;
using System.IO;
using System.Linq;

namespace DemoPostgres
{
    public interface IAdminService
    {
        void InsertLayer(Layer layer, ISession session);

        void ConfigureTest(CSContext context, string connection);
    }

    public class AdminService : IAdminService
    {
        public void ConfigureTest(CSContext context, string connection)
        {
            // create database
            var dbExisted = context.Database.EnsureCreated();

            // create postgis extension
            context.Database.ExecuteSqlCommand("create extension if not exists postgis");

            // Reload type for npgsql
            using (NpgsqlConnection conn = new NpgsqlConnection(connection))
            {
                conn.Open();
                conn.ReloadTypes();
                conn.Close();
            }

            if (!dbExisted)
            {
                _configureNHibernate(connection);

                context.Database.ExecuteSqlCommand("INSERT INTO public.__connection(id, name, username, password, databasename, isdefault, state) VALUES (1, 'localhost', 'postgres', 'EAAAAI7FtR5YKUudVuY+WVtbLLjXOgGLSdufb0CksAQISC4L', 'daty2004', 't', 0);");
                context.Database.ExecuteSqlCommand("INSERT into __folder (name,state) VALUES('Test',0)");
                context.Database.ExecuteSqlCommand("INSERT INTO public.__service(id, connectionid, folderid, name, state, spatialreference, servicetype, iscached, iswmsenabled, isallowanonymous, isstopped, minscale, maxscale, maxrecordcount) VALUES (1, 1, 1, 'daty', 0, 102100, 0, 'f', 'f', 'f', 'f', 0, 0, 1000);");
            }
        }

        private void _configureNHibernate(string connectionString)
        {
            var cfg = new Configuration();
            cfg.ConfigureDatabase();
            cfg.SetProperty("hbm2ddl.auto", "update");
            cfg.SetProperty("adonet.batch_size", "1000");

            cfg.SetProperty("connection.connection_string", connectionString);

            var d = new DirectoryInfo("MetaDataConfigFiles");
            var files = d.GetFiles("*.hbm.xml");

            // get all file in folder
            foreach (var fileName in files.Select(f => f.FullName).ToList())
            {
                var config = File.ReadAllText(fileName).Replace("nvarchar(max)", "text");
                //replace ` for postgres server
                config = config.Replace(new string[] { "sql-type=\"bit\"", "datetime" },
                                        new string[] { "sql-type=\"boolean\"", " TIMESTAMP(3)" });

                if (fileName.Split('\\').Last().Equals("Setting.hbm.xml"))
                {
                    config = config.Replace("`", string.Empty);
                }

                cfg.AddXmlString(config);
            }

            var sessionFactory = cfg.BuildSessionFactory();
            var session = sessionFactory.OpenSession();

            //save SR to database, check if not existed
            var list = session.CreateCriteria("SR").List();
            if (list.Count < 1)
            {
                HashtableAdapter _hashtableAdapter = new HashtableAdapter();
                foreach (var sr in SpatialReferences.SRs)
                {
                    var hashtableSR = _hashtableAdapter.ObjectToHashtable(sr);
                    session.Save(nameof(SR), hashtableSR);
                }
                session.Flush();
            }
            session.Close();
        }

        public void InsertLayer(Layer layer, ISession session)
        {
            var hashtableLayer = _createHashtableMetaData(layer);
            layer.IsEditable = true;
            hashtableLayer[nameof(layer.ServiceId)] = Constants.ServiceId;
            hashtableLayer[nameof(layer.VisibleRange)] = 0;
            hashtableLayer[nameof(layer.IsEditable)] = layer.IsEditable;
            hashtableLayer[nameof(layer.HasAttachments)] = false;
            hashtableLayer[nameof(layer.IsSupportTime)] = false;
            hashtableLayer[nameof(layer.SignalRIsAddEventEnabled)] = false;
            hashtableLayer[nameof(layer.SignalRIsUpdateEventEnabled)] = false;
            hashtableLayer[nameof(layer.SignalRIsDeleteEventEnabled)] = false;
            hashtableLayer[nameof(layer.IsVersioned)] = false;
            hashtableLayer[nameof(layer.IsODataEnabled)] = layer.IsODataEnabled;
            hashtableLayer[nameof(layer.TableName)] = layer.TableName;
            hashtableLayer[nameof(layer.XmlMapping)] = layer.XmlMapping;

            // This cause exception
            var initPoint = new Point(0, 0);
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var multiPoint = geometryFactory.CreateMultiPoint(new IPoint[] { initPoint, initPoint });
            hashtableLayer[nameof(layer.Extent)] = multiPoint;

            // then add service
            session.Save(nameof(Layer), hashtableLayer);
            session.Flush();
        }

        private Hashtable _createHashtableMetaData(MetaDataBase metaDataBase)
        {
            return new Hashtable
            {
                {nameof(metaDataBase.Name), metaDataBase.Name},
                {nameof(metaDataBase.State), 0}
            };
        }
    }
}