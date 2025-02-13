using CommonLayer.Models;
using ManagerLayer.Interface;
using Repository.Entity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Service
{
    public class CollaboratorManager:ICollaboratorManager
    {
        private readonly ICollaboratorRepository repository;
        public CollaboratorManager(ICollaboratorRepository repository)
        {
            this.repository = repository;
        }
        public Collaborator CreateCollaborator(int UserId, CollaboratorModel model)
        {
            return repository.CreateCollaborator(UserId, model);
        }
        public List<Collaborator> GetCollaborators(int UserId)
        {
            return repository.GetCollaborators(UserId);
        }
        public bool DeleteCollaborator(int UserId, int CollaboratorId)
        {
            return repository.DeleteCollaborator(UserId, CollaboratorId);
        }
    }
}
