using System.ComponentModel;

namespace eStationCore.Model.Security.Enums
{
    
    public enum AdminClearances
    {
      
        [Description("Gerer les Roles")]
        SuperUser,

        
        [Description("Ajouter des Personnels")]
        StaffWrite,

        
        [Description("Supprimer des Personnels")]
        StaffDelete,

        
        [Description("Ajouter des Nouveaux Clients")]
        CustomerWrite,

        
        [Description("Archiver des Clients")]
        CustomerDelete,

        
        [Description("Ajouter des Salaires")]
        FinanceWrite,

        
        [Description("Ajouter des Depenses/Recettes")]
        Treasurer,

        

    }
}
