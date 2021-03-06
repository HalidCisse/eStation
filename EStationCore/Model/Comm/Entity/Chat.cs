﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eLib.Database.Interfaces;
using eStationCore.Model.Common.Entity;

namespace eStationCore.Model.Comm.Entity {

    /// <summary>
    /// Talk, Chat, Dicussion
    /// </summary>
    public class Chat: Tracable {

        /// <summary>
        /// ConversationGuid
        /// </summary>
        [Key]
        public Guid ChatGuid { get; set; }
              
        /// <summary>
        /// Suject
        /// </summary>
        public string Subject { get; set; }
        


        /// <summary>
        /// Les Particioants a la Conversation 
        /// </summary>
        public virtual ICollection<Person> Persons { get; set; }= new HashSet<Person>();

        /// <summary>
        /// Les Messages de la Conversation 
        /// </summary>
        public virtual ICollection<Message> Messages { get; set; }= new HashSet<Message>();

    }
}
