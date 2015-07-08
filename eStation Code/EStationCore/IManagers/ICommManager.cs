using System;
using System.Collections.Generic;
using eStationCore.Model.Comm.Entity;
using eStationCore.Model.Comm.Views;
using eStationCore.Model.Common.Entity;

namespace eStationCore.IManagers
{
    public interface ICommManager
    {
        bool DeleteChat(Guid chatGuid);
        bool DeleteMessage(Guid messageGuid);
        Document DownloadAttachement(Guid messageGuid);
        IEnumerable<ChatCard> GetChats(Guid personGuid, DateTime? fromDate = default(DateTime?), DateTime? toDate = default(DateTime?), int maxResult = 20);
        Conversation GetMessage(Guid messageGuid, bool markRead = true);
        IEnumerable<MessageCard> GetMessages(Guid chatGuid, DateTime? fromDate = default(DateTime?), DateTime? toDate = default(DateTime?), int maxChats = 20, bool markRead = true);
        bool PushChat(Message newMessage);
        bool SaveChat(Chat newChat);
        bool SendMessage(Conversation conversation);
    }
}