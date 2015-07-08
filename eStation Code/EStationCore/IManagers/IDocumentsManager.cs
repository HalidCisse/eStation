using System;
using System.Collections;
using eStationCore.Model.Common.Entity;

namespace eStationCore.IManagers
{
    public interface IDocumentsManager
    {
        bool DeleteDocument(Guid documentGuid);
        Document DownloadDocument(Guid documentGuid);
        IEnumerable GetPersonDocuments(Guid personGuid, int maxResult = 7);
        bool SaveDocument(Document document);
    }
}