using System;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CLib;
using EStationCore.Model;
using EStationCore.Model.Common.Entity;
using EStationCore.Model.Common.Enums;
using EStationCore.Model.Hr.Entity;
using EStationCore.Properties;

namespace EStationCore.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<StationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(StationContext context)
        {
            //SeedFromSql(context);
        }

        private static void SeedFromSql(StationContext ef)
        {
            ef.Database.CreateIfNotExists();

            //ProfileSeed(context);
            //Staff_SeedFromSql(context);
            //Student_SeedFromSql(context);

            var sqlPeople = Resources.dbo_People_data;
            var sqlStaffs = Resources.dbo_Staffs_data;
            
            ef.Database.ExecuteSqlCommand("DELETE FROM Staffs");
            ef.Database.ExecuteSqlCommand("DELETE FROM People");


            ef.Database.ExecuteSqlCommand(sqlPeople);
            ef.Database.ExecuteSqlCommand(sqlStaffs);
                    

            var admin = new Staff
            {
                StaffGuid = new Guid("53f258a3-f931-4975-b6ec-17d26aa95848"),
                Matricule = "SS-124-3652",
                PositionPrincipale = "Chef de Departement Info",
                DepartementPrincipale = "Informatique",
                HiredDate = DateTime.Today.AddDays(-500),
                Statut = StaffStatus.Actif,

                Person = new Person
                {
                    Title = PersonTitles.Mr,
                    PersonGuid = new Guid("53f258a3-f931-4975-b6ec-17d26aa95848"),
                    FirstName = "Halid",
                    LastName = "cisse",
                    Nationality = "Mali",
                    IdentityNumber = "",
                    BirthDate = DateTime.Today.AddDays(-5000),
                    BirthPlace = "Tayba",
                    PhoneNumber = "0012547874",
                    EmailAdress = "halid@gmail.com",
                    HomeAdress = "Mabella",
                    RegistrationDate = DateTime.Today.AddDays(-100)
                }
            };

            ef.Staffs.AddOrUpdate(admin);

            var x = ef.Set<Person>().ToList();
            x.ForEach(s => s.PhotoIdentity = GetRandomImg());

            MessageBox.Show(Resources.Seed_Done);
        }

        private static byte[] GetRandomImg()
        {
            var x = RandomHelper.Random(1, 22);

            var imgName = "portrait" + x;

            var img = (Image)Resources.ResourceManager.GetObject(imgName, CultureInfo.InvariantCulture);

            return img == null ? null : ImagesHelper.ImageToByteArray(img);
        }




    }
}
