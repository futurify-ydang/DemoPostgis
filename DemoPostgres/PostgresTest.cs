using DemoPostgres.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using System.IO;
using System.Linq;

namespace DemoPostgres
{
    [TestClass]
    public class PostgresTest
    {
        private ServiceProvider _serviceProvider;
        private ISession _session;
        private CSContext _context;
        private IAdminService _adminService;
        private const string _connectionString = "Server=localhost;Database=daty103;User Id=postgres;Password=admin123;";

        public PostgresTest()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IAdminService, AdminService>();
            services.AddDbContext<CSContext>(builder =>
            {
                builder.UseNpgsql(_connectionString, option =>
                {
                    option.UseNetTopologySuite();
                });
            });
            _addNHibernateSession(services);
            _serviceProvider = services.BuildServiceProvider();
            _adminService = _serviceProvider.GetRequiredService<IAdminService>();
            _context = _serviceProvider.GetRequiredService<CSContext>();
            _session = _serviceProvider.GetRequiredService<ISession>();
        }

        private void _addNHibernateSession(ServiceCollection services)
        {
            services.AddSingleton<ISessionFactory>((provider) =>
            {
                var cfg = new Configuration();

                cfg.ConfigureDatabase();

                cfg.SetProperty("adonet.batch_size", "1000");

                cfg.SetProperty(Constants.ConnectionStringKeyName, _connectionString);

                DirectoryInfo d = new DirectoryInfo(Constants.Folder_MetaDataConfigFiles);

                var files = d.GetFiles("*.hbm.xml");
                // get all file in folder

                foreach (var fileName in files.Select(f => f.FullName).ToList())
                {
                    cfg.AddXmlFile(fileName);
                }

                return cfg.BuildSessionFactory();
            });

            services.AddScoped<ISession>((provider) =>
            {
                var factory = provider.GetService<ISessionFactory>();
                return factory.OpenSession();
            });
        }

        [TestMethod]
        public void CreateDatabase()
        {
            _adminService.ConfigureTest(_context, _connectionString);
        }

        [TestMethod]
        public void InsertLayer()
        {
            var layer = new Layer {
                ServiceId = 1,
                Name = "Banks",
                TableName = "Banks",
                State = 0,
                VisibleRange = 0,
                IsEditable = true,
                IsVersioned = false,
                SignalRIsAddEventEnabled = false,
                SignalRIsDeleteEventEnabled = false,
                SignalRIsUpdateEventEnabled = false,
                HasAttachments = false,
                IsSupportTime = false,
            };
            _adminService.InsertLayer(layer,_session);
        }

        [TestMethod]
        public void InsertLayerByCommand()
        {
            _context.Database.ExecuteSqlCommand("INSERT INTO public.__layer(id, serviceid, name, odkform, tablename, filterexpression, state, spatialreference, type, drawing, visiblerange, iseditable, isversioned, signalrisaddeventenabled, signalrisupdateeventenabled, signalrisdeleteeventenabled, pubnubisaddeventenabled, pubnubisupdateeventenabled, pubnubisdeleteeventenabled, azureisaddeventenabled, azuresupdateeventenabled, azureisdeleteeventenabled, isodataenabled, isodkenabled, hasattachments, issupporttime, timefieldid, mindate, maxdate, extent, popuptype, popupcontent, minscale, maxscale, relationshipsjson) VALUES (3, 1, 'Banks1', NULL, 'banks_1', NULL, 0, NULL, NULL, NULL, 0, 't', 'f', 'f', 'f', 'f', NULL, NULL, NULL, NULL, NULL, NULL, 'f', NULL, 'f', 'f', NULL, NULL, NULL, 'MULTIPOINT((0 0), (0 0))', NULL, NULL, NULL, NULL, NULL);");
        }
    }
}