using System;
using System.Collections;
using System.Linq;
using eStationCore.IManagers;
using eStationCore.Model;
using eStationCore.Model.Common.Entity;

namespace eStationCore.Store.SqlServer {

    /// <summary>
    /// Gestion des Documents
    /// </summary>
    public sealed class DocumentsManager : IDocumentsManager
    {
        private readonly StationContext Db;

        public DocumentsManager(StationContext stationContext)
        {
            Db = stationContext;
        }

        /// <summary>
        /// Represente un enseignant, proff, staff, qui a la possibilite de se connecter a l'Eschool
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        //[PrincipalPermission(SecurityAction.Demand, Role = Clearances.StaffWrite)]
        public bool SaveDocument (Document document) {                      
            using (var db = new StationContext()) {
                if(document.DocumentGuid==Guid.Empty)
                    document.DocumentGuid=Guid.NewGuid();
                
                db.Set<Document>().Add(document);
                return db.SaveChanges()>0;
            }
        }
      

        /// <summary>
        /// Supprimer definitivement un document
        /// </summary>
        /// <returns></returns>
        //[PrincipalPermission(SecurityAction.Demand, Role = Clearances.StaffDelete)]
        public bool DeleteDocument (Guid documentGuid) {
            using (var db = new StationContext()) {
                db.Set<Document>().Remove(db.Set<Document>().Find(documentGuid));
                return db.SaveChanges()>0;               
            }
        }


        /// <summary>
        /// Telecharger un document
        /// </summary>
        /// <param name="documentGuid"></param>
        /// <returns></returns>
        public Document DownloadDocument (Guid documentGuid) {
            using (var db = new StationContext()) {
                return db.Set<Document>().Find(documentGuid);
            }
        }


        /// <summary>
        /// La list des Documents d'une Personne
        /// </summary>
        /// <param name="personGuid"></param>
        /// <param name="maxResult"></param>
        /// <returns></returns>
        public IEnumerable GetPersonDocuments(Guid personGuid, int maxResult = 7)
        {
            using (var db = new StationContext()) {
                return (db.Set<Person>().Find(personGuid).Documents).Select(
                        d => new Document {DocumentGuid = d.DocumentGuid, DocumentName = d.DocumentName, Description = d.Description, FileType = d.FileType }).OrderBy(d=> d.DateAdded);                
            }
        }


    }
}
