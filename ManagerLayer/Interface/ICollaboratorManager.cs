using CommonLayer.Models;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interface
{
    public interface ICollaboratorManager
    {
        public Collaborator CreateCollaborator(int UserId, CollaboratorModel model);
        public List<Collaborator> GetCollaborators(int UserId);
        public bool DeleteCollaborator(int UserId, int CollaboratorId);
    }
}
