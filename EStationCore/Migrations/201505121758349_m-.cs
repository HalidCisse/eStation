using System.Data.Entity.Migrations;

namespace eStationCore.Migrations
{
    public partial class m : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Citernes",
                c => new
                    {
                        CiterneGuid = c.Guid(nullable: false),
                        Numero = c.String(),
                        TypeCarburant = c.Int(nullable: false),
                        Capaciter = c.Int(nullable: false),
                        Quantiter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CiterneGuid);
            
            CreateTable(
                "dbo.Pompes",
                c => new
                    {
                        PompeGuid = c.Guid(nullable: false),
                        ColonneGuid = c.Guid(nullable: false),
                        CiterneGuid = c.Guid(nullable: false),
                        CompteurMecanique = c.Int(nullable: false),
                        CompteurElectronique1 = c.Double(nullable: false),
                        CompteurElectronique2 = c.Double(nullable: false),
                        CompteurElectronique3 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.PompeGuid)
                .ForeignKey("dbo.Citernes", t => t.CiterneGuid, cascadeDelete: true)
                .ForeignKey("dbo.Colonnes", t => t.ColonneGuid, cascadeDelete: true)
                .Index(t => t.ColonneGuid)
                .Index(t => t.CiterneGuid);
            
            CreateTable(
                "dbo.Colonnes",
                c => new
                    {
                        ColonneGuid = c.Guid(nullable: false),
                        Nom = c.String(),
                    })
                .PrimaryKey(t => t.ColonneGuid);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerGuid = c.Guid(nullable: false),
                        PersonGuid = c.Guid(nullable: false),
                        Matricule = c.String(),
                        Company = c.String(),
                        CustomerStatus = c.Int(nullable: false),
                        AddUserGuid = c.Guid(nullable: false),
                        DateAdded = c.DateTime(),
                        LastEditDate = c.DateTime(),
                        LastEditUserGuid = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteUserGuid = c.Guid(nullable: false),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CustomerGuid)
                .ForeignKey("dbo.People", t => t.PersonGuid, cascadeDelete: true)
                .Index(t => t.PersonGuid);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        AchatGuid = c.Guid(nullable: false),
                        CustomerGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AchatGuid)
                .ForeignKey("dbo.Customers", t => t.CustomerGuid, cascadeDelete: true)
                .Index(t => t.CustomerGuid);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductGuid = c.Guid(nullable: false),
                        Purchase_AchatGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.ProductGuid)
                .ForeignKey("dbo.Purchases", t => t.Purchase_AchatGuid)
                .Index(t => t.Purchase_AchatGuid);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonGuid = c.Guid(nullable: false),
                        Title = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhotoIdentity = c.Binary(),
                        HealthState = c.Int(nullable: false),
                        Nationality = c.String(),
                        IdentityNumber = c.String(),
                        BirthDate = c.DateTime(),
                        BirthPlace = c.String(),
                        PhoneNumber = c.String(),
                        EmailAdress = c.String(),
                        HomeAdress = c.String(),
                        RegistrationDate = c.DateTime(),
                        AddUserGuid = c.Guid(nullable: false),
                        DateAdded = c.DateTime(),
                        LastEditDate = c.DateTime(),
                        LastEditUserGuid = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteUserGuid = c.Guid(nullable: false),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PersonGuid);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentGuid = c.Guid(nullable: false),
                        PersonGuid = c.Guid(nullable: false),
                        DocumentName = c.String(),
                        Description = c.String(),
                        FileType = c.Int(nullable: false),
                        DataBytes = c.Binary(),
                        AddUserGuid = c.Guid(nullable: false),
                        DateAdded = c.DateTime(),
                        LastEditDate = c.DateTime(),
                        LastEditUserGuid = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteUserGuid = c.Guid(nullable: false),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentGuid)
                .ForeignKey("dbo.People", t => t.PersonGuid, cascadeDelete: true)
                .Index(t => t.PersonGuid);
            
            CreateTable(
                "dbo.Employments",
                c => new
                    {
                        EmploymentGuid = c.Guid(nullable: false),
                        StaffGuid = c.Guid(nullable: false),
                        Position = c.String(),
                        Category = c.String(),
                        Grade = c.String(),
                        Departement = c.String(),
                        Division = c.String(),
                        Project = c.String(),
                        ReportTo = c.String(),
                        SalaryRecurrence = c.Int(nullable: false),
                        PayType = c.Int(nullable: false),
                        HourlyPay = c.Double(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Description = c.String(),
                        AddUserGuid = c.Guid(nullable: false),
                        DateAdded = c.DateTime(),
                        LastEditDate = c.DateTime(),
                        LastEditUserGuid = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteUserGuid = c.Guid(nullable: false),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmploymentGuid);
            
            CreateTable(
                "dbo.Huiles",
                c => new
                    {
                        HuileGuid = c.Guid(nullable: false),
                        TypeHuile = c.Int(nullable: false),
                        Seuil = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.HuileGuid);
            
            CreateTable(
                "dbo.Payrolls",
                c => new
                    {
                        PayrollGuid = c.Guid(nullable: false),
                        EmploymentGuid = c.Guid(nullable: false),
                        Designation = c.String(),
                        PaycheckDate = c.DateTime(),
                        IsPaid = c.Boolean(nullable: false),
                        IsPaidTo = c.Guid(nullable: false),
                        DatePaid = c.DateTime(),
                        HoursWorkedTicks = c.Long(nullable: false),
                        FinalPaycheck = c.Double(nullable: false),
                        NumeroReference = c.String(),
                        PaymentMethode = c.Int(nullable: false),
                        Description = c.String(),
                        AddUserGuid = c.Guid(nullable: false),
                        DateAdded = c.DateTime(),
                        LastEditDate = c.DateTime(),
                        LastEditUserGuid = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteUserGuid = c.Guid(nullable: false),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PayrollGuid);
            
            CreateTable(
                "dbo.Salaries",
                c => new
                    {
                        SalaryGuid = c.Guid(nullable: false),
                        EmploymentGuid = c.Guid(nullable: false),
                        Designation = c.String(),
                        Remuneration = c.Double(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Description = c.String(),
                        AddUserGuid = c.Guid(nullable: false),
                        DateAdded = c.DateTime(),
                        LastEditDate = c.DateTime(),
                        LastEditUserGuid = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteUserGuid = c.Guid(nullable: false),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SalaryGuid);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffGuid = c.Guid(nullable: false),
                        PersonGuid = c.Guid(nullable: false),
                        Matricule = c.String(),
                        PositionPrincipale = c.String(),
                        DepartementPrincipale = c.String(),
                        Division = c.String(),
                        Qualification = c.String(),
                        Diploma = c.String(),
                        DiplomaLevel = c.String(),
                        Experiences = c.Int(nullable: false),
                        FormerJob = c.String(),
                        Grade = c.String(),
                        HiredDate = c.DateTime(),
                        Statut = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StaffGuid)
                .ForeignKey("dbo.People", t => t.PersonGuid, cascadeDelete: true)
                .Index(t => t.PersonGuid);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionGuid = c.Guid(nullable: false),
                        TransactionReference = c.String(),
                        Designation = c.String(),
                        PaidToward = c.String(),
                        Amount = c.Double(nullable: false),
                        PaymentMethode = c.Int(nullable: false),
                        NumeroVirement = c.String(),
                        Bank = c.String(),
                        TransactionDate = c.DateTime(),
                        Description = c.String(),
                        AddUserGuid = c.Guid(nullable: false),
                        DateAdded = c.DateTime(),
                        LastEditDate = c.DateTime(),
                        LastEditUserGuid = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteUserGuid = c.Guid(nullable: false),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransactionGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "PersonGuid", "dbo.People");
            DropForeignKey("dbo.Customers", "PersonGuid", "dbo.People");
            DropForeignKey("dbo.Documents", "PersonGuid", "dbo.People");
            DropForeignKey("dbo.Products", "Purchase_AchatGuid", "dbo.Purchases");
            DropForeignKey("dbo.Purchases", "CustomerGuid", "dbo.Customers");
            DropForeignKey("dbo.Pompes", "ColonneGuid", "dbo.Colonnes");
            DropForeignKey("dbo.Pompes", "CiterneGuid", "dbo.Citernes");
            DropIndex("dbo.Staffs", new[] { "PersonGuid" });
            DropIndex("dbo.Documents", new[] { "PersonGuid" });
            DropIndex("dbo.Products", new[] { "Purchase_AchatGuid" });
            DropIndex("dbo.Purchases", new[] { "CustomerGuid" });
            DropIndex("dbo.Customers", new[] { "PersonGuid" });
            DropIndex("dbo.Pompes", new[] { "CiterneGuid" });
            DropIndex("dbo.Pompes", new[] { "ColonneGuid" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Staffs");
            DropTable("dbo.Salaries");
            DropTable("dbo.Payrolls");
            DropTable("dbo.Huiles");
            DropTable("dbo.Employments");
            DropTable("dbo.Documents");
            DropTable("dbo.People");
            DropTable("dbo.Products");
            DropTable("dbo.Purchases");
            DropTable("dbo.Customers");
            DropTable("dbo.Colonnes");
            DropTable("dbo.Pompes");
            DropTable("dbo.Citernes");
        }
    }
}
