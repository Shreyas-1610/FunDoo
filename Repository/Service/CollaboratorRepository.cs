using CommonLayer.Models;
using Repository.Context;
using Repository.Entity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Service
{
    public class CollaboratorRepository:ICollaboratorRepository
    {
        private readonly FunDooDbContext context;
        public CollaboratorRepository(FunDooDbContext context)
        {
            this.context = context;
        }

        public Collaborator CreateCollaborator(int UserId, CollaboratorModel model)
        {
            var note = context.Notes.FirstOrDefault(n => n.NotesId == model.NotesId && n.UserId == UserId);
            if (note == null)
            {
                throw new Exception("Only the owner of the note can add collaborators.");
            }
            Collaborator collaborator = new Collaborator();
            collaborator.Email = model.Email;
            collaborator.NotesId = model.NotesId;
            collaborator.UserId = UserId;

            context.Collaborator.Add(collaborator);
            context.SaveChanges();
            return collaborator;

        }

        public List<Collaborator> GetCollaborators(int UserId)
        {
            var allCollaborators = context.Collaborator.Where(
                c => context.Notes.Any(n => n.NotesId == c.NotesId && n.UserId == UserId)).ToList();

            return allCollaborators;
        }

        public bool DeleteCollaborator(int UserId, int CollaboratorId)
        {
            var user = context.Collaborator.FirstOrDefault(c => c.UserId == UserId && c.CollaboratorId == CollaboratorId);
            if (user == null)
            {
                return false;
            }
            var checkOwner = context.Notes.FirstOrDefault(c => c.NotesId == user.NotesId && c.UserId == UserId);
            if (checkOwner == null)
            {
                throw new Exception("Not authorized");
            }
            context.Collaborator.Remove(user);
            context.SaveChanges();
            return true;
        }
    }
}
