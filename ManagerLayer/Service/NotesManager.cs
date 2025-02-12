using CommonLayer.Models;
using ManagerLayer.Interface;
using Repository.Entity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Service
{
    public class NotesManager:INotesManager
    {
        private readonly INotesRepository notesRepository;

        public NotesManager(INotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }
        public Notes CreateNote(int UserId, NotesModel notesModel)
        {
            return notesRepository.CreateNote(UserId, notesModel);
        }

        public List<Notes> GetAllNotes(int UserId)
        {
            return notesRepository.GetAllNotes(UserId);
        }

        public bool UpdateNote(int NotesId, int UserId, UpdateNoteModel updateNoteModel)
        {
            return notesRepository.UpdateNote(NotesId, UserId, updateNoteModel);
        }
        public bool DeleteNote(int NotesId, int UserId)
        {
            return notesRepository.DeleteNote(NotesId, UserId);
        }

        public bool CheckPin(int NotesId, int UserId)
        {
            return notesRepository.CheckPin(NotesId, UserId);
        }

        public bool CheckArchive(int NotesId, int UserId)
        {
            return notesRepository.CheckArchive(NotesId, UserId);
        }

        public bool CheckTrash(int NotesId, int UserId)
        {
            return notesRepository.CheckTrash(NotesId, UserId);
        }

        public string UpdateColor(int NotesId, int UserId, string Color)
        {
            return notesRepository.UpdateColor(NotesId, UserId, Color);
        }

        public bool UpdateImage(int NotesId, int UserId, string ImagePath)
        {
            return notesRepository.UpdateImage(NotesId, UserId, ImagePath); 
        }
        public bool UpdateReminder(int NotesId, int UserId, DateTime reminder)
        {
            return notesRepository.UpdateReminder(NotesId, UserId, reminder);
        }

    }
}
